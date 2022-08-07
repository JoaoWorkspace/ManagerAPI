using Microsoft.AspNetCore.Mvc;

namespace ManagerAPI.Response
{
    public class TorrentManagerOutput
    {
        public ActionResult Response { get; set; } = new OkObjectResult("Default Ok - A proper response message was not defined.");
        public TorrentManagerOutput(OkObjectResult response)
        {
            this.Response = response;
        }
    }
}
