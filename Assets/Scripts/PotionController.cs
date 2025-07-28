using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour {

	private GameObject spawnPotion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSpawnPotion(GameObject spawn){

		spawnPotion = spawn;

	}

	public void IsPicked(){

		SpawnController spawnController = spawnPotion.GetComponent<SpawnController> ();

		spawnController.SetLibre (true);

	}

}
