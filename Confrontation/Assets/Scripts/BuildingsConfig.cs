using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingConfig", menuName = "Infos/BuildingConfig")]
[Serializable]
public class BuildingsConfig : ScriptableObject
{
    public List<BuildingConfig> BuildingConfigs = new List<BuildingConfig>();
}