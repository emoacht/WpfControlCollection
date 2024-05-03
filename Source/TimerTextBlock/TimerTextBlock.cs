using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace TimerTextBlockDemo;

public class TimerTextBlock : TextBlock
{
	private static DispatcherTimer _timer;
	private string? _format;

	static TimerTextBlock()
	{
		_timer = new() { Interval = TimeSpan.FromSeconds(Interval) };
		_timer.Start();
	}

	public static double Interval { get; set; } = 1D;

	public DateTime Time
	{
		get { return (DateTime)GetValue(TimeProperty); }
		set { SetValue(TimeProperty, value); }
	}
	public static readonly DependencyProperty TimeProperty =
		DependencyProperty.Register(
			"Time",
			typeof(DateTime),
			typeof(TimerTextBlock),
			new PropertyMetadata(default(DateTime), OnTimeChanged));

	private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((TimerTextBlock)d).IsRunning = ((DateTime)e.NewValue != default);
	}

	private bool _isRunning;
	public bool IsRunning
	{
		get => _isRunning;
		set
		{
			if (_isRunning != value)
			{
				_isRunning = value;
				if (_isRunning)
				{
					var expression = BindingOperations.GetBindingExpression(this, TimeProperty);
					_format = expression.ParentBinding.StringFormat;

					_timer.Tick += OnTick;
				}
				else
				{
					_timer.Tick -= OnTick;
				}
			}
		}
	}

	private void OnTick(object? sender, EventArgs e)
	{
		this.Text = (DateTime.Now - Time).ToString(_format);
	}
}
