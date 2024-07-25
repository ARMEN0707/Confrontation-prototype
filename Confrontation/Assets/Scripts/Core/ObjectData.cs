using System;

namespace Core
{
    [Serializable]
    public abstract class ObjectData
    {
        internal BaseEntity GetEntity(IWorld world)
        {
            return CreateEntity(world);
        }

        protected abstract BaseEntity CreateEntity(IWorld world);
    }
}
