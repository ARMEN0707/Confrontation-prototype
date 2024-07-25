using UnityEngine;

namespace Interfaces
{
    public interface ISettlement : IBuilding
    {
        public void OnCrash(Collider2D other);
    }
}