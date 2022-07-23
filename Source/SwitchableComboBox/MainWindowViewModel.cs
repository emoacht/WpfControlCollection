using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SwitchableComboBox
{
	public enum Rank { Normal, Second, First };

	public class MainWindowViewModel : ObservableObject
	{
		public IEnumerable<Rank> Ranks { get; } = Enum.GetValues<Rank>();

		public ObservableCollection<MemberViewModel> Members { get; } = new();

		public MainWindowViewModel()
		{
			foreach (var name in new[] { "Fuki", "Takina", "Sakura", "Erica", "Hibana" })
				Members.Add(new MemberViewModel(name));
		}
	}

	public class MemberViewModel : ObservableObject
	{
		public string Name { get; }

		public Rank Rank
		{
			get => _rank;
			set => SetProperty(ref _rank, value);
		}
		private Rank _rank;

		public MemberViewModel(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));
	}
}