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
			LastEvent_StackPanel.Children.Clear();

			if(!matchData.Any()) {
				Undo_Button.IsEnabled = false;

				TextBlock event_TextBlock = new TextBlock() {
					Foreground = new SolidColorBrush(Colors.White),
					TextAlignment = TextAlignment.Center,
					FontSize = 24,
					Text = "Match Started"
				};
				LastEvent_StackPanel.Children.Add(event_TextBlock);

				return;
			} else {
				Undo_Button.IsEnabled = true;
			}

			//Not null if event contains Time property
			ITimedMatchDataElement timedMatchDataElement = matchData[0] as ITimedMatchDataElement;

			TextBlock eventType_TextBlock = new TextBlock {
				Foreground = new SolidColorBrush(Colors.White),
				TextAlignment = TextAlignment.Center,
				FontSize = 24,
				Text = matchData[0].Type
			};
			TextBlock eventStage_TextBlock = new TextBlock {
				Foreground = new SolidColorBrush(Colors.White),
				TextAlignment = TextAlignment.Center,
				FontSize = 18,
				Text = matchData[0].Stage.ToString()
			};
			TextBlock eventTime_TextBlock = timedMatchDataElement == null ? null : new TextBlock {
				Foreground = new SolidColorBrush(Colors.White),
				TextAlignment = TextAlignment.Center,
				FontFamily = (FontFamily)FindResource("Lato Light"),
				FontSize = 18,
				Text = string.Format(@"{0:m\:ss}", new TimeSpan(0, 2, 30) - timedMatchDataElement.Time)
			};

			LastEvent_StackPanel.Children.Add(eventType_TextBlock);
			LastEvent_StackPanel.Children.Add(eventStage_TextBlock);
			if(eventTime_TextBlock != null) LastEvent_StackPanel.Children.Add(eventTime_TextBlock);
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