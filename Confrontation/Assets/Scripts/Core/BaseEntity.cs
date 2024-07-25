namespace Core
{
    public abstract class BaseEntity
    {
        public virtual void Dispose() { }
    }

    public abstract class BaseEntity<TData> : BaseEntity where TData : ObjectData
    {
        protected readonly TData Data;

        protected BaseEntity(TData data)
        {
            Data = data;
        }
    }
}