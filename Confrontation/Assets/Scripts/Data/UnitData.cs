using Core;
using Entities;
using UnityEngine;

namespace Data
{
    public class UnitData : ObjectData
    {
        public int TeamID;
        public float Force;
        public float DebuffProtection;
        public float Speed;
        public Vector3 Position;
        public Vector3 TargetPosition;
        
        protected override BaseEntity CreateEntity(IWorld world)
        {
            return new UnitEntity(this, world);
        }
    }
}