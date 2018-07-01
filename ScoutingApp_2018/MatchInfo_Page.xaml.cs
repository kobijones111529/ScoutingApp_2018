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

using ScoutingApp_2018.Views;

namespace ScoutingApp_2018 {
	public partial class MatchInfo_Page : Page {
		//Cache entered info
		private void CacheInfo() {
			if(App.MatchInfo_Cache == null)
				App.MatchInfo_Cache = new MatchInfo();

			App.MatchInfo_Cache.RecorderID = RecorderID_ComboBox.SelectedItem == null ? null : new MatchInfoElement<String>() { Value = ((TextBlock)RecorderID_ComboBox.SelectedItem).Text };
			App.MatchInfo_Cache.Alliance = Alliance_ComboBox.SelectedItem == null ? null : new MatchInfoElement<String>() { Value = ((TextBlock)Alliance_ComboBox.SelectedItem).Text };
			App.MatchInfo_Cache.Event = Event_ComboBox.SelectedItem == null && !Event_ComboBox.Text.Any() ? null : new MatchInfoElement<String>() { Value = Event_ComboBox.SelectedItem == null ? Event_ComboBox.Text : ((TextBlock)Event_ComboBox.SelectedItem).Text };
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
			string[] recorderID_array = ((JArray)recorderIDs_jObject.GetValue("recorderID_array")).ToObject<string[]>();

			//Set ComboBox items
			foreach(string recorderID in recorderID_array) {
				RecorderID_ComboBox.Items.Add(new TextBlock() {
					FontFamily = (FontFamily)FindResource("Lato Light"),
					FontSize = 24,
					Text = recorderID
				});
			}
			for(int i = 0; i < 6; i++) {
				Alliance_ComboBox.Items.Add(new TextBlock() {
					FontFamily = (FontFamily)FindResource("Lato Light"),
					FontSize = 24,
					Text = string.Format("{0} {1}", i < 3 ? "Blue" : "Red", i % 3 + 1)
				});
			}
			Event_ComboBox.Items.Add(new TextBlock() {
				FontFamily = (FontFamily)FindResource("Lato Light"),
				FontSize = 24,
				Text = "Practice"
			});
			
			//Populate controls from cached info
			if(App.MatchInfo_Cache != null) {
				RecorderID_ComboBox.SelectedIndex = App.MatchInfo_Cache.RecorderID == null ? -1 : RecorderID_ComboBox.Items.Cast<TextBlock>().ToList().FindIndex(recorderID_TextBlock => recorderID_TextBlock.Text == App.MatchInfo_Cache.RecorderID.Value);
				Alliance_ComboBox.SelectedIndex = App.MatchInfo_Cache.Alliance == null ? -1 : Alliance_ComboBox.Items.Cast<TextBlock>().ToList().FindIndex(alliance_TextBlock => alliance_TextBlock.Text == App.MatchInfo_Cache.Alliance.Value);
				Event_ComboBox.SelectedIndex = App.MatchInfo_Cache.Event == null ? -1 : Event_ComboBox.Items.Cast<TextBlock>().ToList().FindIndex(event_TextBlock => event_TextBlock.Text == App.MatchInfo_Cache.Event.Value);
				Event_ComboBox.Text = App.MatchInfo_Cache.Event?.Value;
				MatchNumber_TextBox.Text = App.MatchInfo_Cache.MatchNumber?.Value.ToString();
				TeamNumber_TextBox.Text = App.MatchInfo_Cache.TeamNumber?.Value.ToString();
			} else {
				RecorderID_ComboBox.SelectedItem = RecorderID_ComboBox.Items[0];
				Alliance_ComboBox.SelectedItem = Alliance_ComboBox.Items[0];
				Event_ComboBox.SelectedItem = Event_ComboBox.Items[0];
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

		//Restrict MatchNumber_TextBox to UInt16 parsible and otherwise aesthetic text
		private void MatchNumber_TextBox_TextChanged(object sender, TextChangedEventArgs e) {
			bool isUInt16 = UInt16.TryParse(((TextBox)sender).Text, out UInt16 outUInt16);
			if((!isUInt16 || ((TextBox)sender).Text.Contains(' ') || ((TextBox)sender).Text.StartsWith("0")) && ((TextBox)sender).Text.Length > 0) {
				int pos = ((TextBox)sender).SelectionStart;
				((TextBox)sender).Text = ((TextBox)sender).Text.Remove(pos - 1, 1);
				((TextBox)sender).SelectionStart = pos - 1;
			}
		}

		//UInt16 parsible
		private void TeamNumber_TextBox_TextChanged(object sender, TextChangedEventArgs e) {
			bool isUInt16 = UInt16.TryParse(((TextBox)sender).Text, out UInt16 outUInt16);
			if((!isUInt16 || ((TextBox)sender).Text.Contains(' ') || ((TextBox)sender).Text.StartsWith("0")) && ((TextBox)sender).Text.Length > 0) {
				int pos = ((TextBox)sender).SelectionStart;
				((TextBox)sender).Text = ((TextBox)sender).Text.Remove(pos - 1, 1);
				((TextBox)sender).SelectionStart = pos - 1;
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
			} else if(Event_ComboBox.SelectedItem == null && !Event_ComboBox.Text.Any()) {
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