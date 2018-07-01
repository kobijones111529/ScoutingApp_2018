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

using System.Diagnostics;
using System.Windows.Threading;

namespace ScoutingApp_2018 {
	public partial class Match_Page : Page {
		//Match events are inserted at index 0
		private MatchData matchData = new MatchData();

		private DispatcherTimer MatchDispatcherTimer;
		private Stopwatch MatchStopwatch;

		public TimeSpan MatchLength {
			get {
				return new TimeSpan(0, 0, 3);
			}
		}
		public TimeSpan TimeRemaining {
			get {
				return MatchLength - MatchStopwatch.Elapsed;
			}
		}

		//Displays last event for user
		private void UpdateLastEvent() {
			if(!matchData.Any()) {
				Undo_Button.IsEnabled = false;

				LastEventType_TextBlock.Text = "Match Started";
				LastEventStage_TextBlock.Visibility = Visibility.Collapsed;
				LastEventTime_TextBlock.Visibility = Visibility.Collapsed;

				return;
			} else {
				Undo_Button.IsEnabled = true;
				
				LastEventStage_TextBlock.Visibility = Visibility.Visible;
				LastEventTime_TextBlock.Visibility = Visibility.Visible;
			}

			LastEventType_TextBlock.Text = matchData[0].Type;
			LastEventStage_TextBlock.Text = matchData[0].Stage.ToString();

			ITimedMatchDataElement timedMatchDataElement = matchData[0] as ITimedMatchDataElement;
			if(timedMatchDataElement == null) {
				LastEventTime_TextBlock.Visibility = Visibility.Collapsed;
			} else {
				LastEventTime_TextBlock.Text = string.Format("{0:m\\:ss}", MatchLength - timedMatchDataElement.Time);
			}
		}
		
		public Match_Page() {
			InitializeComponent();

			RecorderID_TextBlock.Text = string.Format(App.MatchInfo_Cache.RecorderID.Value);
			Alliance_TextBlock.Text = string.Format(App.MatchInfo_Cache.Alliance.Value);
			Event_TextBlock.Text = string.Format(App.MatchInfo_Cache.Event.Value);
			MatchNumber_TextBlock.Text = string.Format("Match {0}", App.MatchInfo_Cache.MatchNumber.Value);
			TeamNumber_TextBlock.Text = string.Format("Team {0}", App.MatchInfo_Cache.TeamNumber.Value);

			UpdateLastEvent();

			MatchDispatcherTimer = new DispatcherTimer();
			MatchDispatcherTimer.Tick += MatchDispatcherTimer_Tick;
			MatchDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
			MatchStopwatch = new Stopwatch();
			MatchStopwatch.Start();
			MatchDispatcherTimer.Start();
		}

		private void MatchDispatcherTimer_Tick(object sender, EventArgs e) {
			if(TimeRemaining < TimeSpan.Zero) {
				MatchDispatcherTimer.Stop();
				MatchStopwatch.Stop();
				Abort_Button.Click -= Abort_Button_Click;
				Abort_Button.Click += Abort_Button_Click_Continue;
				Abort_Button.Content = "Continue";
				Timer_TextBlock.Text = "0:00";
				Timer_TextBlock.Foreground = new SolidColorBrush(Color.FromRgb(170, 57, 57));
			} else {
				int minutes = (int)TimeRemaining.TotalMinutes;
				int seconds = (int)TimeRemaining.TotalSeconds % 60;
				Timer_TextBlock.Text = string.Format("{0:0}:{1:00}", minutes, seconds);
			}
		}

		private void Abort_Button_Click(object sender, RoutedEventArgs e) {
			MatchDispatcherTimer.Stop();
			NavigationService.Navigate(new Prematch_Page());
		}

		private void Abort_Button_Click_Continue(object sender, RoutedEventArgs e) {
			App.MatchData_Cache = matchData;

			NavigationService.Navigate(new Postmatch_Page());
		}

		private void Undo_Button_Click(object sender, RoutedEventArgs e) {
			matchData.RemoveAt(0);
			UpdateLastEvent();
		}

		private void CrossBaseline_Button_Click(object sender, RoutedEventArgs e) {
			matchData.Insert(0, new AutonomousCrossBaseline() {
				Time = MatchStopwatch.Elapsed
			});
			UpdateLastEvent();
		}
	}
}