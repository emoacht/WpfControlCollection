using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DialDemo
{
	[TemplatePart(Name = "PART_Dial", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_Rotation", Type = typeof(RotateTransform))]
	public class Dial : ContentControl
	{
		#region Property

		public double Minimum
		{
			get { return (double)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register(
				"Minimum",
				typeof(double),
				typeof(Dial),
				new PropertyMetadata(
					0D,
					(d, e) =>
					{
						var instance = (Dial)d;
						instance.Angle = Math.Max((double)e.NewValue, instance.Angle);
					},
					(d, baseValue) => Math.Max(0D, (double)baseValue)));

		public double Maximum
		{
			get { return (double)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register(
				"Maximum",
				typeof(double),
				typeof(Dial),
				new PropertyMetadata(
					360D,
					(d, e) =>
					{
						var instance = (Dial)d;
						instance.Angle = Math.Min((double)e.NewValue, instance.Angle);
					},
					(d, baseValue) => Math.Min(360D, (double)baseValue)));

		public double Angle
		{
			get { return (double)GetValue(AngleProperty); }
			set { SetValue(AngleProperty, value); }
		}
		public static readonly DependencyProperty AngleProperty =
			DependencyProperty.Register(
				"Angle",
				typeof(double),
				typeof(Dial),
				new PropertyMetadata(
					0D,
					(d, e) =>
					{
						var rotation = ((Dial)d)._rotation;
						if (rotation is not null)
							rotation.Angle = ConvertFrom360To180((double)e.NewValue);
					},
					(d, baseValue) =>
					{
						var instance = (Dial)d;
						if (instance.Minimum >= instance.Maximum)
							throw new InvalidOperationException();

						var value = (double)baseValue;
						if (value is < 0D or >= 360D)
							value = 0D;

						if (instance.Minimum <= value && value <= instance.Maximum)
							return value;

						var reminder = (instance.Minimum + (360D - instance.Maximum)) / 2D;
						if (reminder <= instance.Minimum)
						{
							if (instance.Minimum - reminder <= value && value < instance.Minimum)
								return instance.Minimum;

							return instance.Maximum;
						}
						else
						{
							if (instance.Maximum < value && value <= instance.Maximum + reminder)
								return instance.Maximum;

							return instance.Minimum;
						}
					}));

		#endregion

		private FrameworkElement? _dial;
		private RotateTransform? _rotation;

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_dial = this.GetTemplateChild("PART_Dial") as FrameworkElement;
			_rotation = this.GetTemplateChild("PART_Rotation") as RotateTransform;

			if (_dial is not null)
			{
				_dial.MouseDown += OnDialMouseDown;
				_dial.MouseMove += OnDialMouseMove;
				_dial.MouseUp += OnDialMouseLeave;
				_dial.MouseLeave += OnDialMouseLeave;
				_dial.MouseWheel += OnDialMouseWheel;
			}

			if (_rotation is not null)
				_rotation.Angle = ConvertFrom360To180(Angle);
		}

		private bool _isMoving;

		private void OnDialMouseDown(object sender, MouseButtonEventArgs e)
		{
			Angle = ConvertFrom180To360(GetAngle((FrameworkElement)sender, e));

			_isMoving = true;
		}

		private void OnDialMouseMove(object sender, MouseEventArgs e)
		{
			if (!_isMoving)
				return;

			Angle = ConvertFrom180To360(GetAngle((FrameworkElement)sender, e));
		}

		private void OnDialMouseLeave(object sender, MouseEventArgs e)
		{
			_isMoving = false;
		}

		private void OnDialMouseWheel(object sender, MouseWheelEventArgs e)
		{
			Angle -= e.Delta / 120D;
		}

		private static double GetAngle(FrameworkElement element, MouseEventArgs e)
		{
			var v1 = new Vector(0, -1);
			var v2 = GetVector(element, e);
			return Vector.AngleBetween(v1, v2);

			static Vector GetVector(FrameworkElement element, MouseEventArgs e)
			{
				var center = new Point(element.ActualWidth / 2D, element.ActualHeight / 2D);
				var position = e.GetPosition(element);
				return new Vector(position.X - center.X, position.Y - center.Y);
			}
		}

		private static double ConvertFrom180To360(double value)
		{
			return value switch
			{
				>= -180 and < 0 => value + 360,
				<= 180 => value,
				_ => throw new InvalidOperationException()
			};
		}

		private static double ConvertFrom360To180(double value)
		{
			return value switch
			{
				>= 0 and <= 180 => value,
				<= 360 => value - 360,
				_ => throw new InvalidOperationException()
			};
		}
	}
}