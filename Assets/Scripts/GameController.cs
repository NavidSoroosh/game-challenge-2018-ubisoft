using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	//Points de spawns des pnjs et points de passages utilisés
	//dans le pathfinding pour leur déplacement
	private GameObject[] _destinations;

	//Prefab du pnj
	public GameObject _PNJ;

	//Liste des matériaux des pnjs
	public Material[] _materials;

	//Timer déterminant la durée du round
	public float timer = 120.0f;

	public float timerPotion = 10.0f;
	public float timerPanacee;

	//Nombre de pnjs de chaque joueur
	//0 : church (blue)
	//1 : infected (yellow)
	//2 : medic (green)
	public int[] nbPNJ;

	public int[] finalScore;

	//Liste des tags des factions des pnjs
	public string[] tagsPNJ;

	//Liste des noms des joueurs
	public string[] playersName;

	//Liste des couleurs des joueurs
	public string[] playersColor;

	//Liste des textes dans l'ui affichant le score
	//0 : church(blue)
	//1 : infected (yellow)
	//2 : medic (green)
	public Text[] _textScore;

	//Texte dans l'ui affichant le temps du timer
	public Text _textTimer;

	//Texte dans l'ui affichant le gagnant du round
	public Text _textWinnerRoundName;

	//Texte dans l'ui affichant le gagnant de la partie
	public Text _textWinnerGameName;


	//Texte dans l'ui affichant le numéro du round
	public Text _textRound;

	public Text[] _textsScoreRound;

	public Text[] _textsScoreGame;

	public Text[] _textEndScoreGame;

	public Text _textDescription;

	//Panel  dans l'ui de fin de round
	public GameObject _panelEndRound;

	//Panel dans l'ui de fin de partie
	public GameObject _panelEndGame;

	//Numéro du round actuel
	private int round = 1;

	private bool endRound = true;

	private bool panacee = false;

	private int dominantFaction;

	public GameObject _panacee;
	public GameObject _spawnPanacee;

	public GameObject[] _potions;

	// Use this for initialization
	void Start () {

		AssignDestinations ();

		ManageMovementPlayers (false);

		SetDominantFactionRandom ();
		SetTextDescription ();
		ChangeCinematicElements ();

		/*
		CreatePNJ ();
		UpdateScore ();
		UpdateRound ();
		*/
	}
		
	// Update is called once per frame
	void Update () {

		if (endRound == false) {

			timer -= Time.deltaTime;
			timerPanacee -= Time.deltaTime;
			timerPotion -= Time.deltaTime;

		}

		//Rajouter les if pour les fins de timers

		if (timerPotion <= 0 && endRound == false) {

			InstantiatePotion ();
		}

		if (timerPanacee <= 0 && endRound == false && panacee == false) {

			InstantiatePanacee ();
			panacee = true;

		}
			
		_textTimer.text = "Timer : " + Mathf.RoundToInt(timer).ToString ();

		if (timer <= 0 && endRound == false) {

			EndRound ();
		}

		Debug.Log ("TimerPanacee : + " + timerPanacee.ToString ());
			
	}


	//Change la faction du pnj donné en paramètre
	//new faction correspond à :
	//0 : Clergé
	//1 : Infecté
	//2 : Medecins
	public void ChangeFaction(int newFaction,GameObject pnj){

		pnj.GetComponent<Renderer>().material = _materials[newFaction];
		pnj.tag = tagsPNJ [newFaction];

		PNJController pnjController = pnj.GetComponent<PNJController> ();

		pnjController.SetFaction (newFaction);

	}

	//Met à jour le score des joueurs (Nombre de convertis
	//possédés)
	public void UpdateScore(){

		for (int i = 0; i < 3; i++) {

			_textScore [i].text = nbPNJ[i].ToString ();

		}

	}

	//Met à jour le numéro du round
	private void UpdateRound(){

		_textRound.text = "Round : " + round.ToString ();

	}

	public void ChangeScore(int factionDecrease,int factionIncrease){

		nbPNJ [factionDecrease]--;
		nbPNJ [factionIncrease]++;

		UpdateScore ();
	}


	//Fonction pour tester la conversion des pnjs :
	//change de faction un pnj aléatoire
	//0 : blue to yellow (Clergé => infecté)
	//1 : yellow to green (Infecté => Médecin)
	//2 : green to blue (Médecin => Clergé)
	public void ChangePNJColorRandom(int change){

		int oldFaction = 0;
		int newFaction = 0;

		switch (change) {

		case 0:

			oldFaction = 0;
			newFaction = 1;

			break;

		case 1:

			oldFaction = 1;
			newFaction = 2;

			break;

		case 2:

			oldFaction = 2;
			newFaction = 0;

			break;

		}
			
		GameObject[] pnjs = GameObject.FindGameObjectsWithTag (tagsPNJ[oldFaction]);

		int length = pnjs.Length; 

		int rand = Random.Range (0, length);

		GameObject pnj = pnjs [rand];

		ChangeFaction (newFaction, pnj);

		nbPNJ [oldFaction]--;
		nbPNJ [newFaction]++;

		UpdateScore ();


	}

	//Assigne les GameObject disposant du tag "Destination" au GameController
	public void AssignDestinations(){

		_destinations = GameObject.FindGameObjectsWithTag ("Destination");


	}

	//Instancie les pnjs aléatoirement sur les points
	//de spawn de la carte
	private void CreatePNJ(){

		GameObject destination;
		GameObject pnj;
		Material material;

		int nb;

		//Effectue un tour de boucle pour chaque faction
		for (int i = 0; i < 4; i++) {

			nb = nbPNJ [i];

			//Créé le pnj sur un spawn aléatoire
			//et lui assigne sa faction
			for (int j =0; j < nb; j++) {

				int length = _destinations.Length;
				int rand = Random.Range (0, length);

				destination = _destinations [rand];

				pnj = Instantiate (_PNJ, destination.transform.position, destination.transform.rotation);

				ChangeFaction (i, pnj);

			}

		}


	}

	//Met fin au round et désigne le vainqueur
	public void EndRound(){

		endRound = true;
		ManageMovementPlayers (false);
		Time.timeScale=0; 

		CountFinalScore ();
		//Si le round actuel est le round 3,
		//alors la partie s'achève et on désigne
		//le vainqueur de la partie
		if (round == 3) {

			UpdateEndGameTextScore ();
			_panelEndGame.SetActive(true);

		//Sinon, on affiche le vainqueur du round et le jeu
		//est en attente de lancer le prochain round
		} else {

			UpdateEndRoundTextScore ();
			round++;
			_panelEndRound.SetActive (true);

		}


		GetWinner();

	}

	private void UpdateEndRoundTextScore(){

		for (int i = 0; i < 3; i++) {

			_textsScoreRound [i].text = nbPNJ [i].ToString ();
			_textsScoreGame [i].text = finalScore [i].ToString ();

		}

	}

	private void UpdateEndGameTextScore(){

		for (int i = 0; i < 3; i++) {

			_textEndScoreGame [i].text = finalScore [i].ToString ();
		}
	}

	private void CountFinalScore(){

		for(int i = 0;i<3;i++){

			finalScore[i] += nbPNJ[i];

		}

	}

	//Détermine le vainqueur du round
	//ou de la partie
	private void GetWinner(){

		int winnerScore = 0;
		int winner = 0;


		//Vérifie quel joueur a le nombre
		//de convertis le plus grand

		for (int i = 0; i < 3; i++) {

			if (round == 3) {

				if (finalScore [i] > winnerScore) {

					winner = i;
					winnerScore = finalScore [i];
				}

			} else {

				if (nbPNJ [i] > winnerScore) {

					winner = i;
					winnerScore = nbPNJ [i];
				}
			}
		}

		//Récupère la couleur du joueur ayant gagné
		Color colorWinner = new Color ();
		ColorUtility.TryParseHtmlString (playersColor[winner], out colorWinner);

		//Si on est au round 3, on affiche le gagnant de la partie
		if (round == 3) {
			
			_textWinnerGameName.text = playersName[winner];
			_textWinnerGameName.color = colorWinner;
			SetDominantFaction (winner);

		//Sinon, on affiche le gagnant du round
		} else {

			_textWinnerRoundName.text = playersName[winner];
			_textWinnerRoundName.color = colorWinner;
		}

	}

	//Retire tous les pnjs de la carte
	//(il faudra aussi renvoyer les joueurs à leur spawn)
	private void Clean(){

		GameObject[] pnjs;

		//On boucle sur chaque faction pour
		//en retirer tous les pnjs
		for (int i = 0; i < 3; i++) {

			pnjs = GameObject.FindGameObjectsWithTag (tagsPNJ [i]);

			foreach (GameObject pnj in pnjs) {

				Destroy (pnj);

			}

		}

	}

	//Lance une nouvelle partie
	public void LaunchNewGame(){

		for (int i = 0; i < 3; i++) {

			if (i == dominantFaction) {

				nbPNJ [i] = 3;

			} else {

				nbPNJ[i] = 0;
			}

		}

		nbPNJ[3] = 4;

		for (int i = 0; i < 3; i++) {

			finalScore [i] = 0;
		}

		round = 1;
		_panelEndGame.SetActive (false);
		LaunchNewRound ();

	}

	//Lance un nouveau round
	public void LaunchNewRound(){


		_panelEndRound.SetActive (false);
		Clean ();
		CreatePNJ ();
		timer = 120;
		SetTimerPanacee ();
		Time.timeScale=1;
		endRound = false;
		ManageMovementPlayers (true);
		UpdateRound ();
		UpdateScore ();

	}

	public void LoadScene(string name){

		_panelEndGame.SetActive (false);
		Time.timeScale=1; 
		SceneManager.LoadScene (name);

	}

	public void ManageMovementPlayers(bool move){

		GameObject[] Players = GameObject.FindGameObjectsWithTag ("Player");

		foreach (GameObject player in Players) {

			if (move == true) {


			} else {


			}

		}
	}

	private void SetDominantFactionRandom(){

		dominantFaction = Random.Range (0, 3);

	}

	private void SetDominantFaction(int dominant){

		dominantFaction = dominant;

	}

	private void SetTextDescription(){

		string sDominant = playersName [dominantFaction];

		_textDescription.text = "The time is 1529. The " + sDominant +" rule on this small village. Without support, the two other factions are destined to die out. It is up to you to go back in time to change history.";

	}

	private void ChangeCinematicElements(){

		GameObject[] cinematics = GameObject.FindGameObjectsWithTag ("Cinematic");

		foreach (GameObject cinematic in cinematics) {

			cinematic.GetComponent<Renderer>().material = _materials[dominantFaction];
		}

	}

	private void SetTimerPanacee(){

		timerPanacee = 60.0f + Random.Range (0.0f, 40.0f);



	}

	private void InstantiatePotion(){

		GameObject[] spawnsPotions = GameObject.FindGameObjectsWithTag ("SpawnPotion");
	
		GameObject[] spawnsPotionsLibres = new GameObject[spawnsPotions.Length];

		SpawnController spawnController;

		int j = 0;

		foreach (GameObject spawn in spawnsPotions) {

			spawnController = spawn.GetComponent<SpawnController> ();

			if (spawnController.GetLibre ()) {

				spawnsPotionsLibres [j] = spawn;
				j++;

			}

		}

		if (j != 0) {

			int rand = Random.Range (0, j);
			Debug.Log ("j : " + j.ToString ());
			Debug.Log ("rand : " + rand.ToString ());
			GameObject spawnPotion = spawnsPotionsLibres [rand];

			SpawnController spawnPotionController = spawnPotion.GetComponent<SpawnController> ();

			spawnPotionController.SetLibre (false);

			int lengthPotion = _potions.Length;

			rand = Random.Range (0, lengthPotion);

			GameObject potionPref = _potions [rand];

			GameObject potion;



			potion = Instantiate (potionPref, spawnPotion.transform.position + new Vector3(0,0.5f,0), spawnPotion.transform.rotation);

			PotionController potionController = potion.GetComponent<PotionController> ();

			potionController.SetSpawnPotion (spawnPotion);


		}

		timerPotion = 10.0f;

	}

	private void InstantiatePanacee(){

		Instantiate (_panacee, _spawnPanacee.transform.position  + new Vector3(0,2,0), _spawnPanacee.transform.rotation);

	}
		
		
}
