using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RoundProgressBar
{
	internal class BorderInlineConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values is { Length: 4 } &&
				values[0] is (double width and >= double.Epsilon) &&
				values[1] is (double height and >= double.Epsilon) &&
				values[2] is CornerRadius radius &&
				values[3] is Thickness thickness)
			{
				var rect = new Rect(0, 0, (width - thickness.Left - thickness.Right), (height - thickness.Top - thickness.Bottom));
				var radiusX = radius.TopLeft - thickness.Left / 2D;
				var radiusY = radius.TopLeft - thickness.Top / 2D;

				var clip = new RectangleGeometry(rect, radiusX, radiusY);
				clip.Freeze();

				return clip;
			}

			return DependencyProperty.UnsetValue;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}