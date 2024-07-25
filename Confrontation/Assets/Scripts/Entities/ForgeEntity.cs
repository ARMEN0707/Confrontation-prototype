using Data;
using FuryLion.UI;
using Interfaces;
using Views;

namespace Entities
{
    public class ForgeEntity : BuildingEntity<ForgeData>, IForge
    {
        private readonly ForgeView _forgeView;
    
        public ForgeEntity(ForgeData data) : base(data)
        {
            _forgeView = Recycler.Get<ForgeView>();
            _forgeView.transform.position = data.Position;
            _forgeView.BuildingEntity = this;
            _forgeView.SetLevel(data.Level);
        }

        public float GetForceBonus()
        {
            return Data.ForceBonus * Data.Level;
        }

        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _forgeView.SetLevel(lvl);
        }
    }
}
