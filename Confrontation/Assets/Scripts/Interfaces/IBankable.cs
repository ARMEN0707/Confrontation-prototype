using System;

namespace Interfaces
{
    public interface IBankable : IBuilding
    {
        public event Action<IBankable> Updated;
        public int GetCurrency();
    }
}