using Reign.API.Saving;

namespace Reign.Core.Saving
{
    public interface ISaveDataKnower
    {
        void OnSave(ref SaveData data);
        void OnLoad(SaveData data);
    }
}
