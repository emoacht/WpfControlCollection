using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace SegmentedProgressBarDemo;

public partial class MainWindow : Window
{
	public ObservableCollection<Segment> Segments { get; } = [];

	public MainWindow()
	{
		InitializeComponent();

		Segments.Add(new Segment(40, Brushes.SkyBlue));
		Segments.Add(new Segment(60, Brushes.DeepSkyBlue));
		Segments.Add(new Segment(70, Brushes.Orchid));
	}

	private void Add_Click(object sender, RoutedEventArgs e)
	{
		Segments.Add(new Segment(75, Brushes.Red));
	}
}