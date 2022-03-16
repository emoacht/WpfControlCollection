using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlaskProgressBarDemo
{
	[TemplatePart(Name = "PART_Content", Type = typeof(RectangleGeometry))]
	public class FlaskProgressBar : ProgressBar
	{
		public FlaskProgressBar() : base()
		{
			ValueProperty.OverrideMetadata(typeof(FlaskProgressBar),
				new FrameworkPropertyMetadata(0D, (d, e) => SetValue((FlaskProgressBar)d, (double)e.NewValue)));
		}

		private RectangleGeometry? _content;

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_content = this.GetTemplateChild("PART_Content") as RectangleGeometry;
			SetValue(this, Value);
		}

		private static void SetValue(FlaskProgressBar instance, double value)
		{
			if (instance._content is not null)
			{
				var rect = instance._content.Rect;
				instance._content.Rect = new Rect(rect.X, rect.Height - value, rect.Width, rect.Height);
			}
		}
	}
}