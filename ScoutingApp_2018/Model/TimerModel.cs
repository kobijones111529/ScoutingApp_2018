using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Threading;
using System.Diagnostics;

namespace ScoutingApp_2018.Model
{
	public class TimerModel : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		public string ElapsedMinutes {
			get {
				return elapsedMinutes;
			}
		}
		public string ElapsedSeconds {
			get {
				return elapsedSeconds;
			}
		}
		public string ElapsedMilliseconds {
			get {
				return elapsedMilliseconds;
			}
		}

		private TimeSpan elapsed;
		private string elapsedMinutes;
		private string elapsedSeconds;
		private string elapsedMilliseconds;
		private DispatcherTimer timer;
		private Stopwatch stopwatch;

		public void StartTimer() {
			timer = new DispatcherTimer();
			timer.Tick += DispatcherTimer_Tick;
			timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
			stopwatch = new Stopwatch();
			stopwatch.Start();
			timer.Start();
		}

		private void DispatcherTimer_Tick(object sender, EventArgs e) {
			elapsed = stopwatch.Elapsed;
			elapsedMinutes = ((uint)elapsed.TotalMinutes).ToString("0");
			elapsedSeconds = ((uint)elapsed.TotalSeconds % 60).ToString("00");
			elapsedMilliseconds = ((int)(elapsed.TotalMilliseconds / 100) % 10).ToString("0");
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("elapsed"));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("elapsedMinutes"));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("elapsedSeconds"));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("elapsedMilliseconds"));
		}
	}
}