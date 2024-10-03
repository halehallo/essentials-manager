using DAL;
using System;
using System.Drawing; // Ensure you have System.Drawing.Common NuGet package installed
using System.IO;
using BL.Exceptions;

namespace BL;

public class FileManager : IFileManager
{
    public bool CheckIfImageExists(string uriTitleImage)
    {
        if (!File.Exists(uriTitleImage))
        {
            throw new WrongImageException("The title image does not exist.");
        }

        try
        {
            using (var image = Image.FromFile(uriTitleImage))
            {
                // Successfully loaded the image
                return true;
            }
        }
        catch (OutOfMemoryException)
        {
            // The file does not have a valid image format or GDI+ does not support the pixel format of the file.
            throw new WrongImageException("The file does not have a valid image format or GDI+ does not support the pixel format of the file");
        }
        catch (FileNotFoundException)
        {
            // The file does not exist.
            throw new WrongImageException("The title image does not exist.");
        }
        catch (Exception)
        {
            // Other exceptions
            throw new WrongImageException("Something went wrong with the image. Try to replicate this error in debug mode to know more.");
        }
    }

    public bool CheckIfIsEssentialsProjectFolder(string uriFolder)
    {
        // Check if the provided directory exists
        if (!Directory.Exists(uriFolder))
        {
            return false;
        }

        // Required folders
        string[] requiredFolders = { "Audio", "Data", "Fonts", "Graphics", "PBS", "Plugins" };

        // Check for each required folder
        foreach (string folder in requiredFolders)
        {
            string folderPath = Path.Combine(uriFolder, folder);
            if (!Directory.Exists(folderPath))
            {
                return false; // Required folder not found
            }
        }

        // Check for .rxproj and .exe files in the root of the folder
        bool hasRxproj = Directory.EnumerateFiles(uriFolder, "*.rxproj", SearchOption.TopDirectoryOnly).Any();
        bool hasExe = Directory.EnumerateFiles(uriFolder, "*.exe", SearchOption.TopDirectoryOnly).Any();

        // Both the .rxproj and .exe file must be present
        return hasRxproj && hasExe;
    }
}