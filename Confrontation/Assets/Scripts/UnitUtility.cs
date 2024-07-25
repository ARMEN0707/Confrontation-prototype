using System.Collections.Generic;
using Entities;

public static class UnitUtility
{
    private static List<UnitEntity> _units = new List<UnitEntity>();

    public static void AddUnit(UnitEntity unit)
    {
        _units.Add(unit);
    }

    public static void RemoveUnit(UnitEntity unit)
    {
        _units.Remove(unit);
    }

    public static void Clear()
    {
        _units.Clear();
    }

    public static void OnTeamKill(int teamID)
    {
        for (var i = _units.Count - 1; i >= 0; i--)
        {
            if (_units[i].TeamID == teamID)
                _units[i].Crash();
        }
    }

    public static void SetPause(bool isPaused)
    {
        foreach (var u in _units)
        {
            u.SetPause(isPaused);
        }
    }
}