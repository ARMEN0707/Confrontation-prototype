using Data;
using FuryLion.UI;
using Interfaces;
using Views;

namespace Entities
{
    public class QuarryEntity : BuildingEntity<QuarryData>, IQuarry
    {
        private QuarryView _quarryView;

        public QuarryEntity(QuarryData data) : base(data)
        {
            _quarryView = Recycler.Get<QuarryView>();
            _quarryView.transform.position = data.Position;
            _quarryView.BuildingEntity = this;
            _quarryView.SetLevel(data.Level);
        }

        public float GetProtectionBonus()
        {
            return Data.ProtectionBonus * Data.Level;
        }

        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _quarryView.SetLevel(lvl);
        }
    }
}
