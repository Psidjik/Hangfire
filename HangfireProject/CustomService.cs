namespace Hangfire;

public class CustomService(ILogger<CustomService> logger)   
{
    public void DoWork(string name)
    {
        try
        {
            // Симуляция ошибки
            if (DateTime.Now.Second % 2 == 0)
            {
                throw new Exception("Something went wrong!");
            }

            Console.WriteLine($"{name}, thank you for your job!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in DoWork method");
            throw;
        }
    }
}