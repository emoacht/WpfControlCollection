using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SwitchableComboBox
{
	public class MouseFocusableTextBlock : TextBlock
	{
		public MouseFocusableTextBlock() : base()
		{
			Focusable = true;

			MouseDown += (_, _) => { Keyboard.Focus(this); };
		}
	}
}