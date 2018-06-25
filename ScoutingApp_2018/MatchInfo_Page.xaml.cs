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
using System.Windows.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScoutingApp_2018 {
	public partial class MatchInfo_Page : Page {
		public static MatchInfo_Page CachedPage;
		public static bool IncrementMatchNumber = false;

		public MatchInfo_Page() {
			InitializeComponent();

			Uri recorderIDs_json_uri = new Uri("/RecorderIDs.json", UriKind.Relative);
			StreamResourceInfo streamResourceInfo = Application.GetResourceStream(recorderIDs_json_uri);
			StreamReader streamReader = new StreamReader(streamResourceInfo.Stream);
			string recorderIDs_json = streamReader.ReadToEnd();
			JObject recorderIDs_jObject = JObject.Parse(recorderIDs_json);
			string[] recorderID_array = ((JArray)recorderIDs_jObject.GetValue("recorderID_array")).ToObject<string[]>();

			foreach(string recorderID in recorderID_array) {
				RecorderID_ComboBox.Items.Add(recorderID);
			}

			RecorderID_ComboBox.SelectedItem = App.FormDataCache.RecorderID?.Value;
			Alliance_ComboBox.SelectedItem = App.FormDataCache.Alliance?.Value;
			Event_ComboBox.SelectedItem = App.FormDataCache.Event?.Value;
			MatchNumber_TextBox.Text = App.FormDataCache.MatchNumber?.Value.ToString();
			TeamNumber_TextBox.Text = App.FormDataCache.TeamNumber?.Value.ToString();
		}

		public void Home_Button_Click(object sender, RoutedEventArgs e) {
			NavigationService.Navigate(new Home_Page());
		}

		private void MatchNumber_TextBox_TextChanged(object sender, TextChangedEventArgs e) {
			bool isUInt16 = UInt16.TryParse(((TextBox)sender).Text, out UInt16 outUInt16);
			if((!isUInt16 || ((TextBox)sender).Text.Contains(' ') || ((TextBox)sender).Text.StartsWith("0")) && ((TextBox)sender).Text.Length > 0) {
				int pos = ((TextBox)sender).SelectionStart;
				((TextBox)sender).Text = ((TextBox)sender).Text.Remove(pos - 1, 1);
				((TextBox)sender).SelectionStart = pos - 1;
			}
		}

		private void TeamNumber_TextBox_TextChanged(object sender, TextChangedEventArgs e) {
			bool isUInt16 = UInt16.TryParse(((TextBox)sender).Text, out UInt16 outUInt16);
			if((!isUInt16 || ((TextBox)sender).Text.Contains(' ') || ((TextBox)sender).Text.StartsWith("0")) && ((TextBox)sender).Text.Length > 0) {
				int pos = ((TextBox)sender).SelectionStart;
				((TextBox)sender).Text = ((TextBox)sender).Text.Remove(pos - 1, 1);
				((TextBox)sender).SelectionStart = pos - 1;
			}
		}

		private void Submit_Button_Click(object sender, RoutedEventArgs e) {
			if(RecorderID_ComboBox.SelectedItem == null) {
				InvalidData_TextBlock.Text = "Please select a recorder id";
				InvalidData_StackPanel.Visibility = Visibility.Visible;
				Data_ScrollViewer.ScrollToVerticalOffset(0);
				return;
			} else if(Alliance_ComboBox.SelectedItem == null) {
				InvalidData_TextBlock.Text = "Please select an alliance";
				InvalidData_StackPanel.Visibility = Visibility.Visible;
				Data_ScrollViewer.ScrollToVerticalOffset(0);
				return;
			} else if(Event_ComboBox.SelectedItem == null) {
				InvalidData_TextBlock.Text = "Please enter an event";
				InvalidData_StackPanel.Visibility = Visibility.Visible;
				Data_ScrollViewer.ScrollToVerticalOffset(0);
				return;
			} else if(MatchNumber_TextBox.Text.Length == 0) {
				InvalidData_TextBlock.Text = "Please enter a match number";
				InvalidData_StackPanel.Visibility = Visibility.Visible;
				Data_ScrollViewer.ScrollToVerticalOffset(0);
				return;
			} else if(TeamNumber_TextBox.Text.Length == 0) {
				InvalidData_TextBlock.Text = "Please enter a team number";
				InvalidData_StackPanel.Visibility = Visibility.Visible;
				Data_ScrollViewer.ScrollToVerticalOffset(0);
				return;
			}

			App.FormDataCache.RecorderID = new DataElement<String>(RecorderID_ComboBox.SelectedItem.ToString());
			App.FormDataCache.Alliance = new DataElement<String>(Alliance_ComboBox.SelectedItem.ToString());
			App.FormDataCache.Event = new DataElement<String>(Event_ComboBox.SelectedItem.ToString());
			App.FormDataCache.MatchNumber = new DataElement<UInt16>(UInt16.Parse(MatchNumber_TextBox.Text));
			App.FormDataCache.TeamNumber = new DataElement<UInt16>(UInt16.Parse(TeamNumber_TextBox.Text));

			NavigationService.Navigate(new Prematch_Page());
		}
	}
}