using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJController : MonoBehaviour {


	public float _speed = 4.0f;
	public float _DistFuite = 5.0f;

	private Transform goal;

	private GameObject[] _destinations;

	UnityEngine.AI.NavMeshAgent agent;

	private float timer = 4.0f;

	private int numFaction;

	void Start () {

		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

		AssignDestinations ();
		NewDestination ();
	}
	
	// Update is called once per frame
	void Update () {

		Movement ();

		timer -= Time.deltaTime;

		if (timer <= 0) {

			NewDestination ();
			timer = 4.0f;
		}

	}

	//Assigne les GameObject disposant du tag "Destination" au pnj
	private void AssignDestinations(){

		_destinations = GameObject.FindGameObjectsWithTag ("Destination");


	}

	//Retourne le joueur le plus proche du PNJ
	//s'il est à portée
	private GameObject DetectPlayer(){

		GameObject player = null;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		float min = Mathf.Infinity;
		float minTemp;

		foreach (GameObject p in players) {

			//Debug.Log ("Passage dans players");
			minTemp = Vector3.Distance(p.transform.position,transform.position);

			if(min > minTemp){

				player = p;
				min = minTemp;

				//Debug.Log ("Min = " + min.ToString ());

			}


		}

		if (min > _DistFuite) {
			
			//Debug.Log ("Supérieur à 10");
			player = null;
		}


		return player;



	}

	//Si un joueur est trop proche du PNJ
	//il prend la fuite dans la direction
	//opposée, sinon il reprend sa route
	//vers sa destination
	private void Movement(){

		GameObject player;

		/*player = DetectPlayer ();

		if (player != null) {

			Debug.Log ("Fuite enclenchée");

			float step = _speed * Time.deltaTime;

			agent.Stop ();

			transform.position = Vector3.MoveTowards (transform.position, player.transform.position, -step);
		}
		*/

		 

			agent.Resume ();
			agent.destination = goal.position; 


			
	}

	//Crée une nouvelle destination
	//que le pnj cherchera à atteindre
	private void NewDestination(){

		int lenght = _destinations.Length;

		int rand = Random.Range (0, lenght);

		goal = _destinations [rand].transform;

	}

	public void SetFaction(int f){

		numFaction = f;
	}

	public int GetFaction(){

		return numFaction;
	}

}
