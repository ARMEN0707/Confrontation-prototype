using Core;
using Entities;

namespace Data
{
    public class FarmData : BuildingData
    {
        public float ReproductionBonus = 0.05f;

        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new FarmEntity(this);
        }
    }
}