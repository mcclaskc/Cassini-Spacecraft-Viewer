using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Threading;
using System.Net;
using System.Text;

public class NetworkAccess : DataAccess {

	void Start() {
		RequestEphemeris(new DateTime(2009, 06, 22), 
			new DateTime(2009, 06, 23),
			"");
	}

	//Private list of ephemeris data
	private List<EphemerisData> ephem = new List<EphemerisData>();

	public override List<SensorDataAvailability> AvailableDataBlocks(DateTime start, DateTime end) {
		return new List<SensorDataAvailability>();
	}

	public override void RequestTimeRange(DateTime start, DateTime end, string sensorName) {
	}
	
	public override bool TimeRangeAvailable(DateTime start, DateTime end, string sensorName) {
		return false;
	}

	//Gets the actual data for a sensor and time range
	public override List<DataChunk> GetData(DateTime start, DateTime end, string sensorName) {
		return new List<DataChunk>();
	}

	//Functions used for ephemeris data
	//Used to request some ephemeris be loaded
	public override void RequestEphemeris(DateTime start, DateTime end, string body) {
		string url = "http://cassini-spacecraft-viewer.herokuapp.com/api/ephem?datetime=";
		url += start.Year + "-" + start.Month + "-" + start.Day + "%20";
		url += start.Hour + ":" + start.Minute + ":" + start.Second;
		url += "&end_datetime=";
		url += end.Year + "-" + end.Month + "-" + end.Day + "%20";
		url += end.Hour + ":" + end.Minute + ":" + end.Second;

		ThreadPool.QueueUserWorkItem(new WaitCallback(GetEphem), url);
	}

	//Returns true if the timerange has this chunk of telemetry loaded
	public override bool EphemerisAvailable(DateTime start, DateTime end, string body) {
		return true;
	}

	//Retrieves the list of positions for a body
	public override List<EphemerisData> GetEphemeris(DateTime start, DateTime end, string sensorName) {
		return new List<EphemerisData>();
	}

	//Called in a seperate thread to pull the state info
	private void GetEphem(System.Object stateInfo) {
		//Stateinfo is actually a string
		string url = (String) stateInfo;

		try {
			WebClient client = new WebClient();
			Byte[] pageData = client.DownloadData(url);
			List<EphemerisData> ephemlocal = new List<EphemerisData>();

			int i = 0;
			try{
				json list = json.fromString(Encoding.ASCII.GetString(pageData)).getObject("content").getArray("ephems");
				json chunk;
				for(i = 0; i < list.length(); i++) {
					//Get this particular peice of data
					chunk = list._get(i);
					Vector3 pos = list.getVector3(i);
					DateTime time = DateTime.Parse(chunk.getString("timestamp"));
					//string body = chunk.getString("body");
					EphemerisData eph = new EphemerisData(pos, time);
					ephemlocal.Add(eph);
				}
			} catch (Exception e) {
				Debug.Log(e.ToString());
			} finally {
				//Put stuff in the ephem
				lock(ephem) {
					foreach(EphemerisData e in ephemlocal)
						ephem.Add(e);
				}
			}

		} catch (WebException webEx) {
			Debug.Log(webEx.ToString());
		}
	}
}
