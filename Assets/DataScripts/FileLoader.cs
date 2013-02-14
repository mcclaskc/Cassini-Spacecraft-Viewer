using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class FileLoader : MonoBehaviour, DataAccess {

	//This is an active loader, and just loads up all the files at the beginning
	Dictionary<string, SortedList<DateTime, EphemerisData>> bodies = new Dictionary<string, SortedList<DateTime, EphemerisData>>();
	GameObject[] Movers;
	public void Start() {
		//Finds all gameobjects tagged 'Mover'
		Movers = GameObject.FindGameObjectsWithTag("Mobile");
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
					//Debug.Log (DateTime.Parse(utc));
					times.Add(DateTime.Parse(utc));

				} catch (Exception e) {
					finished = true;
				}
			}
			
			binReader.Close();
		}

		//Now load up the ephemera data
		//Titan
		filename = "Assets/Data/titanEphem_saturn.dat";
		if(File.Exists(filename)) {
			List<Vector3> posList = new List<Vector3>();
			Debug.Log("Loading Titan ephemera");
			BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open));
			bool finished = false;

			while(! finished) {
				try{
					double x = binReader.ReadDouble();
					double z = binReader.ReadDouble();
					double y = binReader.ReadDouble();
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

			bodies["Titan"] = titanData;
		}
//--------------------------------------------------------------------------------------------------
		//Cassini
		//File where binary data exists
		filename = "Assets/Data/cassiniEphem_saturn.dat";
		if(File.Exists (filename)){
			//List to store the positions read from file
			List<Vector3> posList = new List<Vector3>();
			//Sanity check
			Debug.Log ("Loading Cassini ephemera");
			BinaryReader binReader = new BinaryReader(File.Open (filename, FileMode.Open));
			bool finished = false;
			//These are placeholders, cassini file comes with velocity data as well as position
			double velx, vely, velz;
			//Loop that goes until file is loaded
			while(!finished) {
				try{
					//positions
					double x = binReader.ReadDouble();
					double z = binReader.ReadDouble();
					double y = binReader.ReadDouble();
					//velocities
					velx = binReader.ReadDouble();
					velz = binReader.ReadDouble();
					vely = binReader.ReadDouble();
					posList.Add(new Vector3((float) x, (float) y, (float)z));
					
				}catch(Exception e){
				  	finished = true;
				}
			}
			//Matching up times
			SortedList<DateTime, EphemerisData> cassiniData = new SortedList<DateTime, EphemerisData>();
			for(int i = 0; i < posList.Count && i < times.Count; i++) {
				cassiniData.Add(times[i], new EphemerisData(posList[i], times[i]));
			}
			//Add the cassini data to the dictionary
			bodies["Cassini_Temp"] = cassiniData;
		}
		
		
		//Let gameobject know they can now receive their data
		foreach (GameObject mover in Movers){
			mover.SendMessage ("Loaded");
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
		Debug.Log(bodies.ContainsKey(bodyName));
		if(bodies.ContainsKey(bodyName)) {
			SortedList<DateTime, EphemerisData> bodyData = bodies[bodyName];
			//Go through and get the data
			foreach(KeyValuePair<DateTime, EphemerisData> dat in bodyData) {
				if(dat.Key >= start && dat.Key <= end) {
					accumulator.Add(dat.Value);
				}
			}
		}
		Debug.Log (accumulator.Count);

		return accumulator;
	}
	
	//------------------------------
}
