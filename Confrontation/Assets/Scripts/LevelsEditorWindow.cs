#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Views;

public class LevelsEditorWindow : EditorWindow
{
    private readonly Dictionary<CellView, CellData> _cellUnions = new Dictionary<CellView, CellData>();

    private ReorderableList _levels;
    private ReorderableList _cells;
    private ReorderableList _regions;
    private ReorderableList _teams;
    private ReorderableList _teamRegions;

    private Vector2 _pos;

    private int _selectedCell;
    private readonly List<string> _cellOptions = new List<string>();
    private readonly List<CellView> _freeCells = new List<CellView>();
    
    private int _selectedRegion;
    private readonly List<string> _regionOptions = new List<string>();
    private readonly List<RegionData> _freeRegions = new List<RegionData>();
    
    private int _selectedBuilding;
    private readonly List<string> _buildingOptions = new List<string>()
    {
        "None",
        "Barracks",
        "Farm",
        "Stable",
        "Forge",
        "Mine",
        "Settlement",
        "Capital",
        "WizardTower"
    };
    
    [MenuItem("Window/LevelsEditorWindow")]
    private static void Init() => GetWindow<LevelsEditorWindow>("LevelsEditorWindow", true);

    private void OnLevelUpdate()
    {
        _freeCells.Clear();
        _freeRegions.Clear();

        var level = LevelManager.LevelsInfo.Levels[_levels.index];
        
        foreach (var c in _cellUnions)
        {
            if (level.Regions.All(r => !r.Cells.Contains(c.Value)))
                _freeCells.Add(c.Key);
        }

        _cellOptions.Clear();
        _cellOptions.Add("None");
        foreach (var fc in _freeCells)
            _cellOptions.Add("Cell " + _cellUnions.Keys.ToList().IndexOf(fc));

        foreach (var r in level.Regions)
            if (level.Teams.All(t => !t.RegionsData.Contains(r)))
                _freeRegions.Add(r);

        _regionOptions.Clear();
        _regionOptions.Add("None");
        foreach (var fr in _freeRegions)
            _regionOptions.Add("Region " + level.Regions.IndexOf(fr));
        
        foreach (var t in level.Teams)
            foreach (var c in t.RegionsData.SelectMany(r => r.Cells))
                c.TeamID = t.TeamID;

        LevelManager.DestroyObjectsOfType<BuildingView>();
        foreach (var cell in _cellUnions)
        {
            if (cell.Value == default)
                continue;

            cell.Key.SetSprite(LevelManager.LevelsInfo.TeamSprites[cell.Value.TeamID]);
            InstantiateBuilding(cell.Value.Building);
        }
    }

    private void OnHierarchyChange()
    {
        var cells = FindObjectsOfType<CellView>().ToList();

        var deficient = cells.FindAll(c => !_cellUnions.Keys.Contains(c));
        foreach (var d in deficient) 
            _cellUnions.Add(d, default);

        var waste = _cellUnions.Keys.ToList().FindAll(c => !cells.Contains(c));
        foreach (var w in waste) 
            _cellUnions.Remove(w);

        if (deficient.Count != 0 || waste.Count != 0)
            OnLevelUpdate();
    }

    private void OnSelectionChange()
    {
        var selectedObj = Selection.activeObject;
        if (selectedObj.GameObject() != null && selectedObj.GameObject().TryGetComponent<CellView>(out var cell))
            _selectedCell = _freeCells.IndexOf(cell) + 1;
    }

