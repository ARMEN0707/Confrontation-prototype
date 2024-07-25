using System;
using System.Collections.Generic;
using Core;
using Entities;

namespace Data
{
    [Serializable]
    public class RegionData : ObjectData
    {
        public List<CellData> Cells = new List<CellData>();

        protected override BaseEntity CreateEntity(IWorld world)
        {
            var region = new RegionEntity(this);
            foreach (var c in Cells)
            {
                region.AddCell(world.CreateNewObject(c) as CellEntity);
            }
        
            return region;
        }
    }
}