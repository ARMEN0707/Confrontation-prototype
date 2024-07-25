using System.Collections.Generic;
using System.Linq;
using Core;
using Interfaces;

namespace Systems
{
    public class WarFogSystem : BaseSystem<IRegion, IBuilding>
    {
        private List<IRegion> _regions = new List<IRegion>();
        
        protected override void AddActor(IBuilding actor)
        {
            actor.ChangedTeamID += OnChangedTeamID;
        }

        protected override void AddActor(IRegion warehouse)
        {
            _regions.Add(warehouse);
        }

        protected override void RemoveActor(IRegion actor)
        {
            _regions.Remove(actor);
        }

        private void OnChangedTeamID(IBuilding building, int teamID, int newTeamID)
        {
            UpdateAvailable(teamID);
            UpdateAvailable(newTeamID);
        }

        public void UpdateAvailable(int teamID)
        {
            var customer = ShopManager.Customers.Find(t => t.TeamID == teamID);
            if (customer == null)
                return;
            
            var cells = new List<ICell>();
            foreach (var region in _regions)
            {
                foreach (var cell in region.GetCells())
                {
                    if (cell.TeamID == teamID)
                    {
                        cells.Add(cell);
                        var neighbours = cell.FindNeighbours();
                        foreach (var n in neighbours)
                        {
                            if (n.TeamID == cell.TeamID)
                                continue;

                            var neighbourRegion = _regions.Find(r => r.GetCells().Contains(n));
                            if (neighbourRegion == null)
                                continue;

                            cells.AddRange(neighbourRegion.GetCells());
                        }
                    }
                }
            }
            
            foreach (var cell in _regions.SelectMany(region => region.GetCells()))
            {
                if (cells.Contains(cell))
                {
                    if (!customer.AvailableCells.Contains(cell))
                    {
                        customer.AvailableCells.Add(cell);
                        if (customer.TeamID == 1)
                            cell.SetFog(false);
                    }
                }
                else
                {
                    if (customer.AvailableCells.Contains(cell))
                    {
                        customer.AvailableCells.Remove(cell);
                        if (customer.TeamID == 1)
                            cell.SetFog(true);
                    }
                }
            }
        }
    }
}