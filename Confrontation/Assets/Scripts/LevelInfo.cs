using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

[Serializable]
public class LevelInfo
{
    public Vector2 Center;
    public Vector2 Size;
    public List<RegionData> Regions = new List<RegionData>();
    public List<TeamData> Teams = new List<TeamData>();
    public int Reward;
}