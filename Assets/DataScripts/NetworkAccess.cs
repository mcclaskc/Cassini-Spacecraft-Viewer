using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Threading;
using System.Net;
using System.Text;

public class NetworkAccess : MonoBehaviour {

	public int BufferSize = 50;
	private PriorityQueue<EphemBlock> dataBlocks = new PriorityQueue<EphemBlock>();
	private List<DateTime> querying = new List<DateTime>();

	void Start() {}

	public  List<SensorDataAvailability> AvailableDataBlocks(DateTime start, DateTime end) {
		return new List<SensorDataAvailability>();
	}

	public  void RequestTimeRange(DateTime start, DateTime end, string sensorName) {
	}
	
	public  bool TimeRangeAvailable(DateTime start, DateTime end, string sensorName) {
		return false;
	}

	//Gets the actual data for a sensor and time range
	public  List<DataChunk> GetData(DateTime start, DateTime end, string sensorName) {
		return new List<DataChunk>();
	}

	//Functions used for ephemeris data
	//Used to request some ephemeris be loaded
	public void RequestEphemeris(DateTime start, DateTime end) {
		//Figure out how many day-blocks this is
		DateTime queuedDay = new DateTime(start.Year, start.Month, start.Day);
		while(queuedDay < end) {
			DateTime[] startEnd = new DateTime[2];
			EphemBlock block = new EphemBlock(queuedDay);
			lock(dataBlocks) {
				lock(querying) {
					if(!querying.Contains(queuedDay) && dataBlocks.ContainsElement(block) == null) {
						startEnd[0] = queuedDay;
						startEnd[1] = queuedDay.AddDays(1);
						querying.Add(queuedDay);
						ThreadPool.QueueUserWorkItem(new WaitCallback(GetEphem), startEnd);
					}
				}

				block = dataBlocks.ContainsElement(block);
				if(block != null) {
					dataBlocks.UpdatePriority(block, (float)(DateTime.Now - DateTime.Today).Seconds);
				}
			}

			queuedDay = queuedDay.AddDays(1);
		}
	}

	//Returns true if the timerange has this chunk of telemetry loaded
	public bool EphemerisAvailable(DateTime start, DateTime end, string body) {
		return true;
	}

	//Retrieves the list of positions for all bodies
	public  EphemBlock GetEphemeris(DateTime start) {
		//Returns the ephemeris block requested from the start
		DateTime time = new DateTime(start.Year, start.Month, start.Day);
		return dataBlocks.ContainsElement(new EphemBlock(time));
	}

	//Called in a seperate thread to pull the state info
	private void GetEphem(System.Object stateInfo) {
		//Stateinfo is actually a start/end time
		DateTime[] startEnd = (DateTime[]) stateInfo;
		DateTime start = startEnd[0];
		DateTime end = startEnd[1];
		EphemBlock block = new EphemBlock(start);
		EphemBlock newBlock;

		//Check if we've already got this block
		lock(dataBlocks) {
			newBlock = dataBlocks.ContainsElement(block);
			if(newBlock != null) {
				dataBlocks.UpdatePriority(newBlock, (float)(DateTime.Now - DateTime.Today).Seconds);
			}
		}

		if(newBlock != null) return;

		string url = "http://cassini-spacecraft-viewer.herokuapp.com/api/ephem?datetime=";
		url += start.Year + "-" + start.Month + "-" + start.Day + "%20";
		url += start.Hour + ":" + start.Minute + ":" + start.Second;
		url += "&end_datetime=";
		url += end.Year + "-" + end.Month + "-" + end.Day + "%20";
		url += end.Hour + ":" + end.Minute + ":" + end.Second;

		try {
			WebClient client = new WebClient();
			Byte[] pageData = client.DownloadData(url);

			int i = 0;
			try{
				json list = json.fromString(Encoding.ASCII.GetString(pageData)).getObject("content").getArray("ephems");
				json chunk;
				for(i = 0; i < list.length(); i++) {
					//Get this particular peice of data
					chunk = list._get(i);
					Vector3 pos = list.getVector3(i);
					DateTime time = DateTime.Parse(chunk.getString("timestamp"));
					string body = chunk.getString("body");
					EphemerisData eph = new EphemerisData(pos, time);
					block.AddEphem(body, eph);
				}
			} catch (Exception e) {
				Debug.Log(e.ToString());
			} finally {
				//Put the ephemblock into the priority queue
				lock(dataBlocks) {
					dataBlocks.Enqueue(block, (float)(DateTime.Now - DateTime.Today).Seconds);
					//Also, keep the queue under the max buffer size
					if(dataBlocks.Count() > BufferSize) 
						dataBlocks.Dequeue();
				}

				//Also pull this out of querying, finally
				lock(querying) {
					querying.Remove(start);
				}
			}

		} catch (WebException webEx) {
			Debug.Log(webEx.ToString());
		}
	}

	public class EphemBlock : IEquatable<EphemBlock>, IEquatable<DateTime> {
		private Dictionary<string, List<EphemerisData>> data = new Dictionary<string, List<EphemerisData>>();
		private DateTime start;

		public EphemBlock(DateTime start) { this.start = new DateTime(start.Year, start.Month, start.Day); }
		public void AddEphem(string body, EphemerisData e) { 
			if(!data.ContainsKey(body))
				data[body] = new List<EphemerisData>();

			data[body].Add(e); 
		}

		public List<EphemerisData> getData(string body) { 
			if(data.ContainsKey(body))
				return data[body]; 
			else
				return null;
		}

		public bool Equals(EphemBlock other) {
			return other.start.Equals(this.start);
		}

		public bool Equals(DateTime time) {
			DateTime compareTime = new DateTime(time.Year, time.Month, time.Day);
			return start.Equals(compareTime);
		}
	}
}
