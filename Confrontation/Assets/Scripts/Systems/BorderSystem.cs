using System.Collections.Generic;
using Core;
using Interfaces;
using UnityEngine;

namespace Systems
{
    public class BorderSystem : BaseSystem<IRegion>
    {
        private List<IRegion> _regions = new List<IRegion>();

        protected override void AddActor(IRegion warehouse)
        {
            _regions.Add(warehouse);
        }

        protected override void RemoveActor(IRegion actor)
        {
            _regions.Remove(actor);
        }

        public void FindNeighbours()
        {
            foreach (var r in _regions)
            {
                foreach (var c in r.GetCells())
                {
                    var cells = c.FindNeighbours();
                    foreach (var cell in cells)
                    {
                        if (!r.GetCells().Contains(cell))
                        {
                            var a = 0.36f * 2 * Mathf.Sqrt(3) / 3;
                            var vector = (cell.CellView.Position - c.CellView.Position).normalized * a;
                            var p1 = Quaternion.Euler(0, 0, 30) * vector + 
                                c.CellView.Position - Vector3.forward;
                            var p2 = Quaternion.Euler(0, 0, -30) * vector + 
                                c.CellView.Position - Vector3.forward;
                            cell.DrawLine(p1, p2);
                        }
                    }
                }
            }
        }
    }
}