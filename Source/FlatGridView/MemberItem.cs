using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatGridView
{
	public class MemberItem
	{
		public int Id { get; init; }
		public string? Name { get; init; }
		public string? Title { get; init; }
		public bool IsChecked { get; set; }
	}
}