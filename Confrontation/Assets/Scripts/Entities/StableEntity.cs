using Data;
using FuryLion.UI;
using Interfaces;
using UnityEngine;
using Views;

namespace Entities
{
    public class StableEntity : BuildingEntity<StableData>, IStable
    {
        private readonly StableView _stableView;
    
        public StableEntity(StableData data) : base(data)
        {
            _stableView = Recycler.Get<StableView>();
            _stableView.transform.position = data.Position;
            _stableView.BuildingEntity = this;
            _stableView.SetLevel(data.Level);
        }

        public float GetSpeedBonus()
        {
            return Data.SpeedBonus * Data.Level;
        }

        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _stableView.SetLevel(lvl);
        }
    }
}
