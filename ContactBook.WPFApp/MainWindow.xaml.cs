using ContactBook.WPFApp.Mvvm.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace ContactBook.WPFApp;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}