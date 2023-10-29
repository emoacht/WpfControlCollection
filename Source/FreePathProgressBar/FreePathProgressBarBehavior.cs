using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Xaml.Behaviors;

namespace FreePathProgressBar;

public class FreePathProgressBarBehavior : Behavior<Path>
{
	public double Minimum
	{
		get { return (double)GetValue(MinimumProperty); }
		set { SetValue(MinimumProperty, value); }
	}
	public static readonly DependencyProperty MinimumProperty =
		DependencyProperty.Register("Minimum", typeof(double), typeof(FreePathProgressBarBehavior), new PropertyMetadata(0D));

	public double Maximum
	{
		get { return (double)GetValue(MaximumProperty); }
		set { SetValue(MaximumProperty, value); }
	}

	public static readonly DependencyProperty MaximumProperty =
		DependencyProperty.Register("Maximum", typeof(double), typeof(FreePathProgressBarBehavior), new PropertyMetadata(100D));

	public double Value
	{
		get { return (double)GetValue(ValueProperty); }
		set { SetValue(ValueProperty, value); }
	}
	public static readonly DependencyProperty ValueProperty =
		DependencyProperty.Register("Value", typeof(double), typeof(FreePathProgressBarBehavior),
			new PropertyMetadata(
				0D,
				(d, e) => ((FreePathProgressBarBehavior)d).Draw((double)e.NewValue)));

	public double StartOffset
	{
		get { return (double)GetValue(StartOffsetProperty); }
		set { SetValue(StartOffsetProperty, value); }
	}
	public static readonly DependencyProperty StartOffsetProperty =
		DependencyProperty.Register("StartOffset", typeof(double), typeof(FreePathProgressBarBehavior),
			new PropertyMetadata(
				0D,
				(d, e) =>
				{
					var instance = (FreePathProgressBarBehavior)d;
					instance.Draw(instance.Value);
				}));

	protected override void OnAttached()
	{
		base.OnAttached();

		this.AssociatedObject.Loaded += OnLoad;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();

		this.AssociatedObject.Loaded -= OnLoad;
	}

	private void OnLoad(object sender, RoutedEventArgs e)
	{
		Draw(this.Value);
	}

	private double? _length;

	private void Draw(double value)
	{
		if ((_length is null) && (this.AssociatedObject?.Data is not null))
		{
			_length = GetGeometryLength(this.AssociatedObject.Data);
		}

		if (_length is not > 0)
			return;

		var denominator = (this.Maximum - this.Minimum);
		Draw(this.AssociatedObject!, _length.Value, this.StartOffset / denominator, value / denominator);
	}

	private static void Draw(Path path, double totalLength, double offsetRate, double valueRate)
	{
		if ((path is null) || (totalLength <= 0))
			return;

		totalLength /= path.StrokeThickness;

		var offsetLength = totalLength * offsetRate;
		var valueLength = totalLength * valueRate;

		var d0 = 0D;
		var d1 = offsetLength;
		var d2 = valueLength;
		var d3 = totalLength - valueLength;

		var surplus = offsetLength + valueLength - totalLength;
		if (surplus > 0)
		{
			d0 = surplus;
			d1 -= surplus;
		}

		path.StrokeDashArray = new DoubleCollection { d0, d1, d2, d3 };
	}

	private static double GetGeometryLength(Geometry data)
	{
		var path = data.GetFlattenedPathGeometry(0.000001, ToleranceType.Absolute);
		if (path?.Figures?.Count != 1)
			return 0;

		var figures = path.Figures[0];

		var points = new[] { figures.StartPoint }
			.Concat(figures.Segments.OfType<PolyLineSegment>().SelectMany(x => x.Points))
			.ToArray();

		Point? previous = null;
		double length = 0;

		foreach (var p in points)
		{
			if (previous is not null)
			{
				length += (p - previous).Value.Length;
			}
			previous = p;
		}
		return length;
	}
}
