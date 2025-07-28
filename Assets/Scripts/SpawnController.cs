using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

	private bool libre = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool GetLibre(){

		return libre;

	}

	public void SetLibre(bool l){

		libre = l;

	}
}
