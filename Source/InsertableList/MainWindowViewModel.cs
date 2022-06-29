using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace InsertableList
{
	public class MainWindowViewModel : ObservableObject
	{
		public ObservableCollection<MemberViewModel> Members { get; } = new();

		public MainWindowViewModel()
		{
			Add("Alfa");
			Add("Bravo");
			Add("Charlie");
		}

		private void Add(string name)
		{
			Members.Add(new MemberViewModel(name, m => Insert(m), m => Remove(m)));
		}

		private void Insert(MemberViewModel m)
		{
			int index = Members.IndexOf(m);
			if (index >= 0)
				Members.Insert(index, new MemberViewModel($"{m.Name}+", m => Insert(m), m => Remove(m)));
		}

		private void Remove(MemberViewModel m)
		{
			int index = Members.IndexOf(m);
			if (index >= 0)
				Members.RemoveAt(index);
		}
	}

	public class MemberViewModel : ObservableObject
	{
		public string? Name { get; }
		public ICommand InsertCommand { get; }
		public ICommand RemoveCommand { get; }

		public MemberViewModel(string name, Action<MemberViewModel> insert, Action<MemberViewModel> remove)
		{
			this.Name = name;
			InsertCommand = new RelayCommand(() => insert?.Invoke(this));
			RemoveCommand = new RelayCommand(() => remove?.Invoke(this));
		}
	}
}