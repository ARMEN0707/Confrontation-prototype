namespace Core
{
    public abstract class BaseSystem
    {
        internal void AddEntity(BaseEntity entity)
        {
            if (entity is IActor actor)
                AddEntity(actor);
        }

        internal void RemoveEntity(BaseEntity entity)
        {
            if (entity is IActor actor)
                RemoveEntity(actor);
        }

        protected abstract void AddEntity(IActor actor);
        protected abstract void RemoveEntity(IActor actor);
    }

    public abstract class BaseSystem<T> : BaseSystem
    {
        protected override void AddEntity(IActor entity)
        {
            if (entity is T actor)
                AddActor(actor);
        }

        protected override void RemoveEntity(IActor entity)
        {
            if (entity is T actor)
                RemoveActor(actor);
        }

        protected virtual void AddActor(T warehouse)
        {
        }

        protected virtual void RemoveActor(T actor)
        {
        }
    }

    public abstract class BaseSystem<T, T1> : BaseSystem<T>
        where T : IActor
        where T1 : IActor
    {
        protected override void AddEntity(IActor entity)
        {
            base.AddEntity(entity);

            if (entity is T1 actor)
                AddActor(actor);
        }

        protected override void RemoveEntity(IActor entity)
        {
            base.RemoveEntity(entity);

            if (entity is T1 actor)
                RemoveActor(actor);
        }

        protected virtual void AddActor(T1 resource)
        {
        }

        protected virtual void RemoveActor(T1 resource)
        {
        }
    }

    public abstract class BaseSystem<T, T1, T2> : BaseSystem<T, T1>
        where T : IActor
        where T1 : IActor
        where T2: IActor
    {
        protected override void AddEntity(IActor entity)
        {
            base.AddEntity(entity);

            if (entity is T2 actor)
                AddActor(actor);
        }

        protected override void RemoveEntity(IActor entity)
        {
            base.RemoveEntity(entity);

            if (entity is T2 actor)
                RemoveActor(actor);
        }

        protected virtual void AddActor(T2 actor)
        {
        }

        protected virtual void RemoveActor(T2 actor)
        {
        }
    }
    
    public abstract class BaseSystem<T, T1, T2, T3> : BaseSystem<T, T1, T2>
        where T : IActor
        where T1 : IActor
        where T2: IActor
        where T3 : IActor
    {
        protected override void AddEntity(IActor entity)
        {
            base.AddEntity(entity);

            if (entity is T3 actor)
                AddActor(actor);
        }

        protected override void RemoveEntity(IActor entity)
        {
            base.RemoveEntity(entity);

            if (entity is T3 actor)
                RemoveActor(actor);
        }

        protected virtual void AddActor(T3 actor)
        {
        }

        protected virtual void RemoveActor(T3 actor)
        {
        }
    }

    public abstract class BaseSystem<T, T1, T2, T3, T4> : BaseSystem<T, T1, T2, T3>
    where T : IActor
    where T1 : IActor
    where T2 : IActor
    where T3 : IActor
    where T4 : IActor
    {
        protected override void AddEntity(IActor entity)
        {
            base.AddEntity(entity);

            if (entity is T4 actor)
                AddActor(actor);
        }

        protected override void RemoveEntity(IActor entity)
        {
            base.RemoveEntity(entity);

            if (entity is T4 actor)
                RemoveActor(actor);
        }

        protected virtual void AddActor(T4 actor)
        {
        }

        protected virtual void RemoveActor(T4 actor)
        {
        }
    }

    public abstract class BaseSystem<T, T1, T2, T3, T4, T5> : BaseSystem<T, T1, T2, T3, T4>
    where T : IActor
    where T1 : IActor
    where T2 : IActor
    where T3 : IActor
    where T4 : IActor
    where T5 : IActor
    {
        protected override void AddEntity(IActor entity)
        {
            base.AddEntity(entity);

            if (entity is T5 actor)
                AddActor(actor);
        }

        protected override void RemoveEntity(IActor entity)
        {
            base.RemoveEntity(entity);

            if (entity is T5 actor)
                RemoveActor(actor);
        }

        protected virtual void AddActor(T5 actor)
        {
        }

        protected virtual void RemoveActor(T5 actor)
        {
        }
    }
}