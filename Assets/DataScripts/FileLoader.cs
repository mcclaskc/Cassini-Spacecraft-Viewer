using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class FileLoader : MonoBehaviour, DataAccess {

	//This is an active loader, and just loads up all the files at the beginning
	Dictionary<string, SortedList<DateTime, EphemerisData>> bodies = new Dictionary<string, SortedList<DateTime, EphemerisData>>();

	public void Start() {
		//Load up the time data
		List<DateTime> times = new List<DateTime>();
		string filename = "Assets/Data/timeData.dat";
		char[] utcChar = new char[17];
		if(File.Exists(filename)) {
			Debug.Log("Loading Time Data");
			BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open));
			bool finished = false;
			while(! finished) {
				try {
					string utc = "";
					utcChar = binReader.ReadChars(17);
					for(int index = 0; index < 17; index++) {
						utc += utcChar[index];
					}
					double ephemTime = binReader.ReadDouble();
					times.Add(DateTime.Parse(utc));

				} catch (Exception e) {
					finished = true;
				}
			}
			binReader.Close();
		}

		//Now load up the ephemera data
		filename = "Assets/Data/titanEphem_saturn.dat";
		if(File.Exists(filename)) {
			List<Vector3> posList = new List<Vector3>();
			Debug.Log("Loading Titan ephemera");
			BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open));
			bool finished = false;

			while(! finished) {
				try{
					double x = binReader.ReadDouble();
					double y = binReader.ReadDouble();
					double z = binReader.ReadDouble();
					posList.Add(new Vector3((float) x, (float)y, (float)z));
				} catch (Exception e) {
					finished = true;
				}
			}

			//Match up the positions and times
			SortedList<DateTime, EphemerisData> titanData = new SortedList<DateTime, EphemerisData>();
			for(int i = 0; i < posList.Count && i < times.Count; i++) {
				titanData.Add(times[i], new EphemerisData(posList[i], times[i]));
			}

			bodies["titan"] = titanData;
		}
	}


	//DataAccess stuff
	//-----------------------------
	public List<SensorDataAvailability> AvailableDataBlocks(DateTime start, DateTime end) {
		return new List<SensorDataAvailability>();
	}

	public void RequestTimeRange(DateTime start, DateTime end, string sensorName) {

	}

	public bool TimeRangeAvailable(DateTime start, DateTime end, string sensorName) {
		return false;
	}

	public List<DataChunk> GetData(DateTime start, DateTime end, string body) {
		return null;
	}

	public void RequestEphemeris(DateTime start, DateTime end, string body) {
	}

	public bool EphemerisAvailable(DateTime start, DateTime end, string body) {
		return true;
	}

	public List<EphemerisData> GetEphemeris(DateTime start, DateTime end, string bodyName) {
		List<EphemerisData> accumulator = new List<EphemerisData>();
		if(bodies.ContainsKey(bodyName)) {
			SortedList<DateTime, EphemerisData> bodyData = bodies[bodyName];
			//Go through and get the data
			foreach(KeyValuePair<DateTime, EphemerisData> dat in bodyData) {
				if(dat.Key >= start && dat.Key <= end) {
					accumulator.Add(dat.Value);
				}
			}
		}

		return accumulator;
	}
	//------------------------------
}
