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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScoutingApp_2018 {
	public partial class Postmatch_Page : Page {
		private MatchData matchData;

		public Postmatch_Page() {
			InitializeComponent();

			//Store match data in local variable and clear cache
			matchData = App.MatchData_Cache;
			App.MatchData_Cache = null;
		}

		private async void SaveData_Button_Click(object sender, RoutedEventArgs e) {
			App.MatchInfo_Cache.PostmatchNotes = !PostmatchNotes_TextBox.Text.Any() ? null : new MatchInfoElement<String>() { Value = PostmatchNotes_TextBox.Text };

			//Copy data to single object
			DateTime now = DateTime.Now;
			string[] alliance = App.MatchInfo_Cache.Alliance.Value.Split(' ');
			String allianceColor = alliance[0];
			Byte alliancePosition = Byte.Parse(alliance[1]);

			Data data = new Data() {
				DateTime = now,
				RecorderID = App.MatchInfo_Cache.RecorderID.Value,
				EventName = App.MatchInfo_Cache.Event.Value,
				AllianceColor = allianceColor,
				AlliancePosition = alliancePosition,
				TeamNumber = App.MatchInfo_Cache.TeamNumber.Value,
				MatchNumber = App.MatchInfo_Cache.MatchNumber.Value,
				PrematchNotes = App.MatchInfo_Cache.PrematchNotes?.Value,
				PostmatchNotes = App.MatchInfo_Cache.PostmatchNotes?.Value
			};

			//Loop throught match events
			while(matchData.Count > 0) {
				if(matchData[0].GetType() == typeof(CrossBaseline)) {
					data.AutonomousCrossBaseline = true;
				}

				matchData.RemoveAt(0);
			}

			//Convert data object to json string
			string data_json = JsonConvert.SerializeObject(data, Formatting.Indented);

			//Non-roaming scouting data folder
			if(!Directory.Exists(App.ScoutingDataLocation)) Directory.CreateDirectory(App.ScoutingDataLocation);
			string dataFile = Path.Combine(App.ScoutingDataLocation, string.Format("Match{0}-{1}{2}-{3:yyyy-MM-ddTHH-mm-ss-fffffff}.json", data.MatchNumber, data.AllianceColor, data.AlliancePosition, data.DateTime));
			using(StreamWriter writer = new StreamWriter(dataFile)) {
				await writer.WriteAsync(data_json);
			}

			App.MatchInfo_Cache.MatchNumber.Value++;
			App.MatchInfo_Cache.TeamNumber = null;
			App.MatchInfo_Cache.PrematchNotes = null;

			NavigationService.Navigate(new MatchInfo_Page());
		}

		private void DiscardData_Button_Click(object sender, RoutedEventArgs e) {
			App.MatchInfo_Cache.MatchNumber.Value++;
			App.MatchInfo_Cache.TeamNumber = null;

			NavigationService.Navigate(new MatchInfo_Page());
		}
	}
}
