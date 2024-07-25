using System;
using Core;
using Entities;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class CapitalData : SettlementData
    {
        public FarmData Farm;
        public MineData Mine;

        protected override BaseEntity CreateEntity(IWorld world)
        {
            Farm = new FarmData();
            Mine = new MineData();
            return new CapitalEntity(this);
        }
    }
}