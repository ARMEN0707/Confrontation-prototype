using System;
using FuryLion;
using UnityEngine;

[CreateAssetMenu(fileName = "Building Library")]
public class BuildingLibrary : ScriptableObject
{
    public SerializedDictionary<BuildingType, BuildingInfo> Buildings;
}