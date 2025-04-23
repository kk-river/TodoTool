using System.Windows;

namespace TodoTool;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainWindowViewModel viewModel) : this()
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}
