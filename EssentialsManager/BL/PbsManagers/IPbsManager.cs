namespace BL.PbsManagers;

public interface IPbsManager
{
    void ChangeFolderPath(string folderpath);
    void LoadAllPbsFiles();
    bool HasDataSaved();
}