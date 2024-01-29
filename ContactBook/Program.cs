using ContactBook.Interfaces;
using ContactBook.Repositories;
using ContactBook.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddSingleton<IContactRepository, ContactRepository>(provider => new ContactRepository(provider.GetRequiredService<FileService>()));
    services.AddSingleton<FileService>();
    services.AddSingleton<MenuService>();
}).Build();

builder.Start();

var menuService = builder.Services.GetRequiredService<MenuService>();

menuService.Show_MainMenu();