using System;
using Core;
using Entities;
using Interfaces;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class CellData : ObjectData
    {
        public int TeamID;

        public Vector3 Position;
    
        public BuildingData Building;
    
        protected override BaseEntity CreateEntity(IWorld world)
        {
            var cell = new CellEntity(this);
            if (Building != null)
            {
                Building.TeamID = TeamID;
                cell.SetBuilding(world.CreateNewObject(Building) as IBuilding);
            }

            return cell;
        }
    }
}