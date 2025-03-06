namespace Hangfire.BackGroundJobServices;

public class BackgroundJobs() : IBackgroundJobs
{
    public void BackgroundTask(string name)
    {
        Console.WriteLine($"{name}, thank you for your job!");
    }
}