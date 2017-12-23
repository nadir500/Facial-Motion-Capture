using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkCoordinates {
 
	public float x{get;set;}
	public float y {get;set;}
	//private float z;
	public LandmarkCoordinates()
	{
		this.x=0;
		this.y=0;
	}
	public LandmarkCoordinates(float _x,float _y)
	{
		this.x=_x;
		this.y=_y;
	}
}