    private void OnEnable()
    {
        LevelManager.LoadLevels();
        
        var cells = FindObjectsOfType<CellView>().ToList();
        foreach (var c in cells)
            _cellUnions.Add(c, default);
        
        _levels = new ReorderableList(LevelManager.LevelsInfo.Levels, typeof(LevelInfo),
            false, true, true, true)
        {
            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 2;
                EditorGUI.LabelField(rect, "Level " + (index + 1));
            },
            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Levels");
            },
            onAddCallback = list =>
            {
                _cells = default;
                _regions = default;
                _teamRegions = default;
                _teams = default;

                LevelManager.LevelsInfo.Levels.Add(new LevelInfo());
            },
            onRemoveCallback = list =>
            {
                _cells = default;
                _regions = default;
                _teamRegions = default;
                _teams = default;
                
                LevelManager.LevelsInfo.Levels.RemoveAt(list.index);

            },
            onSelectCallback = list =>
            {
                var level = LevelManager.LevelsInfo.Levels[list.index];
                _regions = new ReorderableList(level.Regions, typeof(RegionData),
                    false, true, true, true)
                {
                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        rect.y += 2;
                        EditorGUI.LabelField(rect, "Region " + index);
                    },
                    drawHeaderCallback = rect =>
                    {
                        EditorGUI.LabelField(rect, "Regions");
                    },
                    onAddCallback = rList =>
                    {
                        _cells = default;
                        level.Regions.Add(new RegionData());
                    },
                    onRemoveCallback = rList =>
                    {
                        _cells = default;
                        level.Regions.RemoveAt(rList.index);
                    },
                    onSelectCallback = rList =>
                    {
                        _cells = new ReorderableList(level.Regions[rList.index].Cells, typeof(CellData),
                            false, true, true, true)
                        {
                            drawElementCallback = (rect, index, isActive, isFocused) =>
                            {
                                rect.y += 2;

                                rect.xMax /= 2;
                                EditorGUI.LabelField(rect,
                                    "Cell " + _cellUnions.Values.ToList()
                                        .IndexOf(level.Regions[rList.index].Cells[index]));

                                rect.x = rect.xMax;
                                EditorGUI.LabelField(rect, level.Regions[rList.index].Cells[index]
                                    .Building?.GetType().ToString());
                            },
                            drawHeaderCallback = rect =>
                            {
                                EditorGUI.LabelField(rect, "Cells");
                            },
                            onSelectCallback = cList =>
                            {
                                foreach (var c in _cellUnions.Keys)
                                    c.SetActiveOutline(false);
                                
                                var highlightingCell = _cellUnions.FirstOrDefault(c => 
                                    c.Value == level.Regions[rList.index].Cells[cList.index]).Key;
                                if (highlightingCell != null)
                                    highlightingCell.SetActiveOutline(true);

                                BuildingData building = _selectedBuilding switch
                                {
                                    1 => new BarracksData(),
                                    2 => new FarmData(),
                                    3 => new StableData(),
                                    4 => new ForgeData(),
                                    5 => new MineData(),
                                    6 => new SettlementData(),
                                    7 => new CapitalData(),
                                    8 => new WizardTowerData(),
                                    _ => default
                                };
                                if (building != default)
                                    building.Position = highlightingCell.Position - new Vector3(0, 0, 2);
                                
                                level.Regions[rList.index].Cells[cList.index].Building = building;
                                OnLevelUpdate();
                            },
                            onAddCallback = cList =>
                            {
                                if (_selectedCell == 0)
                                    return;

                                var cell = _freeCells[_selectedCell - 1];
                                _cellUnions[cell] ??= new CellData();
                                _cellUnions[cell].Position = cell.transform.position;

                                level.Regions[rList.index].Cells.Add(_cellUnions[cell]);
                                OnLevelUpdate();
                            },
                            onRemoveCallback = cList =>
                            {
                                level.Regions[rList.index].Cells.RemoveAt(cList.index);
                                OnLevelUpdate();
                            }
                        };
                    }
                };

