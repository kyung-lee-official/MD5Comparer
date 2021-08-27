using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using MD5Comparer.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace MD5Comparer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindowEventArgs mainWindowEventArgs = new MainWindowEventArgs();
		public delegate void MainWindowEventHandler(object sender, MainWindowEventArgs e);
		public static event MainWindowEventHandler MainWindowSizeChanged;
		private string _firstPath;
		public string FirstPath
		{
			get { return _firstPath; }
			set { _firstPath = value; }
		}

		private string _secondPath;
		public string SecondPath
		{
			get { return _secondPath; }
			set { _secondPath = value; }
		}

		private List<string> _firstFileList;
		public List<string> FirstFileList
		{
			get { return _firstFileList; }
			set { _firstFileList = value; }
		}

		private List<string> _secondFileList;
		public List<string> SecondFileList
		{
			get { return _secondFileList; }
			set { _secondFileList = value; }
		}

		private List<string> _firstFileNameList;
		public List<string> FirstFileNameList
		{
			get { return _firstFileNameList; }
			set { _firstFileNameList = value; }
		}

		private List<string> _secondFileNameList;
		public List<string> SecondFileNameList
		{
			get { return _secondFileNameList; }
			set { _secondFileNameList = value; }
		}

		public Brush TextSolidColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d3436"));
		public Brush TextWrongSolidColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dfe6e9"));

		public MainWindow()
		{
			InitializeComponent();
		}

		private void SelectPath1Button_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new CommonOpenFileDialog();
			dialog.IsFolderPicker = true;
			CommonFileDialogResult result = dialog.ShowDialog();
			if (result == CommonFileDialogResult.Ok)
			{
				FirstPath = dialog.FileName;
				if (FirstPath == SecondPath)
				{
					MessageBox.Show("Two paths must be different.", " MD5 Comparer", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				textBox_FirstPath.Text = FirstPath;
				FirstFileList = new List<string>(Directory.GetFiles(FirstPath));
				FirstFileNameList = new List<string>();
				foreach (var item in FirstFileList)
				{
					FirstFileNameList.Add(System.IO.Path.GetFileName(item));
				}
			}
		}

		private void SelectPath2Button_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new CommonOpenFileDialog();
			dialog.IsFolderPicker = true;
			CommonFileDialogResult result = dialog.ShowDialog();
			if (result == CommonFileDialogResult.Ok)
			{
				SecondPath = dialog.FileName;
				if (FirstPath == SecondPath)
				{
					MessageBox.Show("Two paths must be different.", " MD5 Comparer", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				textBox_SecondPath.Text = SecondPath;
				SecondFileList = new List<string>(Directory.GetFiles(SecondPath));
				SecondFileNameList = new List<string>();
				foreach (var item in SecondFileList)
				{
					SecondFileNameList.Add(System.IO.Path.GetFileName(item));
				}
			}
		}

		private void CompareButton_Click(object sender, RoutedEventArgs e)
		{
			string hash1;
			string hash2;
			var md5 = MD5.Create();
			CompareTwoListBoxItem compareTwoListBoxItem;

			compareMatchedListBox.Items.Clear();
			compareNotMatchedListBox.Items.Clear();
			compareDifferentNameListBox.Items.Clear();

			if (FirstPath == null | SecondPath == null)
			{
				MessageBox.Show("Please select paths.", " MD5 Comparer", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			foreach (var item in FirstFileNameList)
			{
				if (SecondFileNameList.Contains(item))
				{
					using (var stream = File.OpenRead(FirstPath + "\\" + item))
					{
						hash1 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
					}
					using (var stream = File.OpenRead(SecondPath + "\\" + item))
					{
						hash2 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
					}
					if (hash1 == hash2)
					{
						compareTwoListBoxItem = new CompareTwoListBoxItem();
						compareTwoListBoxItem.Width = compareMatchedListBox.ActualWidth - 30;
						compareTwoListBoxItem.Background = compareTwoListBoxItem.correctSolidColorBrush;
						compareTwoListBoxItem.firstLabel.Width = compareTwoListBoxItem.Width / 2;
						compareTwoListBoxItem.secondLabel.Width = compareTwoListBoxItem.Width / 2;
						compareTwoListBoxItem.firstTextBlock.Foreground = TextSolidColorBrush;
						compareTwoListBoxItem.secondTextBlock.Foreground = TextSolidColorBrush;
						compareTwoListBoxItem.firstTextBlock.Text = FirstPath + "\\" + item + "\n" + hash1;
						compareTwoListBoxItem.secondTextBlock.Text = SecondPath + "\\" + item + "\n" + hash2;
						compareMatchedListBox.Items.Add(compareTwoListBoxItem);
					}
					else
					{
						compareTwoListBoxItem = new CompareTwoListBoxItem();
						compareTwoListBoxItem.Width = compareMatchedListBox.ActualWidth - 30;
						compareTwoListBoxItem.Background = compareTwoListBoxItem.wrongSolidColorBrush;
						compareTwoListBoxItem.firstLabel.Width = compareTwoListBoxItem.Width / 2;
						compareTwoListBoxItem.secondLabel.Width = compareTwoListBoxItem.Width / 2;
						compareTwoListBoxItem.firstTextBlock.Foreground = TextWrongSolidColorBrush;
						compareTwoListBoxItem.secondTextBlock.Foreground = TextWrongSolidColorBrush;
						compareTwoListBoxItem.firstTextBlock.Text = FirstPath + "\\" + item + "\n" + hash1;
						compareTwoListBoxItem.secondTextBlock.Text = SecondPath + "\\" + item + "\n" + hash2;
						compareNotMatchedListBox.Items.Add(compareTwoListBoxItem);
					}
				}
				else
				{
					compareTwoListBoxItem = new CompareTwoListBoxItem();
					compareTwoListBoxItem.Width = compareMatchedListBox.ActualWidth - 30;
					compareTwoListBoxItem.firstLabel.Width = compareTwoListBoxItem.Width / 2;
					compareTwoListBoxItem.secondLabel.Width = compareTwoListBoxItem.Width / 2;
					compareTwoListBoxItem.firstTextBlock.Foreground = TextSolidColorBrush;
					compareTwoListBoxItem.secondTextBlock.Foreground = TextSolidColorBrush;
					compareTwoListBoxItem.firstTextBlock.Text = FirstPath + "\\" + item;
					compareDifferentNameListBox.Items.Add(compareTwoListBoxItem);
				}
			}
			foreach (var item in SecondFileNameList)
			{
				if (FirstFileNameList.Contains(item) != true)
				{
					compareTwoListBoxItem = new CompareTwoListBoxItem();
					compareTwoListBoxItem.Width = compareMatchedListBox.ActualWidth - 30;
					compareTwoListBoxItem.firstLabel.Width = compareTwoListBoxItem.Width / 2;
					compareTwoListBoxItem.secondLabel.Width = compareTwoListBoxItem.Width / 2;
					compareTwoListBoxItem.firstTextBlock.Foreground = TextSolidColorBrush;
					compareTwoListBoxItem.secondTextBlock.Foreground = TextSolidColorBrush;
					compareTwoListBoxItem.secondTextBlock.Text = SecondPath + "\\" + item;
					compareDifferentNameListBox.Items.Add(compareTwoListBoxItem);
				}
			}
		}

		private void compareListBox_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			mainWindowEventArgs.MainWindowCompareListWidth = compareMatchedListBox.ActualWidth - 30;
			if (MainWindowSizeChanged != null)
			{
				MainWindowSizeChanged(this, mainWindowEventArgs);
			}
		}
	}

	public class MainWindowEventArgs : EventArgs
	{
		public double MainWindowCompareListWidth;
	}
}
