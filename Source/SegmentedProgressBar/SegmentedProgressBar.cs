using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SegmentedProgressBarDemo;

public record Segment(int Value, Brush Brush);

public class SegmentedProgressBar : ProgressBar
{
	public IEnumerable<Segment> Segments
	{
		get { return (IEnumerable<Segment>)GetValue(SegmentsProperty); }
		set { SetValue(SegmentsProperty, value); }
	}
	public static readonly DependencyProperty SegmentsProperty =
		DependencyProperty.Register(
			nameof(Segments),
			typeof(IEnumerable<Segment>),
			typeof(SegmentedProgressBar),
			new PropertyMetadata(null, OnSegmentsChanged));

	private static void OnSegmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var bar = (SegmentedProgressBar)d;
		bar.Draw();

		if (e.OldValue is INotifyCollectionChanged oldCollection)
		{
			oldCollection.CollectionChanged -= bar.OnCollectionChanged;
		}
		if (e.NewValue is INotifyCollectionChanged newCollection)
		{
			newCollection.CollectionChanged += bar.OnCollectionChanged;
		}
	}

	private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args) => Draw();

	private void Draw()
	{
		var segments = Segments?.OrderBy(x => x.Value).ToArray();
		if (segments is not { Length: > 0 })
		{
			this.Foreground = null;
			this.Value = 0;
			return;
		}

		DrawingVisual drawingVisual = new DrawingVisual();
		DrawingContext drawingContext = drawingVisual.RenderOpen();

		int start = 0;

		foreach (var segment in segments)
		{
			drawingContext.DrawLine(
				pen: new Pen(segment.Brush, 1),
				point0: new Point(start, 0),
				point1: new Point(segment.Value, 0));

			start = segment.Value;
		}

		drawingContext.Close();

		this.Foreground = new VisualBrush(drawingVisual);
		this.Value = segments.Last().Value;
	}
}
