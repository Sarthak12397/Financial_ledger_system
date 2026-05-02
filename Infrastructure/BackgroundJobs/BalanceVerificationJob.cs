using Hangfire;
using Microsoft.Extensions.Logging;

public class BalanceVerificationJob
{
    private readonly BalanceService _balanceService;
    private readonly ILogger<BalanceVerificationJob> _logger;

    public BalanceVerificationJob(
        BalanceService balanceService,
        ILogger<BalanceVerificationJob> logger)
    {
        _balanceService = balanceService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("Balance verification started at {Time}", 
            DateTime.UtcNow);

        await _balanceService.VerifyAllBalancesAsync();

        _logger.LogInformation("Balance verification completed at {Time}", 
            DateTime.UtcNow);
    }
}