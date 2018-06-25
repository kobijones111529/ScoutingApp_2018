using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ScoutingApp_2018 {
	public class DataElement<T> {
		public T Value { get; set; }

		public DataElement(T value) {
			Value = value;
		}
	}

	public class Data {
		public UInt16 year;
		public String recorderID;
		public String alliance;
		public String Event;
		public UInt16 teamNumber;
		public UInt16 matchNumber;
	}

	public class FormData {
		public DataElement<String> RecorderID;
		public DataElement<String> Alliance;
		public DataElement<String> Event;
		public DataElement<UInt16> MatchNumber;
		public DataElement<UInt16> TeamNumber;
		public DataElement<String> PrematchNotes;
	}

	public partial class App : Application {
		public static FormData FormDataCache = new FormData();
	}
}
