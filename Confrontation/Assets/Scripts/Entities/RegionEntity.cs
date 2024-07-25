using System.Collections.Generic;
using System.Linq;
using Core;
using Data;
using Interfaces;

namespace Entities
{
    public class RegionEntity : BaseEntity<RegionData>, IRegion
    {
        private List<CellEntity> _cellEntities = new List<CellEntity>();
        
        public RegionEntity(RegionData data) : base(data)
        {
        }

        public void AddCell(CellEntity cell)
        {
            _cellEntities.Add(cell);
        }

        public List<ICell> GetCells()
        {
            var cells = new List<ICell>();
            foreach (var c in _cellEntities)
                if (c is ICell cell)
                    cells.Add(cell);

            return cells;
        }

        public bool ContainsBuilding(IBuilding building)
        {
            return _cellEntities.Any(c => c.Building == building);
        }
        
        public int? GetTeamID()
        {
            return _cellEntities.FirstOrDefault()?.TeamID;
        }

        public void ChangeTeamID(int newTeamID)
        {
            foreach (var c in _cellEntities)
            {
                c.ChangeTeamID(newTeamID);
            }
        }
    }
}