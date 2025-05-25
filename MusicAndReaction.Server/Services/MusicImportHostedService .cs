using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class MusicImportHostedService : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<MusicImportHostedService> _logger;
    private readonly IWebHostEnvironment _env;

    public MusicImportHostedService(IServiceProvider sp, ILogger<MusicImportHostedService> logger, IWebHostEnvironment env)
    {
        _sp = sp;
        _logger = logger;
        _env = env;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // np. raz przy starcie
        await Task.Yield();

        using var scope = _sp.CreateScope();
        var importer = scope.ServiceProvider.GetRequiredService<MusicImportService>();

        try
        {
            string folder = _env.WebRootPath+"/music";
            _logger.LogInformation("Rozpoczynam import z {folder}", folder);
            await importer.ImportAsync(folder);
            _logger.LogInformation("Import zakończony.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Import nie powiódł się");
        }
    }
}
