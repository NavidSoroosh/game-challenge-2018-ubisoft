using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingCamera : MonoBehaviour {

	public float TransitionDuration = 5.0f;

	public Transform _endTarget;
	public Transform _startTarget;
	public Transform _midTarget;
	public Transform _defaultPosition;

	public GameObject _panelFade;
	public GameObject _panelUI;
	public GameObject _panelDescription;

	public GameObject _camera;

	public GameController _gameController;

	private Animator animatorFade;
	private Animator animatorFadeDescription;

	public void StartCinematic(){

		_camera.transform.position = _startTarget.position;

		StartCoroutine(Transition(0));
		StartCoroutine (Fade ());


	}

	public void StartWithoutCinematic(){

		DestroyCinematicElements ();
		_panelDescription.SetActive (false);
		_panelUI.SetActive (true);

		_gameController.AssignDestinations ();
		_gameController.LaunchNewGame ();



	}

	IEnumerator Transition(int transition){

		Transform startTarget;
		Transform endTarget;

		if (transition == 0) {

			startTarget = _startTarget;
			endTarget = _endTarget;

		} else {


			startTarget = _midTarget;
			endTarget = _defaultPosition;

		}
				
		float t = 0.0f;

		while (t < 1.0f) {

			t += Time.deltaTime * (Time.timeScale / TransitionDuration);

			_camera.transform.position = Vector3.Lerp (startTarget.position, endTarget.position, t);

			yield return 0;

		}

	}

	IEnumerator Fade(){

		yield return new WaitForSeconds (TransitionDuration-1.5f);

		_panelFade.SetActive (true);

		yield return new WaitForSeconds (1.6f);


		_camera.transform.position = _midTarget.position;

		yield return new WaitForSeconds (3.0f);

		animatorFade.SetBool ("Replay", true);

		StartCoroutine(Transition(1));

		animatorFadeDescription.SetBool ("start", true);

		yield return new WaitForSeconds (1.5f);



		animatorFade.SetBool ("Replay", false);
		_panelDescription.SetActive (false);
		_panelUI.SetActive (true);

		yield return new WaitForSeconds(3.0f);


		_gameController.LaunchNewGame ();




	}

	private void DestroyCinematicElements(){

		GameObject[] cinematics = GameObject.FindGameObjectsWithTag ("Cinematic");

		foreach (GameObject cinematic in cinematics) {

			Destroy (cinematic);
		}

	}




	// Use this for initialization
	void Start () {

		animatorFade = _panelFade.GetComponent<Animator> ();
		animatorFadeDescription = _panelDescription.GetComponent<Animator> ();
		//StartCinematic ();
		StartWithoutCinematic ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
