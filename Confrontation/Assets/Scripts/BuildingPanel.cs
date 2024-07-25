using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Entities;
using FuryLion.UI;
using Interfaces;
using UnityEngine;
using Views;

public class BuildingPanel : Element, IRecyclable
{
    [SerializeField] private List<Sprite> _sprites;
    
    private List<BuildingButton> _buttons = new List<BuildingButton>();

    public BaseEntity Target { get; private set; }

    private readonly Color _color = new Color(170, 46, 0, 255);

    private void Awake()
    {
        SetData();
        Hide();
        foreach (var b in _buttons)
            b.Click += () => OnBuildingButtonCLick(b);
    }
    
    private void OnBuildingButtonCLick(BuildingButton button)
    {
        switch (Target)
        {
            case CellEntity cellEntity:
                ShopManager.Buy(cellEntity, button.Type);
                break;
            case IBuilding building:
                ShopManager.Upgrade(building);
                break;
        }
    }

    public void Show(BaseView target)
    {
        switch (target)
        {
            case CellView cellView when cellView.CellEntity.TeamID != 1 || cellView.CellEntity.Building != null:
            case BuildingView buildingView when 
                buildingView.BuildingEntity.Level > 4 || buildingView.BuildingEntity.TeamID != 1 || buildingView is ForgeView:
                return;
        }

        Target = target switch
        {
            CellView cellView => cellView.CellEntity,
            BuildingView buildingView => buildingView.BuildingEntity as BaseEntity,
            _ => Target
        };

        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 
            transform.position.z);
        gameObject.SetActive(true);

        PlaceButtons(target is BuildingView);
        ShowCost(target as BuildingView);
    }

    private void PlaceButtons(bool isBuilding)
    {
        if (isBuilding)
            foreach (var button in _buttons)
                button.gameObject.SetActive(button.Type == BuildingType.Upgrade);
        else
            foreach (var button in _buttons)
                button.gameObject.SetActive(button.Type != BuildingType.Upgrade);
    }

    private void ShowCost(BuildingView target)
    {
        foreach (var button in _buttons)
        {
            if (button.Type != BuildingType.Upgrade)
                button.Cost = ShopManager.GetCost(button.Type).ToString();
            else if (target != null) 
                button.Cost = ShopManager.ActOnBuilding(target.BuildingEntity, 
                    type => ShopManager.GetCost(type, target.BuildingEntity.Level)).ToString();
        }
    }

    public void SwitchColor(CustomerController customer)
    {
        foreach (var button in _buttons)
        {
            button.CostColor = Target switch
            {
                IBuilding building => customer.Money < int.Parse(button.Cost)
                    ? Color.white
                    : _color,
                CellEntity cellEntity => customer.Money < int.Parse(button.Cost)
                    ? Color.white
                    : _color,
                _ => button.CostColor
            };
        }
    }

    private void SetData()
    {
        var center = transform.position;
        for (var i = 0; i < (int) BuildingType.Settlement; i++)
        {
            var angle =  i * 360 / (int) BuildingType.Settlement;
            var pos = PlaceInCircle(center, 0.9f ,angle);
            var button = Recycler.Get<BuildingButton>();
            button.transform.position = pos;
            button.Type = (BuildingType) Enum.GetValues(typeof(BuildingType)).GetValue(i);
            button.Spr = _sprites[i];
            button.transform.SetParent(transform);
            if(button.Type == BuildingType.Farm)
                ImageUtility.SetImageScale(button.Image, 0.8f, 1.1f);
            
            _buttons.Add(button);
        }

        var upgrade = Recycler.Get<BuildingButton>();
        upgrade.transform.position = center;
        upgrade.Type = BuildingType.Upgrade;
        upgrade.Spr = _sprites[_sprites.Count - 1];
        upgrade.transform.SetParent(transform);
        ImageUtility.SetImageScale(upgrade.Image, 1.8f, 1.8f);
        _buttons.Add(upgrade);
    }

    private static Vector3 PlaceInCircle(Vector3 center, float radius, int angle)
    {
        var pos = new Vector3();
        var (x, y, z) = center;
        pos.x = x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        pos.z = z;
        return pos;
    }

    public void Hide() => gameObject.SetActive(false);
}
