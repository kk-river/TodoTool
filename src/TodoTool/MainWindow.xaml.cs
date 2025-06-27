using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace TodoTool;

public partial class MainWindow : Window
{
    [Obsolete("This constructor is for design-time use only.", true)]
    public MainWindow() : this(null!) { }

    [ActivatorUtilitiesConstructor]
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}
