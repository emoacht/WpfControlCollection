using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StripedProgressBar
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private async void Start_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.StartButton.IsEnabled = false;
				this.Slider.IsEnabled = false;

				this.Bar.Value = 0;

				while (this.Bar.Value < 100D)
				{
					await Task.Delay(TimeSpan.FromMilliseconds(50));

					this.Bar.Value += (this.Bar.Value < 90) ? 1 : 0.25;
				}
			}
			finally
			{
				this.StartButton.IsEnabled = true;
				this.Slider.IsEnabled = true;
			}
		}
	}
}
