using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MD5Comparer.Controls
{
	/// <summary>
	/// Interaction logic for CompareTwoListBoxItem.xaml
	/// </summary>
	public partial class CompareTwoListBoxItem : UserControl
	{
		public Brush correctSolidColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#55efc4"));
		public Brush wrongSolidColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff7675"));

		public CompareTwoListBoxItem()
		{
			InitializeComponent();
			MainWindow.MainWindowSizeChanged += OnMainWindowSizeChanged;
		}

		public void OnMainWindowSizeChanged(object sender, MainWindowEventArgs e)
		{
			Width = e.MainWindowCompareListWidth;
			firstLabel.Width = (e.MainWindowCompareListWidth) / 2;
			secondLabel.Width = (e.MainWindowCompareListWidth) / 2;
		}
	}
}
