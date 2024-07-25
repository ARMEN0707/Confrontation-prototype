using Data;
using FuryLion.UI;
using Interfaces;
using UnityEngine;
using Views;

namespace Entities
{
    public class FortEntity : BuildingEntity<FortData>, IUnitController, IFort, IBoostable
    {
        private FortView _fortView;

        private float _baseSpeedBonus = 1;
        private float _baseForceBonus = 1;
        private float _baseDebuffProtectionBonus = 0;

        private float _speedBonus;
        private float _forceBonus;
        private float _debuffProtectionBonus;

        private float _boost;

        public float Boost
        {
            get => _boost;
            set
            {
                _boost = value;
                _speedBonus = _baseSpeedBonus * _boost;
            }
        }

        public FortEntity(FortData data) : base(data)
        {
            _fortView = Recycler.Get<FortView>();
            _fortView.transform.position = data.Position;
            _fortView.BuildingEntity = this;
            _fortView.SetLevel(data.Level);
            _fortView.SetArmyCount(data.ArmyCount);
            _fortView.SetLineRendererSettings();

            _speedBonus = _baseSpeedBonus * Boost;
            _forceBonus = _baseForceBonus;
            _debuffProtectionBonus = _baseDebuffProtectionBonus;
        }

        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _fortView.SetLevel(lvl);
        }

        public override void Dispose()
        {
            base.Dispose();
            _speedBonus = _baseSpeedBonus * Boost;
            _forceBonus = _baseForceBonus;
            _debuffProtectionBonus = _baseDebuffProtectionBonus;
        }

        public void SetActiveLine(bool check) => _fortView.SetActiveLine(check);

        public void SetLineEndPos(Vector3 pos) => _fortView.SetLineEndPos(pos);

        public void AddReproductionBonus(float bonus) { }

        public void AddProtectionBonus(float bonus) { }

        public void AddSpeedBonus(float bonus)
        {
            _speedBonus += bonus;
        }

        public void AddForceBonus(float bonus)
        {
            _forceBonus += bonus;
        }

        public void AddDebuffProtectionBonus(float bonus)
        {
            _debuffProtectionBonus += bonus;
        }

        public int GetArmyCount() => Data.ArmyCount;

        public float GetSpeed() => _speedBonus;

        public float GetForce() => _forceBonus;

        public float GetProtectionBonus() => 0;

        public float GetDebuffProtectionBonus() => _debuffProtectionBonus;

        public void UpdateArmyCount(int unitCount)
        {
            Data.ArmyCount = unitCount;
            _fortView.SetArmyCount(unitCount);
        }

        public void OnCrash(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<UnitView>(out var unit))
            {
                var unitEntity = unit.UnitEntity;
                if (unitEntity.TargetPosition != Data.Position)
                    return;

                if (unitEntity.TeamID == TeamID)
                    UpdateArmyCount(Data.ArmyCount + 1);

                unitEntity.Crash();
            }
        }
    }
}
