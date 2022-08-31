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

namespace EndTextBox
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			AdjustButtonPosition();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			AdjustButtonPosition();
		}

		private void AdjustButtonPosition()
		{
			if (this.textBox is null || this.mark is null)
				return;

			var index = textBox.Text.Length;
			var rect = textBox.GetRectFromCharacterIndex(index);

			mark.Margin = new Thickness(rect.Right, rect.Top, 0, 0);
			mark.Height = rect.Height;
		}
	}
}