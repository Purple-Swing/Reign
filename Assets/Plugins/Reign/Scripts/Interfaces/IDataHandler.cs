using Reign.API.Saving;

namespace Reign.Interfaces
{
    public interface IDataHandler
    {
        void LoadData(GameData data);
        void SaveData(ref GameData data);
    }
}