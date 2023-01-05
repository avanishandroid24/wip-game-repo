using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private void Start()
	{
		Application.targetFrameRate = 60;
	}
}

[Serializable]
public class GameConfiguration
{
	[Serializable]
	public class CameraConfiguration
	{
		public float moveSpeed;
		public float dragRotationFactor;
		public float digitalZoomIntensity;
		public float analogZoomIntensity;

		public CameraConfiguration(float moveSpeed, float dragRotationFactor, float digitalZoomIntensity, float analogZoomIntensity)
		{
			this.moveSpeed = moveSpeed;
			this.dragRotationFactor = dragRotationFactor;
			this.analogZoomIntensity = analogZoomIntensity;
			this.digitalZoomIntensity = digitalZoomIntensity;
		}	
	}		

	public CameraConfiguration cameraConfiguration;	
	public GameConfiguration(float moveSpeed, float dragRotationFactor, float digitalZoomIntensity, float analogZoomIntensity)
	{
		cameraConfiguration = new CameraConfiguration(moveSpeed, dragRotationFactor, digitalZoomIntensity, analogZoomIntensity);		
	}
}

/* 
   moveSpeed = 20f;
   moveTime = 5f;
   edgeScrollOffset = 20f;
   dragRotationTime = 10f;
   dragRotationFactor = 0.5f;
   zoomIntensity = 5f;
*/