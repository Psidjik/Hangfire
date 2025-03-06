using Hangfire;
using Hangfire.BackGroundJobServices;
using Hangfire.PostgreSql;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<CustomService>();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

builder.Services.AddHangfire(h =>
    h.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireServiceConnection")));
builder.Services.AddTransient<IBackgroundJobs, BackgroundJobs>();
builder.Services.AddHangfireServer();
// builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/notification/fire-and-forget", (string name) =>
    {
        string jobId = BackgroundJob.Enqueue(() => Console.WriteLine($"{name}, thank you for your job!"));
        return Results.Ok($"Job id: {jobId}");
    }
);
app.MapPost("api/notification/service-background-jobs", (string name, IBackgroundJobs backgroundJobs) =>
    {
        string jobId = BackgroundJob.Enqueue(() => backgroundJobs.BackgroundTask(name));
        return Results.Ok($"Job id: {jobId}");
    }
);
app.MapPost("api/notification/delayed", (string name) =>
    {
        string jobId = BackgroundJob.Schedule(() => Console.WriteLine($"Session for client {name} has been closed."),
            TimeSpan.FromSeconds(60));
        return Results.Ok($"Job id: {jobId}");
    }
);
app.MapPost("api/notification/recurring",
    (string name) =>
    {
        RecurringJob.AddOrUpdate(() => Console.WriteLine("Its recurring! GOOD DAY, BRO!)"), Cron.Minutely());
        return Results.Ok();
    }
);
app.MapPost("api/notification/continuations",
    (string name) =>
    {
        string jobId = BackgroundJob.Enqueue(() =>
            Console.WriteLine($"Check balance logic for {name}"));
        
        // string jobId = BackgroundJob.Schedule(() => Console.WriteLine($"Session for client {name} has been closed."), TimeSpan.FromSeconds(60));
        
        BackgroundJob.ContinueJobWith(jobId, () =>
            Console.WriteLine($"{name}, your balance has been changed."));
        // BackgroundJob.Delete("10");
        // BackgroundJob.Requeue("1");
        return Results.Ok();
    }
);
app.MapPost("api/notification/batch-jobs",
    (string name) =>
    {
        var batchId = (() =>
            Console.WriteLine($"Check balance logic for {name}"));
        return Results.Ok();
    }
);
app.MapPost("api/notification/custom-service", (string name, CustomService service) =>
{
    string jobId = BackgroundJob.Enqueue(() => service.DoWork(name));
    return Results.Ok($"Job id: {jobId}");
});

app.UseHangfireDashboard("/hangfire");

app.UseHealthChecks("/health");
app.Run();