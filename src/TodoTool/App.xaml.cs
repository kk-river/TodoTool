using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace TodoTool;

public partial class App : Application
{
    private readonly ILogger<App> _logger;

    [Obsolete("For design time only", true)]
    public App() : this(null!, null!) { }

    [ActivatorUtilitiesConstructor]
    public App(ILogger<App> logger, IServiceProvider provider)
    {
        InitializeComponent();

        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));

        _logger = logger;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        WpfProviderInitializer.SetDefaultObservableSystem(ex => _logger.ZLogError($"R3 UnhandledException:{ex}"));
    }
}
