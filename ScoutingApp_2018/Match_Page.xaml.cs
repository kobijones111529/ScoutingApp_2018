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

		private DispatcherTimer AbortDispatcherTimer;

		private Stage Stage {
			get {
				return MatchStopwatch.Elapsed < new TimeSpan(0, 0, 15) ? Stage.Autonomous : Stage.Teleop;
			}
		}
		private bool Endgame {
			get {
				return MatchStopwatch.Elapsed < new TimeSpan(0, 2, 0) ? false : true;
			}
		}
		private TimeSpan MatchLength => new TimeSpan(0, 1, 3);
		private TimeSpan TimeRemaining(TimeSpan time) => MatchLength - time;
		private TimeSpan SecondsRemaining(TimeSpan time) => TimeSpan.FromSeconds(Math.Ceiling(TimeRemaining(time).TotalSeconds));

		//Displays last event for user
		private void DisplayLastEvent() {
			if(!matchData.Any()) {
				Undo_Button.IsEnabled = false;

				LastEventType_TextBlock.Visibility = Visibility.Visible;
				LastEventStage_TextBlock.Visibility = Visibility.Collapsed;
				LastEventTime_TextBlock.Visibility = Visibility.Collapsed;
				LastEventType_TextBlock.Text = "Match Started";
				LastEventStage_TextBlock.Text = string.Empty;
				LastEventTime_TextBlock.Text = string.Empty;

				return;
			}

			Undo_Button.IsEnabled = true;

			LastEventType_TextBlock.Visibility = Visibility.Visible;
			LastEventStage_TextBlock.Visibility = Visibility.Visible;
			LastEventType_TextBlock.Text = matchData[0].Type;
			LastEventStage_TextBlock.Text = matchData[0].Stage.ToString();

			ITimedMatchDataElement timedMatchDataElement = matchData[0] as ITimedMatchDataElement;
			if(timedMatchDataElement == null) {
				LastEventTime_TextBlock.Visibility = Visibility.Collapsed;
				LastEventTime_TextBlock.Text = string.Empty;
			} else {
				LastEventTime_TextBlock.Visibility = Visibility.Visible;
				LastEventTime_TextBlock.Text = string.Format(@"{0:m\:ss}", SecondsRemaining(timedMatchDataElement.Time));
			}

			int repeatCount = 0;
			for(int i = 0; matchData[i].GetType() == matchData[0].GetType(); i++) {
				repeatCount = i;
				if(matchData.Count <= i + 1)
					break;
			}
			if(repeatCount > 0) {
				LastEventRepeatCount_TextBlock.Visibility = Visibility.Visible;
				LastEventRepeatCount_TextBlock.Text = string.Format("({0})", repeatCount + 1);
			} else {
				LastEventRepeatCount_TextBlock.Visibility = Visibility.Collapsed;
				LastEventRepeatCount_TextBlock.Text = string.Empty;
			}
		}

		private void MatchDispatcherTimer_Tick(object sender, EventArgs e) {
			if(TimeRemaining(MatchStopwatch.Elapsed) < TimeSpan.Zero) {
				MatchDispatcherTimer.Stop();
				MatchStopwatch.Stop();
				Abort_Continue_Button.Click -= Abort_Button_Click;
				Abort_Continue_Button.Click += Continue_Button_Click;
				Abort_Continue_Button.Content = "Continue";
				Timer_TextBlock.Text = "0:00";
				Timer_TextBlock.Foreground = (SolidColorBrush)FindResource("AccentColor1");
			} else {
				Timer_TextBlock.Text = string.Format(@"{0:m\:ss}", SecondsRemaining(MatchStopwatch.Elapsed));
			}
		}

		public Match_Page() {
			MatchDispatcherTimer = new DispatcherTimer();
			MatchDispatcherTimer.Tick += MatchDispatcherTimer_Tick;
			MatchDispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
			MatchStopwatch = new Stopwatch();
			MatchStopwatch.Start();
			MatchDispatcherTimer.Start();

			AbortDispatcherTimer = new DispatcherTimer() {
				Interval = new TimeSpan(0, 0, 3)
			};
			AbortDispatcherTimer.Tick += delegate {
				AbortDispatcherTimer.Stop();
				MatchDispatcherTimer.Stop();
				NavigationService.Navigate(new Prematch_Page());
			};

			InitializeComponent();

			RecorderID_TextBlock.Text = string.Format(App.MatchInfo_Cache.RecorderID.Value);
			Alliance_TextBlock.Text = string.Format(App.MatchInfo_Cache.Alliance.Value);
			Event_TextBlock.Text = string.Format(App.MatchInfo_Cache.Event.Value);
			MatchNumber_TextBlock.Text = string.Format("Match {0}", App.MatchInfo_Cache.MatchNumber.Value);
			TeamNumber_TextBlock.Text = string.Format("Team {0}", App.MatchInfo_Cache.TeamNumber.Value);

			DisplayLastEvent();
		}

		private void Abort_Button_PreviewMouseDown(object sender, MouseEventArgs e) {
			AbortDispatcherTimer.Start();
		}

		private void Abort_Button_PreviewMouseUp(object sender, MouseEventArgs e) {
			AbortDispatcherTimer.Stop();
		}

		private void Abort_Button_PreviewStylusDown(object sender, StylusEventArgs e) {
			AbortDispatcherTimer.Start();
		}

		private void Abort_Button_PreviewStylusUp(object sender, StylusEventArgs e) {
			AbortDispatcherTimer.Stop();
		}

		private void Abort_Button_PreviewTouchDown(object sender, TouchEventArgs e) {
			AbortDispatcherTimer.Start();
		}

		private void Abort_Button_PreviewTouchUp(object sender, TouchEventArgs e) {
			AbortDispatcherTimer.Stop();
		}

		private void Continue_Button_Click(object sender, RoutedEventArgs e) {
			App.MatchData_Cache = matchData;

			NavigationService.Navigate(new Postmatch_Page());
		}

		private void Undo_Button_Click(object sender, RoutedEventArgs e) {
			if(matchData.Any()) {
				matchData.RemoveAt(0);
				DisplayLastEvent();
			}
		}

		private void CrossBaseline_Button_Click(object sender, RoutedEventArgs e) {
			matchData.Insert(0, new CrossBaseline() {
				Time = MatchStopwatch.Elapsed
			});
			DisplayLastEvent();
		}

		private void CubeFromFloor_Button_Click(object sender, RoutedEventArgs e) {
			matchData.Insert(0, new CubeFromFloor() {
				Stage = Stage,
				Time = MatchStopwatch.Elapsed
			});
			DisplayLastEvent();
		}
	}
}