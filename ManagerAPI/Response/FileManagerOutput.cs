using Microsoft.AspNetCore.Mvc;

namespace ManagerAPI.Response
{
    public class FileManagerOutput
    {
        public string ExecutionTime { get; set; } = "0 miliseconds";
        public List<string> Warnings { get; set; } = new List<string>();
        public ActionResult Response { get; set; } = new OkObjectResult("Default Ok - A proper response message was not defined.");
        public FileManagerOutput(ObjectResult response)
        {
            this.Response = response;
        }
        public FileManagerOutput(ObjectResult response, DateTime start)
        {
            DateTime end = DateTime.UtcNow;
            this.Response = response;
            int executionTimeMiliseconds = (int)new TimeSpan(0, 0, 0).Add(end - start).TotalMilliseconds;
            this.ExecutionTime = StringifyExecutionTime(executionTimeMiliseconds);
        }

        public string StringifyExecutionTime(int timeMiliseconds)
        {
            var y = 60 * 60 * 1000;
            var h = timeMiliseconds / y;
            var m = (timeMiliseconds - (h * y)) / (y / 60);
            var s = (timeMiliseconds - (h * y) - (m * (y / 60))) / 1000;
            var mi = timeMiliseconds - (h * y) - (m * (y / 60)) - (s * 1000);

           return $"{h} hours {m} minutes {s} seconds {mi} miliseconds";
        }
           
    }
}
