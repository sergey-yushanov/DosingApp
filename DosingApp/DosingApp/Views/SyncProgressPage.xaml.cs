using DosingApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace DosingApp.Views
{
    public partial class SyncProgressPage : PopupPage
    {
        public SyncProgressPage(SyncProgressViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}