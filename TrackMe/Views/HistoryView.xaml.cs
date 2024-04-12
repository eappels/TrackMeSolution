using TrackMe.ViewModels;

namespace TrackMe.Views;

public partial class HistoryView : ContentPage
{
	public HistoryView(HistoryViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}