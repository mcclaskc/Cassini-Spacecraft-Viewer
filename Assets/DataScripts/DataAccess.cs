//Data interface, to be used between those grabbing data and those providing data.
using System.Collections.Generic;
using System;
using UnityEngine;

//General use of the DataAccess interface would be as follows:

//choose a data block from the list returned from AvailableDataBlocks
//request the data using RequestTimeRange
//Occasionally check to see if the data is loaded yet using TimeRangeAvailable
//Pull the data when available using GetData


public interface DataAccess {

		//Functions used for information about what data exists
	//Used to determine what types of data are available
	List<SensorDataAvailability> AvailableDataBlocks(DateTime start, DateTime end);

		//Functions used for actual data access
	//Used to request that a particular set of data be loaded
	void RequestTimeRange(DateTime start, DateTime end, string sensorName);
	//Returns whether the timerange has an uninterrupted chunk of data for the given sensor
	bool TimeRangeAvailable(DateTime start, DateTime end, string sensorName);
	//Gets the actual data for a sensor and time range
	List<DataChunk> GetData(DateTime start, DateTime end, string sensorName);

	//Functions used for ephemeris data
	//Used to request some ephemeris be loaded
	void RequestEphemeris(DateTime start, DateTime end, string body);
	//Returns true if the timerange has this chunk of telemetry loaded
	bool EphemerisAvailable(DateTime start, DateTime end, string body);
	//Retrieves the list of positions for a body
	List<EphemerisData> GetEphemeris(DateTime start, DateTime end, string sensorName);

}

public interface DataChunk {
}

public class EphemerisData {
	public readonly Vector3 position;
	public readonly DateTime time;

	public EphemerisData(Vector3 position, DateTime time) {
		this.position = position;
		this.time = time;
	}
}

public class SensorDataAvailability : IEquatable<SensorDataAvailability> {
	public readonly string sensorName;
	public readonly DateTime startTime;
	public readonly DateTime endTime;

	public SensorDataAvailability(string sensorName, DateTime startTime, DateTime endTime) {
		this.sensorName = sensorName;
		this.startTime = startTime;
		this.endTime = endTime;
	}

	public bool Equals(SensorDataAvailability other) {
		return (this.sensorName == other.sensorName) &&
			(this.startTime.Equals(other.startTime)) &&
			(this.endTime.Equals(other.endTime));
	}
}
