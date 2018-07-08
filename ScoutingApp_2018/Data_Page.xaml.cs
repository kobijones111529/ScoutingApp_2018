using System;
using System.Collections.Generic;
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

using System.IO;
using Path = System.IO.Path;
using Microsoft.Win32;
using System.Collections;
using System.Collections.ObjectModel;

namespace ScoutingApp_2018 {
	public partial class Data_Page : Page {
		//Create ListBoxItem for each scouting data file
		private void LoadFiles() {
			if(!Directory.Exists(App.ScoutingDataLocation))
				Directory.CreateDirectory(App.ScoutingDataLocation);

			Files_ListBox.Items.Clear();
			List<string> files = Directory.GetFiles(App.ScoutingDataLocation).ToList();
			foreach(string file in files) {
				Files_ListBox.Items.Add(new StringPair() { Key = file, Value = Path.GetFileName(file) });
			}
		}

		public Data_Page() {
			InitializeComponent();

			LoadFiles();
		}

		//Navigate to home
		private void Home_Button_Click(object sender, RoutedEventArgs e) {
			NavigationService.Navigate(new Home_Page());
		}

		//Delete selected scouting data files
		private void Delete_Button_Click(object sender, RoutedEventArgs e) {
			foreach(StringPair file_FileViewItem in Files_ListBox.SelectedItems) {
				string file = file_FileViewItem.Key;
				if(File.Exists(file))
				File.Delete(file);
			}

			LoadFiles();
		}

		//Export scouting data
		private async void MoveToFolder_Button_Click(object sender, RoutedEventArgs e) {
			if(!Directory.Exists(App.ScoutingDataLocation))
				Directory.CreateDirectory(App.ScoutingDataLocation);

			//Open folder browser and copy selected scouting data files to selected folder
			System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			if(folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				string path = folderBrowserDialog.SelectedPath;
				foreach(StringPair file_FileViewItem in Files_ListBox.SelectedItems) {
					string file = file_FileViewItem.Key;
					FileStream sourceStream = File.Open(file, FileMode.Open);
					FileStream destinationStream = File.Create(Path.Combine(path, Path.GetFileName(file)));
					await sourceStream.CopyToAsync(destinationStream);
					sourceStream.Dispose();
					destinationStream.Dispose();
				}
			}
		}
	}
}
