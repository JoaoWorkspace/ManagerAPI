using ManagerAPI.Application.TorrentArea.Dtos.Enums;

namespace ManagerAPI.Request
{
    public class TorrentRequest
    {
        
        public ManagedAction Action { get; set; }
        public ActionTarget ActionTarget { get; set; }
        public string PathToFile1 { get; set; }
        public string PathToFolder1 { get; set; }
        public ActionConnector ActionConnector { get; set; }
        public IFormFile Data { get; set; }
        public string PathToFile2 { get; set; }
        public string PathToFolder2 { get; set; }
    }
}
