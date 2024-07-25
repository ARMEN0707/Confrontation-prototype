using Core;
using Entities;

namespace Data
{
    public class StableData : BuildingData
    {
        public float SpeedBonus = 0.025f;
    
        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new StableEntity(this);
        }
    }
}