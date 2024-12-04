using Football.Services.DataLoaders;
using Quartz;

namespace Football.Jobs;

public class RecentMatchesUpdateJob(MatchesLoader matchesLoader) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await matchesLoader.LoadRecentAsync();
    }
}