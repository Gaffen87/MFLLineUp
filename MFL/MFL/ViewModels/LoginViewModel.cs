using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFL.Services;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MFL.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
	private readonly MFLApi mfl;
    public LoginViewModel()
    {
        mfl = new MFLApi();
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
