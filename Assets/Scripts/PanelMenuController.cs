using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelMenuController : MonoBehaviour {

	public GameObject Buttons;
	public GameObject _panelCredits;

    private int lastSelectedButton = 0;
    public int selectedButton = 0;

    const float DEFAULT_COOLDOWN = 0.17f;
    float cooldown = DEFAULT_COOLDOWN;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (cooldown > 0) {
            cooldown -= Time.deltaTime;
        }

        if (cooldown <= 0) {
            float yInput = Input.GetAxisRaw("VerticalP1");
            yInput += Input.GetAxisRaw("VerticalP2");
            yInput += Input.GetAxisRaw("VerticalP3");
            if (yInput < -0.45)
            {
                lastSelectedButton = selectedButton;
                selectedButton = (selectedButton + 1) % Buttons.transform.childCount;
            }
            else if (yInput > 0.45)
            {
                lastSelectedButton = selectedButton;
                if (selectedButton == 0)
                {
                    selectedButton = Buttons.transform.childCount - 1;
                }
                else
                {
                    selectedButton--;
                }
            }

            if (selectedButton != lastSelectedButton)
            {
                cooldown = DEFAULT_COOLDOWN;
            }
        }

        Buttons.transform.GetChild(lastSelectedButton).localScale = new Vector3(1, 1);
        Buttons.transform.GetChild(selectedButton).localScale = new Vector3(1.1f, 1.1f);
        if (Input.GetButtonDown("PickItemP1") || Input.GetButtonDown("PickItemP2") || Input.GetButtonDown("PickItemP3"))
            OnClick();
    }

    public void OnClick() {
        switch(selectedButton) {
            case 0:
                Play();
                break;
            case 1:
                ShowCredits();
                break;
            case 2:
                Quit();
                break;
            default:
                break;
        }
    }

    public void Play()
    {
        LoadScene("BaseScene");
    }

    public void ShowCredits()
    {
        _panelCredits.SetActive(true);
    }

    public void CloseCredits()
    {
        _panelCredits.SetActive(false);
    }

	public void LoadScene(string name){

		SceneManager.LoadScene (name);

	}

	public void Quit(){

		Application.Quit ();

	}

}
