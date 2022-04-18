using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace FlatGridView
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<MemberItem> Members { get; } = new();

		public MainWindow()
		{
			InitializeComponent();

			Members.Add(new() { Id = 10, Name = "Elaina", Title = "Ashen Witch" });
			Members.Add(new() { Id = 1, Name = "Victrica", Title = "Ashen Witch", IsChecked = true });
			Members.Add(new() { Id = 2, Name = "Fran", Title = "Stardust Witch" });
			Members.Add(new() { Id = 3, Name = "Sheila", Title = "Night Witch" });
		}

		private void ListViewHeader_Click(object sender, RoutedEventArgs e)
		{
			var clickedHeader = (GridViewColumnHeader)e.OriginalSource;

			Debug.WriteLine($"Header Clicked! {clickedHeader.Column.Header}");
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedItem = e.AddedItems.OfType<MemberItem>().FirstOrDefault();

			Debug.WriteLine($"Selection Changed! {selectedItem?.Name}");
		}
	}
}