using BL.DataTransferObjects;

namespace BL.PbsManagers;

public interface IPbsManager
{
    void ChangeFolderPath(string folderpath);
    void LoadAllPbsFiles();
    bool HasDataSaved();
    void SaveTypeEffectivenessChanges(ICollection<TypeEffectivenessFieldChange> changes);
    void SavePokemonAvailabilityChanges(ICollection<PokemonAvailabilityChange> changes);
    void SaveTypingsToPbsFromDatabase();
}