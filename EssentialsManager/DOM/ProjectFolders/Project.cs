using System.ComponentModel.DataAnnotations;

namespace DOM.ProjectFolders;

public class Project(string name, string folderPath, string photo)
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string FolderPath { get; set; } = folderPath;
    public string Photo { get; set; } = photo;
}