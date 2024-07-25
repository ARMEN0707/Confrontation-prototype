using Core;
using Entities;

namespace Data
{
    public class MineData : BuildingData
    {
        public int CoinEarningBonus = 1;
        
        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new MineEntity(this);
        }
    }
}