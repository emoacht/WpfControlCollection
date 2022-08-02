using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StagedButtonDemo
{
	public class StagedButton : Button
	{
		public int Stage
		{
			get { return (int)GetValue(StageProperty); }
			set { SetValue(StageProperty, value); }
		}
		public static readonly DependencyProperty StageProperty =
			DependencyProperty.Register("Stage", typeof(int), typeof(StagedButton),
				new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null,
					(d, baseValue) => Math.Min(((StagedButton)d).StageCount, Math.Max(1, (int)baseValue))));

		public int StageCount
		{
			get { return (int)GetValue(StageCountProperty); }
			set { SetValue(StageCountProperty, value); }
		}
		public static readonly DependencyProperty StageCountProperty =
			DependencyProperty.Register("StageCount", typeof(int), typeof(StagedButton),
				new PropertyMetadata(1, null,
					(_, baseValue) => Math.Max(1, (int)baseValue)));

		protected override void OnClick()
		{
			var stage = Stage;
			Stage = (++stage <= StageCount) ? stage : 1;

			base.OnClick();
		}
	}
}