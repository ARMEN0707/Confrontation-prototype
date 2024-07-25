using System;
using Core;
using Entities;

namespace Data
{
    [Serializable]
    public class SettlementData : BuildingData
    {
        public int MilitaryCount = 10;
        public int ArmyCount = 10;

        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new SettlementEntity(this);
        }
    }
}