using System;
using Core;
using Entities;

namespace Data
{
    [Serializable]
    public class FortData : BuildingData
    {
        public int ArmyCount = 0;

        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new FortEntity(this);
        }
    }
}
