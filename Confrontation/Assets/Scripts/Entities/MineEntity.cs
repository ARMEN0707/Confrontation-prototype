using System;
using Data;
using FuryLion.UI;
using Interfaces;
using Views;

namespace Entities
{
    public class MineEntity : BuildingEntity<MineData>, IBankable, IUpdatable, IBoostable
    {
        private readonly MineView _mineView;

        private float _timeScale = 1;
        private float _passTime;

        private float _boost = 1;
        
        public float Boost
        {
            get => _boost;
            set
            {
                _timeScale *= _boost / value;
                _boost = value;
            }
        }

        public event Action<IBankable> Updated;
    
        public MineEntity(MineData data) : base(data)
        {
            _mineView = Recycler.Get<MineView>();
            _mineView.transform.position = data.Position;
            _mineView.BuildingEntity = this;
            _mineView.SetLevel(data.Level);
        }

        public int GetCurrency()
        {
            return Data.CoinEarningBonus * Data.Level;
        }

        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _mineView.SetLevel(lvl);
        }
        
        public void OnUpdate(float deltaTime)
        {
            _passTime += deltaTime;
            if (_passTime >= _timeScale)
            {
                Updated?.Invoke(this);
                _passTime = 0;
            }
        }
    }
}
