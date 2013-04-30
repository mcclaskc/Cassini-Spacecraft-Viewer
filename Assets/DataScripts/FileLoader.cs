using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class FileLoader : DataAccess {

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
					//double ephemTime = binReader.ReadDouble();
					//Debug.Log (DateTime.Parse(utc));
					times.Add(DateTime.Parse(utc));

				} catch (Exception) {
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
				} catch (Exception) {
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
		
		filename = "Assets/Data/Dione/dioneEphem_saturn_2009-JUN-21.dat";
		if(File.Exists(filename)) {
			List<Vector3> posList = new List<Vector3>();
			Debug.Log("Loading Dione ephemera");
			BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open));
			bool finished = false;

			while(! finished) {
				try{
					double x = binReader.ReadDouble();
					double z = binReader.ReadDouble();
					double y = binReader.ReadDouble();
					posList.Add(new Vector3((float) x, (float)y, (float)z));
				} catch (Exception) {
					finished = true;
				}
			}

			//Match up the positions and times
			SortedList<DateTime, EphemerisData> dioneData = new SortedList<DateTime, EphemerisData>();
			for(int i = 0; i < posList.Count && i < times.Count; i++) {
				dioneData.Add(times[i], new EphemerisData(posList[i], times[i]));
			}
			
			bodies["Dione"] = dioneData;
			
		}
		
		
		
		filename = "Assets/Data/Enceladus/enceladusEphem_saturn_2009-JUN-21.dat";
		if(File.Exists(filename)) {
			List<Vector3> posList = new List<Vector3>();
			Debug.Log("Loading Enceladus ephemera");
			BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open));
			bool finished = false;

			while(! finished) {
				try{
					double x = binReader.ReadDouble();
					double z = binReader.ReadDouble();
					double y = binReader.ReadDouble();
					posList.Add(new Vector3((float) x, (float)y, (float)z));
				} catch (Exception) {
					finished = true;
				}
			}

			//Match up the positions and times
			SortedList<DateTime, EphemerisData> enceladusData = new SortedList<DateTime, EphemerisData>();
			for(int i = 0; i < posList.Count && i < times.Count; i++) {
				enceladusData.Add(times[i], new EphemerisData(posList[i], times[i]));
			}
			
			bodies["Enceladus"] = enceladusData;
			
		}
		
		
		
		filename = "Assets/Data/Iapetus/iapetusEphem_saturn_2009-JUN-21.dat";
		if(File.Exists(filename)) {
			List<Vector3> posList = new List<Vector3>();
			Debug.Log("Loading Iapetus ephemera");
			BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open));
			bool finished = false;

			while(! finished) {
				try{
					double x = binReader.ReadDouble();
					double z = binReader.ReadDouble();
					double y = binReader.ReadDouble();
					posList.Add(new Vector3((float) x, (float)y, (float)z));
				} catch (Exception) {
					finished = true;
				}
			}

			//Match up the positions and times
			SortedList<DateTime, EphemerisData> iapetusData = new SortedList<DateTime, EphemerisData>();
			for(int i = 0; i < posList.Count && i < times.Count; i++) {
				iapetusData.Add(times[i], new EphemerisData(posList[i], times[i]));
			}
			
			bodies["Iapetus"] = iapetusData;
			
		}
		
		
		filename = "Assets/Data/Mimas/mimasEphem_saturn_2009-JUN-21.dat";
		if(File.Exists(filename)) {
			List<Vector3> posList = new List<Vector3>();
			Debug.Log("Loading Mimas ephemera");
			BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open));
			bool finished = false;

			while(! finished) {
				try{
					double x = binReader.ReadDouble();
					double z = binReader.ReadDouble();
					double y = binReader.ReadDouble();
					posList.Add(new Vector3((float) x, (float)y, (float)z));
				} catch (Exception) {
					finished = true;
				}
			}

			//Match up the positions and times
			SortedList<DateTime, EphemerisData> mimasData = new SortedList<DateTime, EphemerisData>();
			for(int i = 0; i < posList.Count && i < times.Count; i++) {
				mimasData.Add(times[i], new EphemerisData(posList[i], times[i]));
			}
			
			bodies["Mimas"] = mimasData;
			
		}
		
		
		filename = "Assets/Data/Rhea/rheaEphem_saturn_2009-JUN-21.dat";
		if(File.Exists(filename)) {
			List<Vector3> posList = new List<Vector3>();
			Debug.Log("Loading Rhea ephemera");
			BinaryReader binReader = new BinaryReader(File.Open(filename, FileMode.Open));
			bool finished = false;

			while(! finished) {
				try{
					double x = binReader.ReadDouble();
					double z = binReader.ReadDouble();
					double y = binReader.ReadDouble();
					posList.Add(new Vector3((float) x, (float)y, (float)z));
				} catch (Exception) {
					finished = true;
				}
			}

			//Match up the positions and times
			SortedList<DateTime, EphemerisData> rheaData = new SortedList<DateTime, EphemerisData>();
			for(int i = 0; i < posList.Count && i < times.Count; i++) {
				rheaData.Add(times[i], new EphemerisData(posList[i], times[i]));
			}
			
			bodies["Rhea"] = rheaData;
			
		}
		
		
		/*
		filename = "Assets/Data/Mimas/mimasEphem_saturn_2009-JUN-21.dat";
		if(File.Exists(filename)) {
			List<Vector3> posList = new List<Vector3>();
			Debug.Log("Loading Mimas ephemera");
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
			SortedList<DateTime, EphemerisData> mimasData = new SortedList<DateTime, EphemerisData>();
			for(int i = 0; i < posList.Count && i < times.Count; i++) {
				mimasData.Add(times[i], new EphemerisData(posList[i], times[i]));
			}
			
			bodies["Mimas"] = mimasData;
			
		}*/
		
		//Test of LoadFile function on the sun.
		if(LoadFile("Sun", times)) Debug.Log("Sun data loaded");
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
			//double velx, vely, velz;
			//Loop that goes until file is loaded
			while(!finished) {
				try{
					//positions
					double x = binReader.ReadDouble();
					double z = binReader.ReadDouble();
					double y = binReader.ReadDouble();
					//velocities
					binReader.ReadDouble();
					binReader.ReadDouble();
					binReader.ReadDouble();
					posList.Add(new Vector3((float) x, (float) y, (float)z));
					
				}catch(Exception){
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
		
		DateTime[] startEnd = new DateTime[2];
		startEnd[0] = times[0];
		startEnd[1] = times[times.Count - 1];
		
		//Let gameobject know they can now receive their data
		foreach (GameObject mover in Movers){
			mover.SendMessage ("Loaded", startEnd);
		}
	}
	
	
	//Generalized file loading function (basically a copy paste of the titan portion of the code)
	//Loads the file "bodyNameEphem_saturn.dat" 
	public Boolean LoadFile(string bodyName, List<DateTime> times){
		string filename = "Assets/Data/"+bodyName+"Ephem_saturn.dat";
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
				} catch (Exception) {
					finished = true;
				}
			}

			//Match up the positions and times
			SortedList<DateTime, EphemerisData> titanData = new SortedList<DateTime, EphemerisData>();
			for(int i = 0; i < posList.Count && i < times.Count; i++) {
				titanData.Add(times[i], new EphemerisData(posList[i], times[i]));
			}

			bodies[bodyName] = titanData;
			return true;
		} else return false;
	}

	//DataAccess stuff
	//-----------------------------
	public override List<SensorDataAvailability> AvailableDataBlocks(DateTime start, DateTime end) {
		return new List<SensorDataAvailability>();
	}

	public override void RequestTimeRange(DateTime start, DateTime end, string sensorName) {

	}

	public override bool TimeRangeAvailable(DateTime start, DateTime end, string sensorName) {
		return false;
	}

	public override List<DataChunk> GetData(DateTime start, DateTime end, string body) {
		return null;
	}

	public override void RequestEphemeris(DateTime start, DateTime end, string body) {
	}

	public override bool EphemerisAvailable(DateTime start, DateTime end, string body) {
		return true;
	}

	public override List<EphemerisData> GetEphemeris(DateTime start, DateTime end, string bodyName) {
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
