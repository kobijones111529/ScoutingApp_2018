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
	public partial class FileViewItem : UserControl {
		public String Text {
			get { return (String)this.GetValue(TextProperty); }
			set { this.SetValue(TextProperty, value); }
		}
		public String File {
			get { return (String)this.GetValue(FileProperty); }
			set { this.SetValue(FileProperty, value); }
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(FileViewItem), new PropertyMetadata(String.Empty));
		public static readonly DependencyProperty FileProperty = DependencyProperty.Register("File", typeof(String), typeof(FileViewItem), new PropertyMetadata(String.Empty));

		public FileViewItem() {
			InitializeComponent();
		}
	}
}
