using System.Collections.Generic;
using Core;

namespace Interfaces
{
    public interface IRegion : IActor
    {
        public int? GetTeamID();
        public bool ContainsBuilding(IBuilding building);
        public void ChangeTeamID(int newTeamID);
        public List<ICell> GetCells();
    }
}