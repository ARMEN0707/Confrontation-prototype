using System.Collections.Generic;
using Core;
using Interfaces;

namespace Systems
{
    public class BoostSystem : BaseSystem<IBoostable>
    {
        private List<IBoostable> _boostables = new List<IBoostable>();
        private float _boost = 1;

        protected override void AddActor(IBoostable warehouse)
        {
            warehouse.Boost = _boost;
            _boostables.Add(warehouse);
        }

        protected override void RemoveActor(IBoostable actor)
        {
            _boostables.Remove(actor);
        }

        public float OnBoost(float boost)
        {
            if (_boost + boost <= 0.1 || _boost + boost >= 2.1)
                return _boost;
            
            _boost += boost;
            foreach (var b in _boostables)
                b.Boost = _boost;

            return _boost;
        }
    }
}