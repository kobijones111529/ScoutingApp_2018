﻿using System;
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

namespace ScoutingApp_2018.Views {
	public partial class KeyValueView : TextBlock {
		public String Key {
			get {
				return (String)GetValue(KeyProperty);
			}
			set {
				SetValue(KeyProperty, value);
			}
		}

		public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(String), typeof(KeyValueView), new PropertyMetadata());

		public KeyValueView() {
			InitializeComponent();
		}
	}
}
