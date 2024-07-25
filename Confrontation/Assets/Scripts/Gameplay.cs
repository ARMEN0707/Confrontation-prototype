using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FuryLion.UI;
using Core;
using Data;
using Entities;
using Interfaces;
using Systems;
using Views;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private LayerMask _buildingsMask;
    [SerializeField] private LayerMask _cellMask;
    
    [SerializeField] private float _minZoom = 4f;
    [SerializeField] private float _maxZoom = 8f;
    
    private static Gameplay _instance;
    
    private World _currentWorld;

    private bool _isPaused = true;
    private bool _isDragged;
    private Vector3 _mouseDownPos;
    
    private Vector2 _center;
    private Vector3 _size;

    private BuildingPanel _buildingPanel;

    private readonly List<IUnitController> _selectedBuildings = new List<IUnitController>();

    private void Awake()
    {
        _instance = this;
    }

    public static async void Init(int lvlIndex)
    {
        _instance._buildingPanel = Recycler.Get<BuildingPanel>();

        var config = LevelManager.GetLevelConfig(lvlIndex);
        _instance._center = config.Center;
        _instance._size = config.Size;
        
        var transform1 = CameraManager.GameCamera.transform;
        transform1.position = new Vector3(_instance._center.x, _instance._center.y, transform1.position.z);
        
        _instance._currentWorld = new World();
        var data = LevelManager.LoadWorld(lvlIndex);

        ShopManager.InitItems(LevelManager.BuildingsConfig);
        var aiControllers = new List<AIController>();
        foreach (var o in data.Objects)
        {
            if (!(o is TeamData t) || t.TeamID == 0)
                continue;
            
            if (t.TeamID == 1)
            {
                var customer = new CustomerController(t.TeamID, t.Money, t.Mana);
                ShopManager.AddCustomer(customer);
                GamePage.SetCustomer(customer);
                customer.UpdatedMoney += GamePage.OnUpdateMoney;
                customer.UpdatedMana += GamePage.OnUpdateMana;
                customer.UpdatedMoney += _instance._buildingPanel.SwitchColor;
                customer.UpdatedMoney += c => t.Money = c.Money;
                customer.UpdatedMana += c => t.Mana = c.Mana;
                continue;
            }

            var ai = new AIController(t.TeamID, t.Money, t.Mana);
            ai.UpdatedMoney += c => t.Money = c.Money;
            ai.UpdatedMana += c => t.Mana = c.Mana;
            ai.SendUnits += _instance.SendUnits;
            aiControllers.Add(ai);
            ShopManager.AddCustomer(ai);
        }
        
        var bonusSystem = new BonusSystem();
        var updateSystem = new UpdateSystem();
        var currencySystem = new CurrencySystem(ShopManager.Customers);
        var boostSystem = new BoostSystem();
        var teamChangeSystem = new TeamChangeSystem();
        var warFogSystem = new WarFogSystem();
        var borderSystem = new BorderSystem();

        teamChangeSystem.PassedLevel += () => OnEndedLevel(true);
        teamChangeSystem.LostLevel += () => OnEndedLevel(false);
        teamChangeSystem.ChangedTeamID += _instance.OnChangedTeamID;
        foreach (var ai in aiControllers)
        {
            updateSystem.AddUpdatableObject(ai);
            teamChangeSystem.ChangedTeamID += ai.OnChangedTeamID;
        }

        GamePage.UpdatedBoost += boostSystem.OnBoost;
        
        _instance._currentWorld.Init(data, _instance.transform, bonusSystem, teamChangeSystem, 
            warFogSystem, updateSystem, currencySystem, boostSystem, borderSystem);

        foreach (var c in ShopManager.Customers) 
            warFogSystem.UpdateAvailable(c.TeamID);
        
        borderSystem.FindNeighbours();
        await updateSystem.Update();
    }

    private static void OnEndedLevel(bool isWin)
    {
        SetPause(true);
        Deactivate();
        LevelManager.ResetLevel();
        EndGamePage.IsWin = isWin;
        PageManager.Open<EndGamePage>();
    }

    public static void SetPause(bool isPaused)
    {
        _instance._isPaused = isPaused;
        _instance._currentWorld.GetSystem<UpdateSystem>()?.SetPause(isPaused);
        UnitUtility.SetPause(isPaused);
    }

    public static void Deactivate()
    {
        _instance._selectedBuildings.Clear();
        _instance._currentWorld?.Deactivate();
        UnitUtility.Clear();
        ShopManager.Clear();
    }

    private void Update()
    {
        if (_isPaused)
        {
            if (Input.GetMouseButtonUp(0))
                HandleButton();

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _mouseDownPos = MouseManager.GetMousePosition(0, CameraManager.GameCamera);
            SelectBuilding();
        }

        if (Input.touchCount == 2)
            CameraManager.CameraZoom(Input.GetTouch(0), Input.GetTouch(1), CameraManager.GameCamera, _minZoom, _maxZoom);

        if (Input.GetMouseButtonUp(0))
        {
            if (HandleButton() != null)
                return;

            var button = MouseManager.GetObject<BaseButton>(_buildingsMask, CameraManager.GameCamera);
            if (button != null)
                button.OnClick();

            if (_isDragged)
            {
                _buildingPanel.Hide();
                var targetBuilding = MouseManager.GetObject<BuildingView>(_buildingsMask, CameraManager.GameCamera);
                if (targetBuilding is SettlementView && ShopManager.Customers.Find(c => c.TeamID == 1).
                    AvailableCells.Select(c => c.Building).Contains(targetBuilding.BuildingEntity))
                {
                    SendUnits(_selectedBuildings, targetBuilding.BuildingEntity);
                    HideLines();
                    _selectedBuildings.Clear();
                }
                
                HideLines();
                _isDragged = false;
            }
            else
            {
                HideLines();
                _selectedBuildings.Clear();
                HandleBuildingPanel();
            }
            
        }

        if (Input.GetMouseButton(0))
        {
            if (_mouseDownPos != MouseManager.GetMousePosition(0, CameraManager.GameCamera))
            {
                _isDragged = true;
                if(_selectedBuildings.Count > 0)
                    SelectBuilding();
                
                DrawLines();
                if (_selectedBuildings.Count == 0)
                    CameraManager.CameraMove(CameraManager.GameCamera, _mouseDownPos, _center, _size);
            }
        }
        
        //TODO закомментировать перед сборкой проекта
        CameraManager.Zoom(CameraManager.GameCamera, Input.GetAxis("Mouse ScrollWheel"), _minZoom, _maxZoom);
    }

    public static BaseEntity CreateNewObject(ObjectData data)
    {
        return _instance._currentWorld.CreateNewObject(data);
    }

    private BaseButton HandleButton()
    {
        BaseButton button = null;
        var popUpObj = MouseManager.GetObject<Element>(CameraManager.PopUpMask, CameraManager.PopUpCamera);
        var messageBoxObj = MouseManager.GetObject<Element>(CameraManager.MessageBoxMask, CameraManager.MessageBoxCamera);
        var pageObj = MouseManager.GetObject<Element>(CameraManager.PageMask, CameraManager.PageCamera);
        var academyObj = MouseManager.GetObject<Element>(CameraManager.AcademyMask, CameraManager.AcademyCamera);
        if (popUpObj != null)
        {
            if (popUpObj is BaseButton popUpButton)
                button = popUpButton;
        }
        else if (messageBoxObj != null)
        {
            if (messageBoxObj is BaseButton messageBoxButton)
                button = messageBoxButton;
        }
        else if (pageObj != null)
        {
            if (pageObj is BaseButton pageButton)
                button = pageButton;
        }
        else if (academyObj != null)
        {
            if (academyObj is BaseButton academyButton)
                button = academyButton;
        }

        if (button == null)
            return button;
        
        _buildingPanel?.Hide();
        button.OnClick();

        return button;
    }

    private void HandleBuildingPanel()
    {
        if (_buildingPanel.gameObject.activeSelf)
        {
            _buildingPanel.Hide();
            return;
        }

        var buildingView = MouseManager.GetObject<BuildingView>(_buildingsMask, CameraManager.GameCamera);
        if (buildingView != null)
            _buildingPanel.Show(buildingView);  
        else
        {
            var cellView = MouseManager.GetObject<CellView>(_cellMask, CameraManager.GameCamera);
            if (cellView != null)
                _buildingPanel.Show(cellView);
        }
    }

    private void SelectBuilding()
    {
        var building = MouseManager.GetObject<BuildingView>(_buildingsMask, CameraManager.GameCamera);
        if (building == null)
            return;

        if (!ShopManager.Customers.Find(c => c.TeamID == 1).
            AvailableCells.Select(c => c.Building).Contains(building.BuildingEntity))
            return;
        
        if (building.BuildingEntity.TeamID != 1)
            return;

        if (!(building.BuildingEntity is IUnitController unitController)) 
            return;

        if (!_selectedBuildings.Contains(unitController)) 
            _selectedBuildings.Add(unitController);
    }

    private void OnChangedTeamID(List<ICell> cells)
    {
        foreach (var cell in cells)
        {
            if (cell == _buildingPanel.Target || cell.Building == _buildingPanel.Target)
                _buildingPanel.Hide();
            
            if (!(cell.Building is IUnitController unitController))
                continue;

            unitController.SetActiveLine(false);
            if (_selectedBuildings.Contains(unitController))
                _selectedBuildings.Remove(unitController);
        }
    }

    private void SendUnits(List<IUnitController> selectedBuildings, IBuilding targetBuilding)
    {
        foreach (var b in selectedBuildings)
        {
            if (b == targetBuilding)
                continue;
                
            var teamID = b.TeamID;
            var curUnitCount = b.GetArmyCount();
            var debuffProtection = b.GetDebuffProtectionBonus();
            var speed = b.GetSpeed();
            var force = b.GetForce();
            var unitCount = b switch
            {
                ISettlement settlementEntity => curUnitCount / 2,
                IBarracks barracksEntity => curUnitCount,
                _ => 0
            };

            b.UpdateArmyCount(curUnitCount - unitCount);
            for (var i = 0; i < unitCount; i++)
            {
                var unit = _currentWorld.CreateNewObject(new UnitData()) as UnitEntity;
                if (unit == null)
                    continue;
                
                unit.Init(teamID, b.Position, targetBuilding.Position, speed, force, debuffProtection);
                unit.Move();
            }
        }
    }

    private void HideLines()
    {
        foreach (var building in _selectedBuildings)
            building.SetActiveLine(false);
    }

    private void DrawLines()
    {
        foreach (var building in _selectedBuildings)
        {
            building.SetActiveLine(true);
            building.SetLineEndPos(MouseManager.GetMousePosition(4, CameraManager.GameCamera));
        }
    }
}
