using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float speed = 4.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float step = Time.deltaTime * speed;

		if (Input.GetKey (KeyCode.Z)) {

			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + step);


		} else if (Input.GetKey (KeyCode.S)) {

			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - step);

		}

		else if (Input.GetKey (KeyCode.Q)) {

			transform.position = new Vector3 (transform.position.x - step, transform.position.y, transform.position.z);

		} else if (Input.GetKey (KeyCode.D)) {

			transform.position = new Vector3 (transform.position.x + step, transform.position.y, transform.position.z);


		}

	}
}
