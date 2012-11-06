using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class loadData : MonoBehaviour {
	
	//Globals
	public double[,] titanPos;
	public double[,] cassiniPos;
	public double[,] cassiniVel;
	string[] utcArray;
	double[] ephemTimeArray;
	
	
	void Start(){
		Load_TimeData(20);
		Load_CassiniData(20);
	}
	
	
//------------------------------------- Titan -------------------------------------
	private bool Load_TitanData( int numTimesteps )
	{
		string filename = "Assets/Data/titanEphem_saturn.dat";
		int t_index = 0;
		double xPos, yPos, zPos;
		
		titanPos = new double[3 , numTimesteps];
		
		if ( File.Exists( filename ) )
		{
			BinaryReader binReader= new BinaryReader( File.Open( filename, FileMode.Open ) );
			for (t_index = 0; t_index < numTimesteps; t_index++)
			{
				xPos = binReader.ReadDouble();
				//Debug.Log( xPos );
				yPos = binReader.ReadDouble();
				//Debug.Log( yPos );
				zPos = binReader.ReadDouble();
				//Debug.Log( zPos );
				titanPos[ 0, t_index ] = xPos;
				titanPos[ 1, t_index ] = yPos;
				titanPos[ 2, t_index ] = zPos;				
			} // end for
			binReader.Close();
			return true;
		}// end if
		else 
		{
			Debug.Log( "Could not load Titan Ephem Data" );
			return false;
		}// end else
		
	}


//------------------------------------- Cassini -------------------------------------
	private bool Load_CassiniData( int numTimesteps )
	{
		string filename = "Assets/Data/cassiniEphem_saturn.dat";
		int t_index = 0;
		double xPos, yPos, zPos, delX, delY, delZ;
		
		cassiniPos = new double[3,numTimesteps];
		cassiniVel = new double[3,numTimesteps];
		
		if ( File.Exists( filename ) )
		{
			BinaryReader binReader= new BinaryReader( File.Open( filename, FileMode.Open ) );
			for (t_index = 0; t_index < numTimesteps; t_index++)
			{
				xPos = binReader.ReadDouble();
				//Debug.Log( xPos );
				yPos = binReader.ReadDouble();
				//Debug.Log( yPos );
				zPos = binReader.ReadDouble();
				//Debug.Log( zPos );
				delX = binReader.ReadDouble();
				//Debug.Log( delX );
				delY = binReader.ReadDouble();
				//Debug.Log( delY );
				delZ = binReader.ReadDouble();
				//Debug.Log( delZ );
				cassiniPos[ 0, t_index ] = xPos;
				cassiniPos[ 1, t_index ] = yPos;
				cassiniPos[ 2, t_index ] = zPos;	
				cassiniVel[ 0, t_index ] = delX;
				cassiniVel[ 1, t_index ] = delY;
				cassiniVel[ 2, t_index ] = delZ;				
			} // end for
			binReader.Close();
			return true;
		}// end if
		else 
		{
			Debug.Log( "Could not load Cassini Ephem Data" );
			return false;
		}// end else
	
	}


//------------------------------------- time steps -------------------------------------
	bool Load_TimeData(int numTimesteps)
	{
		int t_index = 0;
		int index = 0;
		double ephemTime;
		char[] utcChar = new char[17];
		string utc = "";
		string filename = "Assets/Data/timeData.dat";
		
		utcArray = new string[ numTimesteps ];
		ephemTimeArray = new double[ numTimesteps ];

		if ( File.Exists( filename ) )
		{
			Debug.Log( "Loading Time Data" );
			BinaryReader binReader= new BinaryReader( File.Open( filename, FileMode.Open ) );
			for ( t_index = 0; t_index < numTimesteps; t_index++ )
			{
				utcChar = binReader.ReadChars( 17 );
				for ( index = 0; index < 17; index ++ )
				{
					if ( index == 0 )
					{
						utc = "UTC Time: ";
					}
					utc += utcChar[ index ];
				}

				ephemTime = binReader.ReadDouble();

				utcArray[ t_index ] = utc;
				ephemTimeArray[ t_index ] = ephemTime;
			} // end for
			binReader.Close();
			return true;
		}// end if
		else 
		{
			Debug.Log( "Could not load Time Data" );
			return false;
		}// end else
	}
}
