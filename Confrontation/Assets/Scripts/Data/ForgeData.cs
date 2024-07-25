using Core;
using Entities;

namespace Data
{
    public class ForgeData : BuildingData
    {
        public float ForceBonus = 1f;
    
        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new ForgeEntity(this);
        }
    }
}