using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class moveCassini : MonoBehaviour {
	
	private double[,] cassiniPos;
	private double[,] cassiniVel;
	private double[,] titanPos;
	private int i = 0;
	private Vector3 target;
	private Vector3 TitanTarget;
	public GameObject Saturn;
	public GameObject Titan;
	private float updateRate = 0.01f;
	private float nextUpdate = 0.0f;
	private bool play = false;
	int count;
	
	// Use this for initialization
	void Start () {
	
		Load_CassiniData(100);
		Load_TitanData (100);
		
		transform.position = new Vector3((float)cassiniPos[0,0],(float)cassiniPos[1,0],(float)cassiniPos[2,0]);
		Titan.transform.position = new Vector3((float)titanPos[0,0],(float)titanPos[1,0],(float)titanPos[2,0]); 
		
		target = transform.position;
		TitanTarget = Titan.transform.position;
		
		count = 0;
		
		
	}
	
	void Update(){
		
		transform.position = Vector3.MoveTowards(transform.position, target, 100f);
		Titan.transform.position = Vector3.MoveTowards (Titan.transform.position, TitanTarget, 100f);
		
		if(Input.GetKeyDown (KeyCode.E))
		{
			if(play)
			{
				play = false;
			}
			else
			{
				play = true;
			}
		}
		if(Time.time > nextUpdate && play)
		{
			
			Debug.Log (count);
			Debug.Log(gameObject.name);
			Debug.Log(titanPos[0,i] + " " + titanPos[1,i] + " " +titanPos[2,i]);
			nextUpdate = Time.time + updateRate;
			target = new Vector3((float)cassiniPos[0,i],(float)cassiniPos[1,i],(float)cassiniPos[2,i]);
			TitanTarget = new Vector3((float)titanPos[0,i],(float)titanPos[1,i],(float)titanPos[2,i]);
			i++;
			count++;
		}
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
				cassiniPos[ 0, t_index ] = xPos/10;
				cassiniPos[ 1, t_index ] = yPos/10;
				cassiniPos[ 2, t_index ] = zPos/10;	
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
				titanPos[ 0, t_index ] = xPos/10;
				titanPos[ 1, t_index ] = yPos/10;
				titanPos[ 2, t_index ] = zPos/10;				
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

	
}
