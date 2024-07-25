using System;
using System.Collections.Generic;
using Core;
using Entities;

namespace Data
{
    [Serializable]
    public class TeamData : ObjectData
    {
        public int TeamID;
        public int Money;
        public int Mana;
        public List<RegionData> RegionsData = new List<RegionData>();
        
        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new TeamEntity(this);
        }
    }
}