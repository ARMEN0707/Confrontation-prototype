using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using FuryLion.UI;
using Interfaces;
using UnityEngine;
using Views;

namespace Entities
{
    public class SettlementEntity : BuildingEntity<SettlementData>, IUnitController, ISettlement, IUpdatable, IBoostable
    {
        private readonly SettlementView _settlementView;
        
        private int _maxMilitaryCount = 20;
        private float _baseMilitaryReproduction;
        private float _baseSpeed;
        private float _baseForce;
        private float _baseProtectionBonus = 0;
        private float _baseDebuffProtectionBonus = 0;
        
        private float _militaryReproduction;
        private float _speed;
        private float _force;
        private float _protectionBonus;
        private float _debuffProtectionBonus;

        private float _timeScale;
        private float _passTime;

        private float _boost = 1;

        public float Boost
        {
            get => _boost;
            set
            {
                _timeScale *= _boost / value;
                _speed /= _boost * value;
                _boost = value;
            }
        }
        
        public SettlementEntity(SettlementData data) : base(data)
        {
            OnChangedTeamID(TeamID);
            ChangedTeamID += (b, teamID, newTeamID) => OnChangedTeamID(newTeamID);
            
            _settlementView = Recycler.Get<SettlementView>();
            _settlementView.transform.position = data.Position;
            _settlementView.BuildingEntity = this;
            _settlementView.SetArmyCount(data.ArmyCount);
            _settlementView.SetLevel(data.Level);
            _settlementView.SetMilitaryCount(data.MilitaryCount);
            _settlementView.SetLineRendererSettings();
        }
        
        private void OnChangedTeamID(int newTeamID)
        {
            _militaryReproduction -= _baseMilitaryReproduction;
            _force -= _baseForce;
            _speed -= _baseSpeed;
            
            _baseMilitaryReproduction = newTeamID == 1 ? 
                LevelManager.PlayerData.BaseMilitaryReproduction : 
                AIAcademy.BaseMilitaryReproduction;
            _baseForce = newTeamID == 1 ? LevelManager.PlayerData.BaseForce : AIAcademy.BaseForce;
            _baseSpeed = newTeamID == 1 ? LevelManager.PlayerData.BaseSpeed : AIAcademy.BaseSpeed;

            _militaryReproduction += _baseMilitaryReproduction;
            _force += _baseForce;
            _speed += _baseSpeed;
            _protectionBonus = _baseProtectionBonus;
            _debuffProtectionBonus = _baseDebuffProtectionBonus;

            _timeScale = _militaryReproduction / Data.Level;
        }
        
        public override void Dispose()
        {
            base.Dispose();
            _militaryReproduction = _baseMilitaryReproduction;
            _speed = _baseSpeed * Boost;
            _force = _baseForce;
            _protectionBonus = _baseProtectionBonus;
            _debuffProtectionBonus = _baseDebuffProtectionBonus;
        }
        
        public void OnUpdate(float deltaTime)
        {
            _passTime += deltaTime;
            if (_passTime >= _timeScale)
            {
                UpdateMilitaryCount(Data.MilitaryCount + 1);
                _passTime = 0;
            }
        }

        public void AddReproductionBonus(float bonus)
        {
            _militaryReproduction -= bonus;
            _timeScale = _militaryReproduction / Data.Level;
        }

        public void SetActiveLine(bool check)
        {
            _settlementView.SetActiveLine(check);
        }

        public void SetLineEndPos(Vector3 pos)
        {
            _settlementView.SetLineEndPos(pos);
        }

        public void AddSpeedBonus(float bonus)
        {
            _speed += bonus;
        }

        public void AddForceBonus(float bonus)
        {
            _force += bonus;
        }

        public void AddProtectionBonus(float bonus)
        {
            _protectionBonus += bonus;
        }

        public void AddDebuffProtectionBonus(float bonus)
        {
            _debuffProtectionBonus += bonus;
        }

        public int GetArmyCount() => Data.ArmyCount;
        
        public float GetSpeed() => _speed;

        public float GetForce() => _force;

        public float GetProtectionBonus() => _protectionBonus;

        public float GetDebuffProtectionBonus() => _debuffProtectionBonus;

        public void OnCrash(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<UnitView>(out var unit))
            {
                var unitEntity = unit.UnitEntity;
                if (unitEntity.TargetPosition != Data.Position)
                    return;

                if (unitEntity.TeamID == TeamID)
                    UpdateArmyCount(Data.ArmyCount + 1);
                else
                    StartBattle(unitEntity);
                
                unitEntity.Crash();
            }
        }

        private void StartBattle(UnitEntity unit)
        {
            var protection = _protectionBonus - unit.DebuffProtection;
            var armyForce = (_force * Data.ArmyCount) + (protection * Data.ArmyCount);
            var militaryForce = (_force * Data.MilitaryCount) + (protection * Data.MilitaryCount);

            armyForce -= militaryForce <= 0 && armyForce > 0 ? unit.Force : unit.Force / 2;
            militaryForce -= armyForce <= 0 && militaryForce > 0 ? unit.Force : unit.Force / 2;

            if (militaryForce > 0 && armyForce <= 0)
                TeamID = 0;

            if (militaryForce <= 0 && armyForce <= 0)
                TeamID = unit.TeamID;

            UpdateArmyCount((int)((Mathf.Abs(armyForce) / _force) - (Mathf.Abs(armyForce) / protection)));
            UpdateMilitaryCount((int)((Mathf.Abs(militaryForce) / _force) - (Mathf.Abs(militaryForce) / protection)));
        }

        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _settlementView.SetLevel(lvl);
        }
        
        public void UpdateArmyCount(int unitCount)
        {
            Data.ArmyCount = unitCount;
            _settlementView.SetArmyCount(unitCount);
        }

        private void UpdateMilitaryCount(int militaryCount)
        {
            if (militaryCount > _maxMilitaryCount * Data.Level)
                return;
            
            Data.MilitaryCount = militaryCount;
            _settlementView.SetMilitaryCount(militaryCount);
        }
    }
}
