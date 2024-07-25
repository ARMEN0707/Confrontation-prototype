using System;
using System.Collections.Generic;
using Core;
using Data;
using Newtonsoft.Json;
using UnityEngine;
using Views;

[CreateAssetMenu(fileName = "LevelsInfo", menuName = "Infos/LevelsInfo")]
[Serializable]
public class LevelsInfo : ScriptableObject
{
    public List<LevelInfo> Levels = new List<LevelInfo>();
    
    [JsonIgnore]
    public List<Sprite> TeamSprites = new List<Sprite>();
    [JsonIgnore]
    public CellView CellPrefab;
    [JsonIgnore]
    public BarracksView BarracksPrefab;
    [JsonIgnore]
    public FarmView FarmPrefab;
    [JsonIgnore]
    public StableView StablePrefab;
    [JsonIgnore]
    public ForgeView ForgePrefab;
    [JsonIgnore]
    public MineView MinePrefab;
    [JsonIgnore]
    public SettlementView SettlementPrefab;
    [JsonIgnore]
    public CapitalView CapitalPrefab;
    [JsonIgnore]
    public WizardTowerView WizardTowerPrefab;
}