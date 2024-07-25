using System.Collections.Generic;
using Core;
using Data;
using UnityEngine;
using Views;

namespace Interfaces
{
    public interface ICell : IActor
    {
        public CellView CellView { get; }
        public int TeamID { get; set; }
        public IBuilding Building { get; set; }
        public void CreateBuilding(BuildingType type);
        public List<ICell> FindNeighbours();
        public void DrawLine(Vector3 p1, Vector3 p2);
        public void SetFog(bool active);
    }
}