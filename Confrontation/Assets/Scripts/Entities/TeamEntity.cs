using Core;
using Data;

namespace Entities
{
    public class TeamEntity : BaseEntity<TeamData>
    {
        public TeamEntity(TeamData data) : base(data)
        {
        }
    }
}