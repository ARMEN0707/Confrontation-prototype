using Core;
using Entities;

namespace Data
{
    public class WizardTowerData : BuildingData
    {
        public int ManaEarning = 1;
    
        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new WizardTowerEntity(this);
        }
    }
}