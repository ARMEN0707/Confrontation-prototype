using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUpdatable
    {
        public void OnUpdate(float deltaTime);
    }
}