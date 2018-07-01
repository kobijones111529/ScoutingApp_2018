using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using System.IO;
using Path = System.IO.Path;

namespace ScoutingApp_2018 {
	public enum Stage {
		Autonomous, Teleop, Endgame
	}

	//All data to be stored locally in json format
	public class Data {
		public DateTime DateTime;
		public String RecorderID;
		public String AllianceColor;
		public Byte AlliancePosition;
		public String EventName;
		public UInt16 TeamNumber;
		public UInt16 MatchNumber;
		public String PrematchNotes;
		public String PostmatchNotes;
		public Boolean AutonomousCrossBaseline;
	}

	//Base classes for match data types
	public interface IMatchDataElement {
		String Type { get; }
		Stage Stage { get; }
	}

	public interface ITimedMatchDataElement : IMatchDataElement {
		TimeSpan Time { get; set; }
	}

	//Match data types
	public class AutonomousCrossBaseline : ITimedMatchDataElement {
		private String _type = "Cross Baseline";
		private Stage _stage = Stage.Autonomous;
		private TimeSpan _time;

		public String Type {
			get {
				return _type;
			}
		}
		public Stage Stage {
			get {
				return _stage;
			}
		}
		public TimeSpan Time {
			get {
				return _time;
			}
			set {
				_time = value;
			}
		}
	}

	//Match data collected during the match
	public class MatchData : List<IMatchDataElement> { }

	//Wrapper class, allows MatchInfo elements to be nullable
	public class MatchInfoElement<T> {
		public T Value;
	}

	//Match info entered prior to or after the match
	public class MatchInfo {
		public MatchInfoElement<String> RecorderID;
		public MatchInfoElement<String> Alliance;
		public MatchInfoElement<String> Event;
		public MatchInfoElement<UInt16> MatchNumber;
		public MatchInfoElement<UInt16> TeamNumber;
		public MatchInfoElement<String> PrematchNotes;
		public MatchInfoElement<String> PostmatchNotes;
	}
	
	public partial class App : Application {
		//Location of scouting data within non-roaming appdata
		public static string AppDataLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ScoutingApp_2018");
		public static string ScoutingDataLocation = Path.Combine(AppDataLocation, @"Data");

		//Cache scouting data, easier than passing it between pages until needed
		public static MatchInfo_Page MatchInfo_Page_Cache;
		public static Prematch_Page Prematch_Page_Cache;
		public static MatchInfo MatchInfo_Cache;
		public static MatchData MatchData_Cache;
	}
}