                _teams = new ReorderableList(level.Teams, typeof(TeamData),
                    false, true, true, true)
                {
                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        rect.y += 2;

                        rect.xMax /= 3;
                        level.Teams[index].TeamID = EditorGUI.IntField(rect, 
                            "Team Number", level.Teams[index].TeamID);
                        
                        rect.x = rect.xMax;
                        level.Teams[index].Money = EditorGUI.IntField(rect, "Money", level.Teams[index].Money);

                        rect.x = rect.xMax;
                        level.Teams[index].Mana = EditorGUI.IntField(rect, "Mana", level.Teams[index].Mana);
                    },
                    drawHeaderCallback = rect =>
                    {
                        EditorGUI.LabelField(rect, "Teams");
                    },
                    onAddCallback = tList =>
                    {
                        _teamRegions = default;
                        level.Teams.Add(new TeamData());
                    },
                    onRemoveCallback = tList =>
                    {
                        _teamRegions = default;
                        level.Teams.RemoveAt(tList.index);
                    },
                    onSelectCallback = tList =>
                    {
                        _teamRegions = new ReorderableList(level.Teams[tList.index].RegionsData, typeof(RegionData), 
                            false, true, true, true)
                        {
                            drawElementCallback = (rect, index, isActive, isFocused) =>
                            {
                                rect.y += 2;
                                EditorGUI.LabelField(rect,
                                    "Region " + level.Regions.IndexOf(level.Teams[tList.index]
                                        .RegionsData[index]));
                            },
                            drawHeaderCallback = rect =>
                            {
                                EditorGUI.LabelField(rect, "Team " + level.Teams[tList.index].TeamID);
                            },
                            onAddCallback = trList =>
                            {
                                if (_selectedRegion == 0)
                                    return;

                                level.Teams[tList.index].RegionsData.Add(_freeRegions[_selectedRegion - 1]);
                                OnLevelUpdate();
                            },
                            onRemoveCallback = trList =>
                            {
                                level.Teams[tList.index].RegionsData.RemoveAt(trList.index);
                                OnLevelUpdate();
                            }
                        };

                        OnLevelUpdate();
                    }
                };
            }
        };
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        _pos = EditorGUILayout.BeginScrollView(_pos);

        GUILayout.Label("Level Settings", EditorStyles.boldLabel);

        if (_levels != null)
        {
            _levels.DoLayoutList();
            if (GUILayout.Button("Load"))
                LoadLevel(_levels.index);

            if (GUILayout.Button("Save"))
                LevelManager.SaveLevels();

            if (_levels.index >= 0 && _levels.index < _levels.count)
            {
                LevelManager.LevelsInfo.Levels[_levels.index].Center = EditorGUILayout.Vector2Field("Center",
                    LevelManager.LevelsInfo.Levels[_levels.index].Center);
                LevelManager.LevelsInfo.Levels[_levels.index].Size = EditorGUILayout.Vector2Field("Size",
                    LevelManager.LevelsInfo.Levels[_levels.index].Size);
                LevelManager.LevelsInfo.Levels[_levels.index].Reward = EditorGUILayout.IntField("Reward",
                    LevelManager.LevelsInfo.Levels[_levels.index].Reward);
                
                var center = LevelManager.LevelsInfo.Levels[_levels.index].Center;
                var (x, y) = LevelManager.LevelsInfo.Levels[_levels.index].Size;
                
                var leftBottom = center + new Vector2(-x / 2, -y / 2);
                var leftUp = center + new Vector2(-x / 2, y / 2);
                var rightUp = center + new Vector2(x / 2, y / 2);
                var rightBottom = center + new Vector2(x / 2, -y / 2);
                
                Debug.DrawLine(leftBottom, leftUp);
                Debug.DrawLine(leftUp, rightUp);
                Debug.DrawLine(rightUp, rightBottom);
                Debug.DrawLine(rightBottom, leftBottom);
            }
        }
        
        _regions?.DoLayoutList();

        if (_cells != null)
        {
            EditorGUI.BeginChangeCheck();
            _selectedCell = EditorGUILayout.Popup("Add Cell", _selectedCell, _cellOptions.ToArray());
            EditorGUI.EndChangeCheck();
        
            EditorGUI.BeginChangeCheck();
            _selectedBuilding = EditorGUILayout.Popup("Add Building", _selectedBuilding, _buildingOptions.ToArray());
            EditorGUI.EndChangeCheck();
        
            _cells.DoLayoutList();
        }
        
        _teams?.DoLayoutList();

        if (_teamRegions != null)
        {
            EditorGUI.BeginChangeCheck();
            _selectedRegion = EditorGUILayout.Popup("Add Region", _selectedRegion, _regionOptions.ToArray());
            EditorGUI.EndChangeCheck();
        
            _teamRegions.DoLayoutList();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void InstantiateBuilding(BuildingData buildingData)
    {
        if (buildingData == default)
            return;

        var buildingPosition = buildingData.Position;
        switch (buildingData)
        {
            case BarracksData barracksData:
                Instantiate(LevelManager.LevelsInfo.BarracksPrefab).Position = buildingPosition;
                break;
            case FarmData farmData:
                Instantiate(LevelManager.LevelsInfo.FarmPrefab).Position = buildingPosition;
                break;
            case StableData stableData:
                Instantiate(LevelManager.LevelsInfo.StablePrefab).Position = buildingPosition;
                break;
            case ForgeData forgeData:
                Instantiate(LevelManager.LevelsInfo.ForgePrefab).Position = buildingPosition;
                break;
            case MineData mineData:
                Instantiate(LevelManager.LevelsInfo.MinePrefab).Position = buildingPosition;
                break;
            case CapitalData capitalData:
                Instantiate(LevelManager.LevelsInfo.CapitalPrefab).Position = buildingPosition;
                break;
            case SettlementData settlementData:
                Instantiate(LevelManager.LevelsInfo.SettlementPrefab).Position = buildingPosition;
                break;
            case WizardTowerData wizardTowerData:
                Instantiate(LevelManager.LevelsInfo.WizardTowerPrefab).Position = buildingPosition;
                break;
        }
    }

    private void LoadLevel(int lvlIndex)
    {
        if (lvlIndex < 0 || lvlIndex >= LevelManager.LevelsInfo.Levels.Count)
            return;

        LevelManager.DestroyObjectsOfType<CellView>();
        LevelManager.DestroyObjectsOfType<BuildingView>();
        
        _cellUnions.Clear();

        var lvl = LevelManager.LevelsInfo.Levels[lvlIndex];
        foreach (var r in lvl.Regions)
        {
            foreach (var c in r.Cells)
            {
                var cell = Instantiate(LevelManager.LevelsInfo.CellPrefab);
                cell.transform.position = c.Position;
                cell.SetSprite(LevelManager.LevelsInfo.TeamSprites[c.TeamID]);
                _cellUnions.Add(cell, c);
                InstantiateBuilding(c.Building);
            }
        }
    }
}
#endif