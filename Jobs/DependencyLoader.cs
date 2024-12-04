using Quartz;

namespace Football.Jobs;

public static class DependencyLoader
{
    public static void AddJobs(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseInMemoryStore();

            q.AddJob<UpcomingMatchesLoadJob>(j => j
                .WithIdentity("UpcomingMatchesLoadJob")
                .Build());

            q.AddJob<RecentMatchesUpdateJob>(j => j
                .WithIdentity("RecentMatchesUpdateJob")
                .Build());

            q.AddTrigger(t => t
                .ForJob("UpcomingMatchesLoadJob")
                .WithIdentity("UpcomingMatchesLoadJobTrigger")
                .WithSimpleSchedule(s => s
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .StartNow());

            q.AddTrigger(t => t
                .ForJob("RecentMatchesUpdateJob")
                .WithIdentity("RecentMatchesUpdateJobTrigger")
                .WithSimpleSchedule(s => s
                    .WithIntervalInHours(12)
                    .RepeatForever())
                .StartNow());
        });
    }
}