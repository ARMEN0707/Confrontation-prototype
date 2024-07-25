using System;
using Core;
using UnityEngine;

namespace Data
{
    [Serializable]
    public abstract class BuildingData : ObjectData
    {
        public int Level = 1;
        public int TeamID;
        public Vector3 Position;
    }
}