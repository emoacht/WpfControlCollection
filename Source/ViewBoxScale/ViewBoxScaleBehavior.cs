using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace ViewBoxScale;

public class ViewBoxScaleBehavior : Behavior<Viewbox>
{
	protected override void OnAttached()
	{
		base.OnAttached();

		SetScale();
		this.AssociatedObject.SizeChanged += OnSizeChanged;
	}

	protected override void OnDetaching()
	{
		this.AssociatedObject.SizeChanged -= OnSizeChanged;

		base.OnDetaching();
	}

	private void OnSizeChanged(object sender, SizeChangedEventArgs e) => SetScale();

	public double ScaleX
	{
		get { return (double)GetValue(ScaleXProperty); }
		set { SetValue(ScaleXProperty, value); }
	}
	public static readonly DependencyProperty ScaleXProperty =
		DependencyProperty.Register("ScaleX", typeof(double), typeof(ViewBoxScaleBehavior), new PropertyMetadata(1.0));

	public double ScaleY
	{
		get { return (double)GetValue(ScaleYProperty); }
		set { SetValue(ScaleYProperty, value); }
	}
	public static readonly DependencyProperty ScaleYProperty =
		DependencyProperty.Register("ScaleY", typeof(double), typeof(ViewBoxScaleBehavior), new PropertyMetadata(1.0));

	private void SetScale()
	{
		var child = VisualTreeHelper.GetChild(this.AssociatedObject, 0) as ContainerVisual;
		if (child is null)
			return;

		var scale = child.Transform as ScaleTransform;
		if (scale is null)
			return;

		(ScaleX, ScaleY) = (scale.ScaleX, scale.ScaleY);
	}
}