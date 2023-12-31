﻿using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.SimpleRouter;
using MFL.ViewModels;
using MFL.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MFL;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

		IServiceProvider services = ConfigureServices();
		var mainViewModel = services.GetRequiredService<MainViewModel>();

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

	private static ServiceProvider ConfigureServices()
	{
		var services = new ServiceCollection();
		// Add the HistoryRouter as a service
		services.AddSingleton<HistoryRouter<ViewModelBase>>(s => new HistoryRouter<ViewModelBase>(t => (ViewModelBase)s.GetRequiredService(t)));

		// Add the ViewModels as a service (Main as singleton, others as transient)
		services.AddSingleton<MainViewModel>();
		services.AddTransient<LoginViewModel>();
		services.AddTransient<RosterViewModel>();
		return services.BuildServiceProvider();
	}
}
