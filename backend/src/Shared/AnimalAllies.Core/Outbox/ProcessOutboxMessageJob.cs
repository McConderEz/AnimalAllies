using Quartz;

namespace AnimalAllies.Core.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxMessageJob: IJob
{
    private readonly ProcessOutboxMessageService _processOutboxMessageService;

    public ProcessOutboxMessageJob(ProcessOutboxMessageService processOutboxMessageService)
    {
        _processOutboxMessageService = processOutboxMessageService;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await _processOutboxMessageService.Execute(context.CancellationToken);
    }
}