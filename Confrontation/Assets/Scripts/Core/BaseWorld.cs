using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface IWorld
    {
        Transform Parent { get; }
        BaseEntity CreateNewObject(ObjectData data);
        BaseSystem GetSystem(Type type);
        void RemoveEntity(BaseEntity entity);
        void Deactivate();
    }

    public abstract class BaseWorld<TData> : IWorld where TData : WorldData
    {
        private readonly Dictionary<Type, BaseSystem> _systems = new Dictionary<Type, BaseSystem>();
        private Dictionary<BaseEntity, ObjectData> _entities;

        protected TData Data;
    
        public Transform Parent { private set; get; }
    
        public virtual void Init(TData worldData, Transform parent, params BaseSystem[] systems)
        {
            Data = worldData;

            _entities = new Dictionary<BaseEntity, ObjectData>();

            foreach (var system in systems)
            {
                if (!_systems.ContainsKey(system.GetType()))
                    _systems.Add(system.GetType(), system);
            }

            Parent = parent;

            var count = worldData.Objects.Count;
            for (var i = 0; i < count; i++)
                CreateObject(worldData.Objects[i]);
        }
    
        public void AddSystem(BaseSystem system)
        {
            if (_systems.ContainsKey(system.GetType()))
                return;
        
            _systems.Add(system.GetType(), system);

            foreach (var entity in _entities)
                system.AddEntity(entity.Key);
        }

        public BaseSystem GetSystem(Type type)
        {
            return _systems.ContainsKey(type) ? _systems[type] : null;
        }
    
        public T GetSystem<T>() where T : BaseSystem
        {
            return (_systems.ContainsKey(typeof(T)) ? _systems[typeof(T)] : null) as T;
        }
    
        public BaseEntity CreateNewObject(ObjectData data)
        {
            var entity = CreateObject(data);
            Data.Objects.Add(data);

            return entity;
        }

        private BaseEntity CreateObject(ObjectData data)
        {
            var entity = data.GetEntity(this);
            _entities.Add(entity, data);

            foreach (var system in _systems)
                system.Value.AddEntity(entity);

            return entity;
        }

        public void RemoveEntity(BaseEntity entity)
        {
            foreach (var system in _systems)
                system.Value.RemoveEntity(entity);

            Data.Objects.Remove(_entities[entity]);
            _entities.Remove(entity);
        
            entity.Dispose();
        }
    
        public void RemoveSystem(BaseSystem system)
        {
            if (_systems.ContainsKey(system.GetType()))
                _systems.Remove(system.GetType());

            foreach (var entity in _entities)
                system.RemoveEntity(entity.Key);
        }

        public virtual void Deactivate()
        {
            var baseSystems = new List<BaseSystem>();
            foreach (var system in _systems)
                baseSystems.Add(system.Value);

            foreach (var type in baseSystems)
                RemoveSystem(type);
        }
    }
}