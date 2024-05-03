using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace TimerTextBlockDemo;

public partial class MainWindow : Window
{
	public ObservableCollection<DateTime> Times { get; } = [];

	public MainWindow()
	{
		InitializeComponent();

		var dateTime = DateTime.Now;
		Times.Add(dateTime);
		Times.Add(dateTime + TimeSpan.FromSeconds(1));
		Times.Add(dateTime + TimeSpan.FromSeconds(2));
	}
}
