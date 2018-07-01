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

namespace ScoutingApp_2018 {
	public partial class Prematch_Page : Page {
		private void CacheInfo() {
			App.MatchInfo_Cache.PrematchNotes = !PrematchNotes_TextBox.Text.Any() ? null : new MatchInfoElement<String> { Value = PrematchNotes_TextBox.Text };
		}

		public Prematch_Page() {
			InitializeComponent();

			//Summarize info for user
			RecorderID_TextBlock.Text = string.Format(App.MatchInfo_Cache.RecorderID.Value);
			Alliance_TextBlock.Text = string.Format(App.MatchInfo_Cache.Alliance.Value);
			Event_TextBlock.Text = string.Format(App.MatchInfo_Cache.Event.Value);
			MatchNumber_TextBlock.Text = string.Format("Match {0}", App.MatchInfo_Cache.MatchNumber.Value);
			TeamNumber_TextBlock.Text = string.Format("Team {0}", App.MatchInfo_Cache.TeamNumber.Value);
			PrematchNotes_TextBox.Text = App.MatchInfo_Cache.PrematchNotes?.Value;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			App.Prematch_Page_Cache = null;
		}

		//Navigate to home
		public void Home_Button_Click(object sender, RoutedEventArgs e) {
			App.Prematch_Page_Cache = this;

			NavigationService.Navigate(new Home_Page());
		}

		//Navigate back to match info page
		private void Back_Button_Click(object sender, RoutedEventArgs e) {
			CacheInfo();

			NavigationService.Navigate(new MatchInfo_Page());
		}

		//Cache data and navigate to match page
		private void StartMatch_Button_Click(object sender, RoutedEventArgs e) {
			App.MatchInfo_Cache.PrematchNotes = !PrematchNotes_TextBox.Text.Any() ? null : new MatchInfoElement<String>() { Value = PrematchNotes_TextBox.Text };

			NavigationService.Navigate(new Match_Page());
		}
	}
}
