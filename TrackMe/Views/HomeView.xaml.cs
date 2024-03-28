using TrackMe.ViewModels;

namespace TrackMe.Views;
public partial class HomeView : ContentPage
{
    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}