using ManagerAPI.Application.MusicArea.Commands.OpenFile;
using ManagerAPI.Application.MusicArea;
using ManagerAPI.ExceptionHandling;
using ManagerAPI.Response;
using Microsoft.AspNetCore.Mvc;

namespace ManagerAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class UserController : ControllerBase
    {
        public readonly IMusicService musicService;

        public UserController(IMusicService musicService)
        {
            this.musicService = musicService;
        } 

        [HttpPut("PlayFile")]
        public async Task<FileManagerOutput> PlaySongAsync(string fullPath, CancellationToken cancellationToken)
        {
            DateTime start = DateTime.UtcNow;

            try
            {
                var result = await musicService.PlaySongAsync(new OpenFileCommand(fullPath), cancellationToken);
                return new FileManagerOutput(Ok(result), start);
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("CloseFileProcessIfExists", $"Failed to close a proocess including the file {fullPath}.", ex);
                return new FileManagerOutput(BadRequest(ex.Message), start);
            }
        }
    }
}
