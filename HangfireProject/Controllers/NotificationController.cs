/*using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Controllers;


[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    [HttpPost]
    [Route("fire-and-forget")]
    public IActionResult FireAndForget(string client)
    {
        string jobId = BackgroundJob.Enqueue(() => Console.WriteLine($"{client}, thank you for your job!"));
        return Ok($"Job id: {jobId}");
}
}*/