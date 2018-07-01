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
using System.Collections.ObjectModel;
using System.Globalization;

namespace ScoutingApp_2018 {
	public partial class MatchInfo_Page : Page {
		//Cache entered info
		private void CacheInfo() {
			if(App.MatchInfo_Cache == null)
				App.MatchInfo_Cache = new MatchInfo();
			
			App.MatchInfo_Cache.RecorderID = RecorderID_ComboBox.SelectedItem == null ? null : new MatchInfoElement<String>() { Value = RecorderID_ComboBox.SelectedItem.ToString() };
			App.MatchInfo_Cache.Alliance = Alliance_ComboBox.SelectedItem == null ? null : new MatchInfoElement<String>() { Value = Alliance_ComboBox.SelectedItem.ToString() };
			App.MatchInfo_Cache.Event = !Event_ComboBox.Text.Any() ? null : new MatchInfoElement<String>() { Value = Event_ComboBox.Text };
			App.MatchInfo_Cache.MatchNumber = !MatchNumber_TextBox.Text.Any() ? null : new MatchInfoElement<UInt16>() { Value = UInt16.Parse(MatchNumber_TextBox.Text) };
			App.MatchInfo_Cache.TeamNumber = !TeamNumber_TextBox.Text.Any() ? null : new MatchInfoElement<UInt16>() { Value = UInt16.Parse(TeamNumber_TextBox.Text) };
		}

		public MatchInfo_Page() {
			InitializeComponent();

			//Retrieve recorder ids from json file
			Uri recorderIDs_json_uri = new Uri("/RecorderIDs.json", UriKind.Relative);
			StreamResourceInfo streamResourceInfo = Application.GetResourceStream(recorderIDs_json_uri);
			StreamReader streamReader = new StreamReader(streamResourceInfo.Stream);
			string recorderIDs_json = streamReader.ReadToEnd();
			JObject recorderIDs_jObject = JObject.Parse(recorderIDs_json);

			//Set ComboBox items
			List<string> recorderID_List = ((JArray)recorderIDs_jObject.GetValue("recorderID_array")).ToObject<List<String>>();
			List<string> alliance_List = new List<string>() { "Blue 1", "Blue 2", "Blue 3", "Red 1", "Red 2", "Red 3" };
			List<string> eventCode_List = new List<string>() { "Practice" };

			recorderID_List.ForEach(recorderID => RecorderID_ComboBox.Items.Add(recorderID));
			alliance_List.ForEach(alliance => Alliance_ComboBox.Items.Add(alliance));
			eventCode_List.ForEach(eventCode => Event_ComboBox.Items.Add(eventCode));
			
			//Populate controls from cached info
			if(App.MatchInfo_Cache != null) {
				RecorderID_ComboBox.SelectedItem = App.MatchInfo_Cache.RecorderID?.Value;
				Alliance_ComboBox.SelectedItem = App.MatchInfo_Cache.Alliance?.Value;
				Event_ComboBox.Text = App.MatchInfo_Cache.Event?.Value;
				MatchNumber_TextBox.Text = App.MatchInfo_Cache.MatchNumber?.Value.ToString();
				TeamNumber_TextBox.Text = App.MatchInfo_Cache.TeamNumber?.Value.ToString();
			} else {
				RecorderID_ComboBox.SelectedIndex = 0;
				Alliance_ComboBox.SelectedIndex = 0;
				Event_ComboBox.SelectedIndex = 0;
				MatchNumber_TextBox.Text = "1";
				TeamNumber_TextBox.Text = "2512";
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			App.MatchInfo_Page_Cache = null;
		}

		//Navigate to home
		public void Home_Button_Click(object sender, RoutedEventArgs e) {
			App.MatchInfo_Page_Cache = this;
			CacheInfo();

			NavigationService.Navigate(new Home_Page());
		}

		//Restrict TextBox to UInt16 parsible and otherwise aesthetic text
		private void UInt16_TextBox_TextChanged(object sender, TextChangedEventArgs e) {
			TextBox sender_TextBox = sender as TextBox;
			if((sender_TextBox.Text.Any(c => !Char.IsDigit(c)) || sender_TextBox.Text.Length > 5) && sender_TextBox.Text.Any()) {
				int pos = sender_TextBox.SelectionStart;
				pos = pos > 0 ? pos : 1;
				sender_TextBox.Text = sender_TextBox.Text.Remove(pos - 1, 1);
				sender_TextBox.SelectionStart = pos - 1;
			}
		}

		//Cache info if valid (otherwise alerts user and returns), navigates to prematch info page
		private void Submit_Button_Click(object sender, RoutedEventArgs e) {
			bool valid = true;
			string prompt = string.Empty;

			if(RecorderID_ComboBox.SelectedItem == null) {
				valid = false;
				prompt = "Please select a recorder id";
			} else if(Alliance_ComboBox.SelectedItem == null) {
				valid = false;
				prompt = "Please select an alliance";
			} else if(!Event_ComboBox.Text.Any()) {
				valid = false;
				prompt = "Please enter an event";
			} else if(MatchNumber_TextBox.Text.Length == 0) {
				valid = false;
				prompt = "Please enter a match number";
			} else if(TeamNumber_TextBox.Text.Length == 0) {
				valid = false;
				prompt = "Please enter a team number";
			}

			if(!valid) {
				InvalidData_TextBlock.Text = prompt;
				InvalidData_StackPanel.Visibility = Visibility.Visible;
				Data_ScrollViewer.ScrollToVerticalOffset(0);
				return;
			}

			CacheInfo();
			NavigationService.Navigate(new Prematch_Page());
		}
	}
}