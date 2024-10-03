namespace BL;

public interface IFileManager
{
    bool CheckIfImageExists(string uriTitleImage);
    bool CheckIfIsEssentialsProjectFolder(string uriFolder);
}