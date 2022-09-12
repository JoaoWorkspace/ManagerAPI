using System.ComponentModel;

namespace ManagerAPI.Request;

public class FolderRequest
{
    [DefaultValue("C:/")]
    public string FolderPath { get; set; }
    [DefaultValue("C:/Users/Downloads")]
    public string SavePath { get; set; }
    [DefaultValue(0)]
    public int MaximumFolderDepth { get; set; }
}
