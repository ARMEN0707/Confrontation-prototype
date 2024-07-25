using UnityEngine;

namespace Interfaces
{
    public interface IUnitController : IBuilding
    {
        public void AddSpeedBonus(float bonus);
        public void AddForceBonus(float bonus);
        public void AddReproductionBonus(float bonus);
        public void AddProtectionBonus(float bonus);
        public void AddDebuffProtectionBonus(float bonus);
        public int GetArmyCount();
        public float GetProtectionBonus();
        public float GetDebuffProtectionBonus();
        public float GetSpeed();
        public float GetForce();
        public void UpdateArmyCount(int unitCount);
        public void SetActiveLine(bool check);
        public void SetLineEndPos(Vector3 pos);
    }
}