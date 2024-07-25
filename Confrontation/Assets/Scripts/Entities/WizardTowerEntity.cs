using System;
using Core;
using Data;
using FuryLion.UI;
using Interfaces;
using UnityEngine;
using Views;

namespace Entities
{
    public class WizardTowerEntity : BuildingEntity<WizardTowerData>, IBankable, IUpdatable, IBoostable
    {
        private WizardTowerView _wizardTower;
        
        private float _timeScale = 1;
        private float _passTime;

        private float _boost;

        public event Action<IBankable> Updated;
        
        public float Boost
        {
            get => _boost;
            set
            {
                _timeScale *= _boost / value;
                _boost = value;
            }
        }
        
        public WizardTowerEntity(WizardTowerData data) : base(data)
        {
            _wizardTower = Recycler.Get<WizardTowerView>();
            _wizardTower.transform.position = data.Position;
            _wizardTower.BuildingEntity = this;
            _wizardTower.SetLevel(data.Level);
        }

        public int GetCurrency()
        {
            return Data.ManaEarning * Data.Level;
        }
        
        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _wizardTower.SetLevel(lvl);
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