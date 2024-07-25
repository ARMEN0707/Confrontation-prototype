using System;
using Core;
using Entities;

namespace Data
{
    [Serializable]
    public class BarracksData : BuildingData
    {
        public int ArmyCount = 10;

        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new BarracksEntity(this);
        }
    }
}