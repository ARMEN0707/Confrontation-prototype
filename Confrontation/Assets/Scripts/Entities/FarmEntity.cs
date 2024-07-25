using Data;
using FuryLion.UI;
using Interfaces;
using Views;

namespace Entities
{
    public class FarmEntity : BuildingEntity<FarmData>, IFarm
    {
        private readonly FarmView _farmView;
    
        public FarmEntity(FarmData data) : base(data)
        {
            _farmView = Recycler.Get<FarmView>();
            _farmView.transform.position = data.Position;
            _farmView.BuildingEntity = this;
            _farmView.SetLevel(data.Level);
        }

        public float GetReproductionBonus()
        {
            return Data.ReproductionBonus * Data.Level;
        }

        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _farmView.SetLevel(lvl);
        }
    }
}
