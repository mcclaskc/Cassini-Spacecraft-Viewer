using UnityEngine;
using System.Collections;
using System.IO;

public class ImportData : MonoBehaviour {

	public TextAsset titanEphemData;
	public TextAsset cassiniEphemData;
	public TextAsset timeData;

	private double[,] titanPos;
	private double[,] cassiniPos;
	private double[,] cassiniVel;
	private string[] utcArray;
	private double[] ephemTimeArray;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//------------------------------------- Titan -------------------------------------
	private bool Load_TitanData( int numTimesteps )
	{
		int t_index = 0;
		double xPos, yPos, zPos;
		
		titanPos = new double[ 3, numTimesteps ];
		
		if ( titanEphemData )
		{
			BinaryReader binReader= new BinaryReader( new MemoryStream(titanEphemData.bytes) );
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
		int t_index = 0;
		double xPos, yPos, zPos, delX, delY, delZ;
		
		if ( cassiniEphemData )
		{
			BinaryReader binReader= new BinaryReader( new MemoryStream(cassiniEphemData.bytes) );

			cassiniVel = new double[3, numTimesteps];
			cassiniPos = new double[3, numTimesteps]; 

			for (t_index = 0; t_index < numTimesteps; t_index++)
			{
				xPos = binReader.ReadDouble();
				//Debug.Log( xPos );
				yPos = binReader.ReadDouble();
				//Debug.Log( yPos );
				zPos = binReader.ReadDouble();
				//Debug.Log( zPos );
				delX = binReader.ReadDouble();
				//Debug.Log( xPos );
				delY = binReader.ReadDouble();
				//Debug.Log( yPos );
				delZ = binReader.ReadDouble();
				//Debug.Log( zPos );
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
		
		utcArray = new string[ numTimesteps ];
		ephemTimeArray = new double[ numTimesteps ];

		if ( timeData )
		{
			Debug.Log( "Loading Time Data" );
			BinaryReader binReader= new BinaryReader( new MemoryStream(timeData.bytes) );
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
