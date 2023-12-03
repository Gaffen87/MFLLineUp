using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFL.ViewModels;
public partial class RosterViewModel : ViewModelBase
{
    public RosterViewModel()
    {
            
    }

    [ObservableProperty]
    private string? franchiseID;
}
