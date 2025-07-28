using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

    public int port;
    public bool dontDestroy;

    private NetworkConnection Phone;
    private List<Player> m_players = new List<Player>();

    public static NetworkManager instance = null;

    private OnChangeConnectionStateListener onChangeConnectionStateListener = null;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    public static NetworkManager GetInstance() {
        return instance;
    }

    // Use this for initialization
    void Start () {

        if (dontDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }

        // string ipaddress = Network.player.ipAddress;
        ConnectionConfig config = new ConnectionConfig();
        config.AddChannel(QosType.ReliableSequenced);
        config.AddChannel(QosType.Unreliable);
        NetworkServer.Configure(config, 1);
        NetworkServer.Listen(port);
        NetworkServer.RegisterHandler(888, OnReceiveMessage);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        NetworkServer.RegisterHandler(MsgType.Error, OnError);
    }

    private void OnError(NetworkMessage netMsg) {
        Debug.Log("Error");
    }

    // Update is called once per frame
    void Update ()
    {
        if (PhoneIsConnected())
        {
        }
        else
        {
            if (NetworkServer.active) {
            }
            else {
            }
        }
    }

    public void AddOnChangeConnectionStateListener(OnChangeConnectionStateListener listener)
    {
        onChangeConnectionStateListener = listener;
    }

    public void RemoveOnChangeConnectionStateListener(OnChangeConnectionStateListener listener)
    {
        if (listener == onChangeConnectionStateListener)
        {
            onChangeConnectionStateListener = null;
        }
    }

    private void SendMessageToPhone(string messageToSend)
    {
        StringMessage StringMessage = new StringMessage();
        StringMessage.value = messageToSend;
        Phone.Send(888, StringMessage);
    }

    private void OnReceiveMessage(NetworkMessage message)
    {
        StringMessage msg = new StringMessage();
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] ret = msg.value.Split('-');
        if (ret[0] == "destun")
        {
            if (m_players.Count == 0)
            {
                m_players.Add(GameObject.Find("Player1").GetComponent<Player>());
                m_players.Add(GameObject.Find("Player2").GetComponent<Player>());
                m_players.Add(GameObject.Find("Player3").GetComponent<Player>());
            }
            int id = Int32.Parse(ret[1]);
            m_players[id - 1].isStun = false;
        }
        Debug.Log("Message received: " + msg.value);
    }

    public bool PhoneIsConnected()
    {
        return Phone != null && Phone.isConnected;
    }

    void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Phone connected");

        for (int i = 0; i < NetworkServer.connections.Count; i++)
        {
            NetworkConnection nconnection = NetworkServer.connections[i];
            if (nconnection != null)
            {
                Phone = NetworkServer.connections[i];
                // Debug.Log(nconnection.isConnected);
                break;
            }
        }

        if (onChangeConnectionStateListener != null)
        {
            onChangeConnectionStateListener.OnChangeConnectionState(true);
        }

    }

    private void OnDisconnected(NetworkMessage netMsg)
    {
        Debug.Log("Phone disconnected");
        Phone = null;
        if (onChangeConnectionStateListener != null)
        {
            onChangeConnectionStateListener.OnChangeConnectionState(false);
        }
    }
}
