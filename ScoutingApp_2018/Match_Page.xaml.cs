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
		public ViewModel.TimerViewModel TimerViewModel;

		private DispatcherTimer MatchDispatcherTimer;
		private Stopwatch MatchStopwatch;

		public Match_Page() {
			InitializeComponent();

			MatchDispatcherTimer = new DispatcherTimer();
			MatchDispatcherTimer.Tick += MatchDispatcherTimer_Tick;
			MatchDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
			MatchStopwatch = new Stopwatch();
			MatchStopwatch.Start();
			MatchDispatcherTimer.Start();

			RecorderID_TextBlock.Text = string.Format(App.FormDataCache.RecorderID.Value);
			Alliance_TextBlock.Text = string.Format(App.FormDataCache.Alliance.Value);
			Event_TextBlock.Text = string.Format(App.FormDataCache.Event.Value);
			MatchNumber_TextBlock.Text = string.Format("Match {0}", App.FormDataCache.MatchNumber.Value);
			TeamNumber_TextBlock.Text = string.Format("Team {0}", App.FormDataCache.TeamNumber.Value);
		}

		private void MatchDispatcherTimer_Tick(object sender, EventArgs e) {
			TimeSpan timeRemaining = new TimeSpan(0, 2, 30) - MatchStopwatch.Elapsed;
			TimerMinutes_TextBlock.Text = ((int)timeRemaining.TotalMinutes).ToString("0");
			TimerSeconds_TextBlock.Text = ((int)timeRemaining.TotalSeconds % 60).ToString("00");
		}

		private void Abort_Button_Click(object sender, RoutedEventArgs e) {
			NavigationService.Navigate(new Prematch_Page());
		}
	}
}