using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace TodoTool;

public partial class App : Application
{
    private readonly ILogger<App> _logger;
    private readonly Window _window;

    [Obsolete("For designer support")]
    public App()
    {
        InitializeComponent();

        _logger = null!;
        _window = null!;
    }

    [ActivatorUtilitiesConstructor]
    public App(ILogger<App> logger, IServiceProvider provider)
    {
        InitializeComponent();

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _window = provider.GetRequiredService<Window>(); //Must be created after App.InitializeComponent for fluent design
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        WpfProviderInitializer.SetDefaultObservableSystem(ex => _logger.ZLogError($"R3 UnhandledException:{ex}"));

        _window.Show();
    }
}
