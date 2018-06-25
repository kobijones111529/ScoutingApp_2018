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
		public Prematch_Page() {
			InitializeComponent();

			RecorderID_TextBlock.Text = string.Format(App.FormDataCache.RecorderID?.Value);
			Alliance_TextBlock.Text = string.Format(App.FormDataCache.Alliance?.Value);
			Event_TextBlock.Text = string.Format(App.FormDataCache.Event?.Value);
			MatchNumber_TextBlock.Text = string.Format("Match {0}", App.FormDataCache.MatchNumber?.Value.ToString());
			TeamNumber_TextBlock.Text = string.Format("Team {0}", App.FormDataCache.TeamNumber?.Value.ToString());
			PrematchNotes_TextBox.Text = App.FormDataCache.PrematchNotes?.Value;
		}

		public void Home_Button_Click(object sender, RoutedEventArgs e) {
			NavigationService.Navigate(new Home_Page());
		}

		private void Back_Button_Click(object sender, RoutedEventArgs e) {
			NavigationService.Navigate(new MatchInfo_Page());
		}

		private void StartMatch_Button_Click(object sender, RoutedEventArgs e) {
			App.FormDataCache.PrematchNotes = new DataElement<String>(PrematchNotes_TextBox.Text);
		}
	}
}
