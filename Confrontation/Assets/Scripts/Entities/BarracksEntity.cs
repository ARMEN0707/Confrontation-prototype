using Data;
using FuryLion.UI;
using Interfaces;
using UnityEngine;
using Views;

namespace Entities
{
    public class BarracksEntity : BuildingEntity<BarracksData>, IUnitController, IBarracks, IUpdatable, IBoostable
    {
        private BarracksView _barracksView;
        
        private float _baseArmyReproduction;
        private float _baseSpeed;
        private float _baseForce;
        private float _baseProtectionBonus = 0;
        private float _baseDebuffProtectionBonus = 0;
        
        private float _armyReproduction;
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

        public BarracksEntity(BarracksData data) : base(data)
        {
            OnChangedTeamID(TeamID);
            ChangedTeamID += (b, teamID, newTeamID) => OnChangedTeamID(newTeamID);
            
            _barracksView = Recycler.Get<BarracksView>();
            _barracksView.transform.position = data.Position;
            _barracksView.BuildingEntity = this;
            _barracksView.SetLevel(data.Level);
            _barracksView.SetUnitCount(data.ArmyCount);
            _barracksView.SetLineRendererSettings();
        }

        private void OnChangedTeamID(int newTeamID)
        {
            _armyReproduction -= _baseArmyReproduction;
            _speed -= _baseSpeed;
            _force -= _baseForce;
            
            _baseArmyReproduction = newTeamID == 1 ? 
                LevelManager.PlayerData.BaseArmyReproduction : 
                AIAcademy.BaseArmyReproduction;
            _baseForce = newTeamID == 1 ? LevelManager.PlayerData.BaseForce : AIAcademy.BaseForce;
            _baseSpeed = newTeamID == 1 ? LevelManager.PlayerData.BaseSpeed : AIAcademy.BaseSpeed;
            
            _armyReproduction += _baseArmyReproduction;
            _speed += _baseSpeed;
            _force += _baseForce;
            _protectionBonus = _baseProtectionBonus;
            _debuffProtectionBonus = _baseDebuffProtectionBonus;
            
            _timeScale = _armyReproduction / Data.Level;
        }
        
        public void OnUpdate(float deltaTime)
        {
            _passTime += deltaTime;
            if (_passTime >= _timeScale)
            {
                if (TeamID != 0)
                    UpdateArmyCount(Data.ArmyCount + 1);
                
                _passTime = 0;
            }
        }
        
        protected override void OnChangeLevel(int lvl)
        {
            base.OnChangeLevel(lvl);
            _barracksView.SetLevel(lvl);
        }

        public override void Dispose()
        {
            base.Dispose();
            _armyReproduction = _baseArmyReproduction;
            _speed = _baseSpeed * Boost;
            _force = _baseForce;
            _protectionBonus = _baseProtectionBonus;
            _debuffProtectionBonus = _baseDebuffProtectionBonus;
        }

        public void AddReproductionBonus(float bonus)
        {
            _armyReproduction -= bonus;
            _timeScale = _armyReproduction / Data.Level;
        }

        public void SetActiveLine(bool check) => _barracksView.SetActiveLine(check);
        
        public void SetLineEndPos(Vector3 pos) => _barracksView.SetLineEndPos(pos);

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

        public void UpdateArmyCount(int unitCount)
        {
            Data.ArmyCount = unitCount;
            _barracksView.SetUnitCount(unitCount);
        }
    }
}
