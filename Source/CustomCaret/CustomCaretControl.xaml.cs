using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomCaret
{
	public partial class CustomCaretControl : UserControl
	{
		public CustomCaretControl()
		{
			InitializeComponent();

			CustomTextBox.SelectionChanged += (_, _) => MoveCaret(CustomTextBox, CustomCaret);
			CustomTextBox.LayoutUpdated += (_, _) => MoveCaret(CustomTextBox, CustomCaret);

			CustomTextBox.LostFocus += (_, _) => CustomCaret.Visibility = Visibility.Collapsed;
			CustomTextBox.GotFocus += (_, _) => CustomCaret.Visibility = Visibility.Visible;
		}

		private static void MoveCaret(TextBox textBox, FrameworkElement caret)
		{
			var caretRect = textBox.GetRectFromCharacterIndex(textBox.CaretIndex);
			if (caretRect.IsEmpty)
				return;

			var caretLocation = caretRect.Location;

			if ((textBox.FlowDirection == FlowDirection.RightToLeft) &&
				IsCaretAtLineEndPosition(textBox))
			{
				Canvas.SetLeft(caret, 0);
			}
			else
			{
				if (!double.IsInfinity(caretLocation.X))
					Canvas.SetLeft(caret, caretLocation.X);
			}

			if (!double.IsInfinity(caretLocation.Y))
				Canvas.SetTop(caret, caretLocation.Y);

			caret.Height = caretRect.Height;
		}

		private static bool IsCaretAtLineEndPosition(TextBox textBox)
		{
			Debug.WriteLine($"Index {textBox.SelectionStart}");

			var selectionPropertyInfo = typeof(TextBox).GetProperty("TextSelectionInternal", BindingFlags.NonPublic | BindingFlags.Instance);
			var selectionInstance = selectionPropertyInfo?.GetValue(textBox) as TextSelection;
			if (selectionInstance is null)
				return false;

			//var offsetPropertyInfo = typeof(TextPointer).GetProperty("Offset", BindingFlags.NonPublic | BindingFlags.Instance);

			var selectionStart = selectionInstance.Start;
			if (selectionStart.IsAtLineStartPosition)
				return false;

			var nextPoint = selectionStart.GetNextInsertionPosition(LogicalDirection.Forward);
			if (nextPoint is null)
				return true;

			var nextLineStart = selectionStart.GetLineStartPosition(1);
			if (nextLineStart is not null)
				return nextLineStart.CompareTo(nextPoint) == 0;

			return true;
		}
	}
}