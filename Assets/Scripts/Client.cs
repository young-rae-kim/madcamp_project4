using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.IO;
using System;

public class Client : MonoBehaviour
{
    public string clientName;
    public bool isHost;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public List<GameClient> players = new List<GameClient>();

    public bool ConnectToServer(string host, int port)
    {
        if (socketReady)
            return false;

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            socketReady = true;
        }
        catch (Exception ex)
        {
            Debug.Log("Socket Error: " + ex.Message);
            GameManager.Instance.connectMenu.SetActive(false);
            GameManager.Instance.mainMenu.SetActive(true);
        }

        return socketReady;
    }

    public void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();
    }

    private void OnIncomingData(string data)
    {
        Debug.Log("Client: " + data);
        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "SWHO":
                for (int i = 1; i < aData.Length; i++)
                {
                    if (aData[i] != "")
                        UserConnected(aData[i], false);
                }
                Send("CWHO|" + clientName + "|" + ((isHost)? 1: 0));
                break;
            case "SCNN":
                UserConnected(aData[1], false);
                break;
            case "SMOV":
                BoardManager.Instance.
                    UpdateChessman(aData[1], int.Parse(aData[2]), int.Parse(aData[3]), int.Parse(aData[4]), int.Parse(aData[5]));
                BoardManager.Instance.
                    LogMessage(0, aData[1], int.Parse(aData[2]), int.Parse(aData[3]), 
                    int.Parse(aData[4]), int.Parse(aData[5]), int.Parse(aData[6]));
                break;
            case "SMSG":
                BoardManager.Instance.ChatMessage(aData[1]);
                break;
            case "SPRO":
                BoardManager.Instance.UpdatePromotion(int.Parse(aData[1]), int.Parse(aData[2]), int.Parse(aData[3]));
                BoardManager.Instance.
                    LogMessage(1, aData[1], int.Parse(aData[2]), int.Parse(aData[3]), 0, 0, int.Parse(aData[4]));
                break;
            case "SCRD":
                CardManager.Instance.
                    UpdateCard(aData[1], int.Parse(aData[2]), int.Parse(aData[3]), int.Parse(aData[4]), int.Parse(aData[5]));
                BoardManager.Instance.
                    LogMessage(2, aData[1], int.Parse(aData[2]), int.Parse(aData[3]),
                    int.Parse(aData[4]), int.Parse(aData[5]), int.Parse(aData[6]));
                break;
            case "SCRD3":
                CardManager.Instance.
                    UpdateCard3(aData[1], int.Parse(aData[2]), int.Parse(aData[3]),
                    int.Parse(aData[4]), int.Parse(aData[5]), int.Parse(aData[6]), int.Parse(aData[7]));
                BoardManager.Instance.
                    LogMessage3(2, aData[1], int.Parse(aData[2]), int.Parse(aData[3]),
                    int.Parse(aData[4]), int.Parse(aData[5]), int.Parse(aData[6]), int.Parse(aData[7]), int.Parse(aData[8]));
                break;
            case "STRP":
                BoardManager.Instance.
                    UpdateTrap(int.Parse(aData[1]), int.Parse(aData[2]), int.Parse(aData[3]), int.Parse(aData[4]));
                break;
            case "SRMT":
                BoardManager.Instance.RestartGame();
                break;
            case "SQUT":
                Server s = FindObjectOfType<Server>();
                if (s != null)
                    Destroy(s.gameObject);

                Client c = FindObjectOfType<Client>();
                if (c != null)
                    Destroy(c.gameObject);

                SceneManager.LoadScene("Menu");
                break;
        }
    }

    private void UserConnected(string name, bool host)
    {
        GameClient c = new GameClient();
        c.name = name;

        players.Add(c);
        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log(clientName + ": " + players[i].name);
        }

        if (players.Count == 2)
        {
            Server s = FindObjectOfType<Server>();
            try
            {
                if (s.clients[0].isHost && s.clients[1].isHost)
                {
                    Debug.Log("Host Conflicted.");
                    string msg = "CQUT|";
                    Send(msg);
                    Debug.Log("Quit: " + msg);
                    players.Clear();
                    return;
                }
                else
                {
                    GameManager.Instance.StartGame();
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                GameManager.Instance.StartGame();
            }
        }
        else if (players.Count > 2)
        {
            players.Clear();
            return;
        }
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (socketReady)
        {
            if (stream != null)
            {
                if (stream.DataAvailable)
                {
                    string data = reader.ReadLine();
                    if (data != null)
                        OnIncomingData(data);
                }
            }
            else
            {
                CloseSocket();
                Server s = FindObjectOfType<Server>();
                if (s != null)
                    Destroy(s.gameObject);

                Client c = FindObjectOfType<Client>();
                if (c != null)
                    Destroy(c.gameObject);

                SceneManager.LoadScene("Menu");
            }
        }
    }

    private void CloseSocket()
    {
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}

public class GameClient
{
    public string name;
    public bool isHost;
}