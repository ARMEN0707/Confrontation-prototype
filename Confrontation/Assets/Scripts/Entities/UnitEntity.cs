using System;
using Core;
using Data;
using DG.Tweening;
using FuryLion.UI;
using Interfaces;
using UnityEngine;
using Views;
using Random = UnityEngine.Random;

namespace Entities
{
    public class UnitEntity : BaseEntity<UnitData>
    {
        private UnitView _unitView;
        private Tween _tween;

        public int TeamID => Data.TeamID;

        public Vector3 TargetPosition => Data.TargetPosition;

        public float Force => Data.Force;

        public float DebuffProtection => Data.DebuffProtection;

        public event Action<UnitEntity> Crashed;

        public UnitEntity(UnitData data, IWorld world) : base(data)
        {
            _unitView = Recycler.Get<UnitView>();
            _unitView.gameObject.SetActive(false);
            _unitView.UnitEntity = this;
            UnitUtility.AddUnit(this);
            Crashed += world.RemoveEntity;
            if (data.TargetPosition != data.Position) 
                Move();
        }

        public void Init(int teamID, Vector3 startPos, Vector3 targetPosition, float speed, float force, float debuffProtection)
        {
            Data.TeamID = teamID;
            Data.Position = startPos;
            Data.TargetPosition = targetPosition;
            Data.Speed = speed;
            Data.Force = force;
            Data.DebuffProtection = debuffProtection;
        }

        public void Move()
        {
            _unitView.gameObject.SetActive(true);
            _unitView.Position = (Vector3) Random.insideUnitCircle * 0.1f + Data.Position;
            var targetPos = (Vector3) Random.insideUnitCircle * 0.1f + Data.TargetPosition;
            var duration = (targetPos - _unitView.Position).magnitude / Data.Speed;
            _unitView.LookAt(targetPos);
            _unitView.SetAnimations(TeamID - 1);
            _unitView.PlayMoveAnim();
            _tween = _unitView.transform.DOMove(targetPos, duration);
        }

        public void SetPause(bool isPaused)
        {
            if (isPaused)
            {
                _tween?.Pause();
                Data.Position = _unitView.Position;
            }
            else
                _tween?.Play();
        }

        public void Crash()
        {
            if (Crashed == null)
                return;

            _tween?.Kill();
            Recycler.Release(_unitView);
            UnitUtility.RemoveUnit(this);
            Crashed.Invoke(this);
            Crashed = null;
        }
    }
}
