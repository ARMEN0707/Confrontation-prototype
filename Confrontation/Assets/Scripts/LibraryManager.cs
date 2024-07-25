using System.Collections.Generic;
using FuryLion;
using UnityEngine;

public static class LibraryManager
{
    private const string BuildingsInfoFileName = "Levels/Building Library";
    
    private static SerializedDictionary<BuildingType, BuildingInfo> _buildingsInfo;

    public static SerializedDictionary<BuildingType, BuildingInfo> GetBuildingsInfo()
    {
        _buildingsInfo ??= Resources.Load<BuildingLibrary>(BuildingsInfoFileName).Buildings;
        return _buildingsInfo;
    }
}