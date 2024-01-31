using ContactBook.Shared.Interfaces;
using ContactBook.Shared.Repositories;
using ContactBook.Shared.Services;
using ContactBook.WPFApp.Mvvm.ViewModels;
using ContactBook.WPFApp.Mvvm.Views;
using ContactBook.WPFApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace ContactBook.WPFApp;
public partial class App : Application
{
    private IHost? _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<FileService>();
                services.AddSingleton<IContactRepository, ContactRepository>(provider => new ContactRepository(provider.GetRequiredService<FileService>()));
                services.AddSingleton<ContactService>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
                services.AddTransient<MainMenuViewModel>();
                services.AddTransient<MainMenuView>();
                services.AddTransient<AllContactsViewModel>();
                services.AddTransient<AllContactsView>();
                services.AddTransient<EditContactViewModel>();
                services.AddTransient<EditContactView>();
                services.AddTransient<AddContactViewModel>();
                services.AddTransient<AddContactView>();
                services.AddTransient<FindContactViewModel>();
                services.AddTransient<FindContactView>();
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host!.Start();

        var mainWindow = _host!.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
