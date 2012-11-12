using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class moveCassini : MonoBehaviour {
	
	private double[,] cassiniPos;
	private double[,] cassiniVel;
	private int i = 0;
	
	// Use this for initialization
	void Start () {
		Load_CassiniData(20);
	}
	
	private void updatePos(){
		Debug.Log(gameObject.name);
		Debug.Log(cassiniPos[0,i] + " " + cassiniPos[1,i] + " " +cassiniPos[2,i]);
		transform.Translate(new Vector3((float)cassiniPos[0,i],(float)cassiniPos[1,i],(float)cassiniPos[2,i]));
		i++;
	}
	
	
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

}
