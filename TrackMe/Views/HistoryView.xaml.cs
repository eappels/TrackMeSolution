using Microsoft.Maui.Controls.Maps;
using TrackMe.ViewModels;

namespace TrackMe.Views;

public partial class HistoryView : ContentPage
{
	public HistoryView(HistoryViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

        viewModel.Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 6
        };

        MyMap.MapElements.Add(viewModel.Track);
    }
}