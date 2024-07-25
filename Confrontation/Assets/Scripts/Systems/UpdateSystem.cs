using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Cysharp.Threading.Tasks;
using Entities;
using Interfaces;
using UnityEngine;

namespace Systems
{
    public class UpdateSystem : BaseSystem<IUpdatable>
    {
        private readonly List<IUpdatable> _updateObjs = new List<IUpdatable>();
        private int _frameCount = 1;
        private float _passTime;

        private bool _isUpdating = true;

        public void AddUpdatableObject(IUpdatable obj)
        {
            _updateObjs.Add(obj);
        }
        
        protected override void AddActor(IUpdatable warehouse)
        {
            _updateObjs.Add(warehouse);
        }

        protected override void RemoveActor(IUpdatable actor)
        {
            _updateObjs.Remove(actor);
        }

        public async Task SetPause(bool isPaused)
        {
            _isUpdating = !isPaused;
            if (_isUpdating)
                await Update();
        }

        public async Task Update()
        {
            while (_isUpdating)
            {
                await UniTask.DelayFrame(_frameCount);
                var delta = Time.realtimeSinceStartup - _passTime;
                _passTime = Time.realtimeSinceStartup;
                var count = _updateObjs.Count;
                for (var i = 0; i < count; i++)
                    _updateObjs[i].OnUpdate(delta);
            }
        }
    }
}