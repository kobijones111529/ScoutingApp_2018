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

namespace ScoutingApp_2018 {
	public partial class Match_Page : Page {
		Stopwatch MatchTimer = new Stopwatch();

		public Match_Page() {
			InitializeComponent();

			RecorderID_TextBlock.Text = string.Format(App.FormDataCache.RecorderID.Value);
			Alliance_TextBlock.Text = string.Format(App.FormDataCache.Alliance.Value);
			Event_TextBlock.Text = string.Format(App.FormDataCache.Event.Value);
			MatchNumber_TextBlock.Text = string.Format("Match {0}", App.FormDataCache.MatchNumber.Value);
			TeamNumber_TextBlock.Text = string.Format("Team {0}", App.FormDataCache.TeamNumber.Value);

			MatchTimer.Start();
		}
	}
}
