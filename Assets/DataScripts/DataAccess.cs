//Data interface, to be used between those grabbing data and those providing data.
using System.Collections.Generic;
using System;

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

}

public interface DataChunk {
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
			(this.startTime == other.startTime) &&
			(this.endTime == other.endTime);
	}
}
