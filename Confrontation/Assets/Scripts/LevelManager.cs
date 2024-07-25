using System.IO;
using Core;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using Views;

public static class LevelManager
{
    private const string LevelsInfoFileName = "LevelsInfo";
    private static readonly string LevelsInfoFilePath = Path.Combine(Application.dataPath, "Resources", "LevelsInfo.txt");
    private static readonly string LevelsFilePath = Path.Combine(Application.persistentDataPath, "Levels.txt");
    public static readonly string PlayerDataFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.txt");

    public static readonly LevelsInfo LevelsInfo = Resources.Load<LevelsInfo>("Levels/LevelsInfo");
    public static readonly BuildingsConfig BuildingsConfig = Resources.Load<BuildingsConfig>("Levels/BuildingConfig");
    public static readonly PlayerData PlayerData = Storage.Load<PlayerData>(PlayerDataFilePath);

    private static WorldsData _worlds;
    public static int CurrentLevel { get; private set; }

    public static int RewardCurrentLevel => LevelsInfo.Levels[CurrentLevel].Reward;

    public static LevelInfo GetLevelConfig(int lvlIndex) =>
        Storage.LoadConfig<LevelsInfo>(LevelsInfoFileName).Levels[lvlIndex];

    public static WorldData LoadWorld(int lvlIndex)
    {
        DestroyObjectsOfType<CellView>();
        DestroyObjectsOfType<BuildingView>();
        DestroyObjectsOfType<UnitView>();
        DestroyObjectsOfType<BuildingPanel>();

        CurrentLevel = lvlIndex;
        _worlds = Storage.Load<WorldsData>(LevelsFilePath) ?? new WorldsData();
        if (_worlds.Worlds.Count == 0)
        {
            LoadLevels();
            foreach (var l in LevelsInfo.Levels)
            {
                var data = new WorldData();
                data.Objects.AddRange(l.Regions);
                data.Objects.AddRange(l.Teams);
                _worlds.Worlds.Add(data);
            }
        }

        return _worlds.Worlds[lvlIndex];
    }

    public static void SaveWorlds()
    {
        foreach (var w in _worlds.Worlds)
            w.Objects = w.Objects.FindAll(o => o is RegionData || o is TeamData || o is UnitData);

        Storage.Save(LevelsFilePath, _worlds);
    }

    public static int ResetLevel()
    {
        LoadLevels();
        var lvl = LevelsInfo.Levels[CurrentLevel];
        _worlds.Worlds[CurrentLevel].Objects.Clear();
        _worlds.Worlds[CurrentLevel].Objects.AddRange(lvl.Regions);
        _worlds.Worlds[CurrentLevel].Objects.AddRange(lvl.Teams);

        SaveWorlds();
        return CurrentLevel;
    }

    public static void LoadLevels() => LevelsInfo.Levels = Storage.LoadConfig<LevelsInfo>(LevelsInfoFileName).Levels;

    public static void SaveLevels() => Storage.Save(LevelsInfoFilePath, LevelsInfo);

    public static void DestroyObjectsOfType<T>() where T : Object
    {
        var objects = Object.FindObjectsOfType<T>();
        for (var i = objects.Length - 1; i >= 0; i--)
            Object.DestroyImmediate(objects[i].GameObject());
    }
}