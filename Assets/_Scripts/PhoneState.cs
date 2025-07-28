using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneState : MonoBehaviour, OnChangeConnectionStateListener
{
    public GameObject noPhone;
    public GameObject Phone;
    public Text ip;

    public void OnChangeConnectionState(bool isConnection)
    {
        if(isConnection)
        {
            noPhone.SetActive(false);
            Phone.GetComponent<Image>().color = Color.white;
        }
        else
        {
            noPhone.SetActive(true);
            Phone.GetComponent<Image>().color = Color.gray;
        }
    }

    void SubscribeChangeConnectionState()
    {
        NetworkManager.GetInstance().AddOnChangeConnectionStateListener(this);
    }

    public void UnsubscribeChangeConnectionState()
    {
        NetworkManager.GetInstance().RemoveOnChangeConnectionStateListener(this);
    }

    // Use this for initialization
    void Start () {
        SubscribeChangeConnectionState();
        ip.text = Network.player.ipAddress;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
