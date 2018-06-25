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
	public partial class Home_Page : Page {
		public Home_Page() {
			InitializeComponent();
		}

		private void Scouting_Button_Click(object sender, RoutedEventArgs e) {
			NavigationService.Navigate(new MatchInfo_Page());
		}
	}
}
