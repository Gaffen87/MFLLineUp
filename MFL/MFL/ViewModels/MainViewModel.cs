using Avalonia.Controls;
using Avalonia.SimpleRouter;
using CommunityToolkit.Mvvm.ComponentModel;
using MFL.Views;

namespace MFL.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	[ObservableProperty]
	private ViewModelBase content = default!;

    public MainViewModel(HistoryRouter<ViewModelBase> router)
    {
		router.CurrentViewModelChanged += viewModel => Content = viewModel;

		router.GoTo<LoginViewModel>();
	}
}
