using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.SimpleRouter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using MFL.Services;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MFL.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
	private ViewLocator _viewLocator;
	private readonly MFLApi mfl;
	private readonly HistoryRouter<ViewModelBase> router;

	public LoginViewModel(HistoryRouter<ViewModelBase> router)
    {
        mfl = new MFLApi();
		_viewLocator = new ViewLocator();
		this.router = router;
	}

    public string Greeting => "Welcome to Login!";

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(LogInCommand))]
	private string? email = "askelysgaard@hotmail.com";

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(LogInCommand))]
	private string? password = "Gaffen87";

	[RelayCommand(CanExecute = (nameof(FormFilled)))]
	public async Task LogIn()
	{
		if (await mfl.ValidateUser(Email!, Password!))
		{
			string franchiseID = await mfl.GetFranchiseID();
			var rosterVM = router.GoTo<RosterViewModel>();
			rosterVM.FranchiseID = franchiseID;
		}
		else
		{
			var box = MessageBoxManager.GetMessageBoxStandard("Error", "Wrong Email or Password", MsBox.Avalonia.Enums.ButtonEnum.Ok);
			await box.ShowAsync();
			Email = string.Empty; 
			Password = string.Empty;
		}
	}
	private bool FormFilled()
	{
		return Email != null && Password != null;
	}
}
