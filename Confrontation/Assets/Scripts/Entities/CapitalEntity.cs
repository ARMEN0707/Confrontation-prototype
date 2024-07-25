using System;
using Data;
using FuryLion.UI;
using Interfaces;
using UnityEngine;
using Views;

namespace Entities
{
    public class CapitalEntity : BuildingEntity<CapitalData>, IFarm, IUnitController, ICapital, IUpdatable, 
        IBankable, IBoostable
    {
        private CapitalView _capitalView;

        private int _maxMilitaryCount = 20;
        private float _baseMilitaryReproduction;
        private float _baseArmyReproduction;
        private float _baseSpeed;
        private float _baseForce;
        private float _baseProtectionBonus = 0;
        private float _baseDebuffProtectionBonus = 0;
        
        private float _militaryReproduction;
        private float _armyReproduction;
        private float _speed;
        private float _force;
        private float _protectionBonus;
        private float _debuffProtectionBonus;

        private float _baseMilitaryTimeScale;
        private float _militaryTimeScale;
        private float _passMilitaryTime;
        
        private float _baseArmyTimeScale;
        private float _armyTimeScale;
        private float _passArmyTime;

        private float _moneyTimeScale = 1;
        private float _passMoneyTime;

        private float _boost = 1;
        
        public event Action<IBankable> Updated;

        public float Boost
        {
            get => _boost;
            set
            {
                _militaryTimeScale *= _boost / value;
                _armyTimeScale *= _boost / value;
                _moneyTimeScale *= _boost / value;
                _speed /= _boost * value;
                _boost = value;
            }
        }
    
        public CapitalEntity(CapitalData data) : base(data)
        {
            OnChangedTeamID(TeamID);
            ChangedTeamID += (b, teamID, newTeamID) => OnChangedTeamID(newTeamID);
            
            _capitalView = Recycler.Get<CapitalView>();
            _capitalView.transform.position = data.Position;
            _capitalView.BuildingEntity = this;
            _capitalView.SetLevel(data.Level);
            _capitalView.SetLineRendererSettings();
            _capitalView.SetArmyCount(data.ArmyCount);
            _capitalView.SetMilitaryCount(data.MilitaryCount);
        }
        
        private void OnChangedTeamID(int newTeamID)
        {
            _militaryReproduction -= _baseMilitaryReproduction;
            _armyReproduction -= _baseArmyReproduction;
            _speed -= _baseSpeed;
            _force -= _baseForce;
            
            _baseMilitaryReproduction = newTeamID == 1 ? 
                LevelManager.PlayerData.BaseMilitaryReproduction : 
                AIAcademy.BaseMilitaryReproduction;
            _baseArmyReproduction = newTeamID == 1 ? 
                LevelManager.PlayerData.BaseArmyReproduction : 
                AIAcademy.BaseArmyReproduction;
            _baseForce = newTeamID == 1 ? LevelManager.PlayerData.BaseForce : AIAcademy.BaseForce;
            _baseSpeed = newTeamID == 1 ? LevelManager.PlayerData.BaseSpeed : AIAcademy.BaseSpeed;
            
            _militaryReproduction += _baseMilitaryReproduction;
            _armyReproduction += _baseArmyReproduction;
            _speed += _baseSpeed;
            _force += _baseForce;
            _protectionBonus = _baseProtectionBonus;
            _debuffProtectionBonus = _baseDebuffProtectionBonus;
            
            _militaryTimeScale = _militaryReproduction / Data.Level;
            _armyTimeScale = _armyReproduction / Data.Level;
        }
        
        public override void Dispose()
        {
            base.Dispose();
            _militaryReproduction = _baseMilitaryReproduction;
            _armyReproduction = _baseArmyReproduction;
            _speed = _baseSpeed * Boost;
            _force = _baseForce;
            _protectionBonus = _baseProtectionBonus;
            _debuffProtectionBonus = _baseDebuffProtectionBonus;
        }

        public void OnUpdate(float deltaTime)
        {
            _passMilitaryTime += deltaTime;
            if (_passMilitaryTime >= _militaryTimeScale)
            {
                UpdateMilitaryCount(Data.MilitaryCount + 1);
                _passMilitaryTime = 0;
            }

            _passArmyTime += deltaTime;
            if (_passArmyTime >= _armyTimeScale)
            {
                if (TeamID != 0)
                    UpdateArmyCount(Data.ArmyCount + 1);
                
                _passArmyTime = 0;
            }
            
            _passMoneyTime += deltaTime;
            if (_passMoneyTime >= _moneyTimeScale)
            {
                Updated?.Invoke(this);
                _passMoneyTime = 0;
            }
        }

        public void AddReproductionBonus(float bonus)
        {
            _militaryReproduction -= bonus;
            _militaryTimeScale = _militaryReproduction / Data.Level;
            
            _armyReproduction -= bonus;
            _armyTimeScale = _armyReproduction / Data.Level;
        }

        public void SetActiveLine(bool check)
        {
            _capitalView.SetActiveLine(check);
        }

        public void SetLineEndPos(Vector3 pos)
        {
            _capitalView.SetLineEndPos(pos);
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

        public float GetProtectionBonus() => _protectionBonus;

        public float GetDebuffProtectionBonus() => _debuffProtectionBonus;

        public float GetForce() => _force;
        
        public float GetReproductionBonus()
        {
            return Data.Farm.ReproductionBonus * Data.Level;
        }

        public int GetCurrency()
        {
            return Data.Mine.CoinEarningBonus * Data.Level;
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

            UpdateArmyCount((int) ((Mathf.Abs(armyForce) / _force) - (Mathf.Abs(armyForce) / protection)));
            UpdateMilitaryCount((int)((Mathf.Abs(militaryForce) / _force) - (Mathf.Abs(militaryForce) / protection)));
        }

        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _capitalView.SetLevel(lvl);
        }
        
        public void UpdateArmyCount(int unitCount)
        {
            Data.ArmyCount = unitCount;
            _capitalView.SetArmyCount(unitCount);
        }

        private void UpdateMilitaryCount(int militaryCount)
        {
            if (militaryCount > _maxMilitaryCount * Data.Level)
                return;

            Data.MilitaryCount = militaryCount;
            _capitalView.SetMilitaryCount(militaryCount);
        }
    }
}
