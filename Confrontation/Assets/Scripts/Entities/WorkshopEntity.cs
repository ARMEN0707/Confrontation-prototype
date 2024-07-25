using Data;
using FuryLion.UI;
using Interfaces;
using Views;

namespace Entities
{
    public class WorkshopEntity : BuildingEntity<WorkhopData>, IWorkshop
    {
        private WorkshopView _workshopView;

        public WorkshopEntity(WorkhopData data) : base(data)
        {
            _workshopView = Recycler.Get<WorkshopView>();
            _workshopView.transform.position = data.Position;
            _workshopView.BuildingEntity = this;
            _workshopView.SetLevel(data.Level);
        }

        public float GetDebuffProtectionBonus()
        {
            return Data.ProtectionBonus * Data.Level;
        }

        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _workshopView.SetLevel(lvl);
        }
    }
}