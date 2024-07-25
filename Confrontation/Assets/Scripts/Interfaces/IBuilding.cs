using System;
using Core;
using Data;
using UnityEngine;

namespace Interfaces
{
    public interface IBuilding : IActor
    {
        public int TeamID { get; set; }
        public int Level { get; set; }
        public Vector3 Position { get; set; }
        public event Action<IBuilding, int> ChangedLevel;
        public event Action<IBuilding, int, int> ChangedTeamID;
        public void Dispose();
    }
}