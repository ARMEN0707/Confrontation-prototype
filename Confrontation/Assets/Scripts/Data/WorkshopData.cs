using Core;
using Entities;

namespace Data
{
    public class WorkhopData : BuildingData
    {
        public float ProtectionBonus = 1;

        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new WorkshopEntity(this);
        }
    }
}
