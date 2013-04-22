using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Threading;
using System.Net;
using System.Text;

public class NetworkAccess : MonoBehaviour, DataAccess {

	void Start() {
		RequestEphemeris(new DateTime(2009, 06, 22), 
			new DateTime(2009, 06, 22),
			"");
	}

	//Private list of ephemeris data
	private List<EphemerisData> ephem = new List<EphemerisData>();
	public string test;

	public List<SensorDataAvailability> AvailableDataBlocks(DateTime start, DateTime end) {
		return new List<SensorDataAvailability>();
	}

	public void RequestTimeRange(DateTime start, DateTime end, string sensorName) {
	}
	
	public bool TimeRangeAvailable(DateTime start, DateTime end, string sensorName) {
		return false;
	}

	//Gets the actual data for a sensor and time range
	public List<DataChunk> GetData(DateTime start, DateTime end, string sensorName) {
		return new List<DataChunk>();
	}

	//Functions used for ephemeris data
	//Used to request some ephemeris be loaded
	public void RequestEphemeris(DateTime start, DateTime end, string body) {
		string url = "http://cassini-spacecraft-viewer.herokuapp.com/api/ephem?datetime=";
		url += start.Year + "-" + start.Month + "-" + start.Day + "%20";
		url += start.Hour + ":" + start.Minute + ":" + start.Second;
		url += "&end_datetime=";
		url += end.Year + "-" + end.Month + "-" + end.Day + "%20";
		url += end.Hour + ":" + end.Minute + ":" + end.Second;

		ThreadPool.QueueUserWorkItem(new WaitCallback(GetEphem), url);
	}

	//Returns true if the timerange has this chunk of telemetry loaded
	public bool EphemerisAvailable(DateTime start, DateTime end, string body) {
		return true;
	}

	//Retrieves the list of positions for a body
	public List<EphemerisData> GetEphemeris(DateTime start, DateTime end, string sensorName) {
		return new List<EphemerisData>();
	}

	//Called in a seperate thread to pull the state info
	private void GetEphem(System.Object stateInfo) {
		//Stateinfo is actually a string
		string url = (String) stateInfo;

		try {
			WebClient client = new WebClient();
			Byte[] pageData = client.DownloadData(url);
			json list = json.fromString(Encoding.ASCII.GetString(pageData)).getArray("content");
			json chunk;

			int i = 0;
			try{
				while(true) {
					chunk = list._get(i);
					Vector3 pos = chunk.getVector3("ephem");
				}

			} catch (Exception e) {
			}

		} catch (WebException webEx) {
			Debug.Log(webEx.ToString());
		}
	}
}
