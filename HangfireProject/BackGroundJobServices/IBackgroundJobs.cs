namespace Hangfire.BackGroundJobServices;

public interface IBackgroundJobs
{
    void BackgroundTask(string name);
}