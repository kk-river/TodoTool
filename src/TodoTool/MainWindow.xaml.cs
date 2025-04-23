using System.Windows;

namespace TodoTool;

public partial class MainWindow : Window
{
    [Obsolete("For designer support")]
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}
