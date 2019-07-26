using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using GameSparks.Api.Requests;
using GameSparks.Core;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    public GameObject mainMenu;
    public GameObject serverMenu;
    public GameObject createMenu;
    public GameObject connectMenu;
    public GameObject randomMenu;
    public GameObject closeMenu;

    public GameObject serverPrefab;
    public GameObject clientPrefab;

    public InputField nameInput;

    private string clientName = "";
    private static string Client_IP
    {
        get
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string ClientIP = "127.0.0.1";
            for (int i = 0; i < host.AddressList.Length; i++)
            {
                if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    ClientIP = host.AddressList[i].ToString();
                }
            }
            return ClientIP;
        }
    }
    private string empty_ip = "";
    private string temp_ip = Client_IP;
    private bool created = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        Instance = this;
        GameObject.Find("IPText").GetComponent<Text>().text = "User's IP: " + Client_IP;
        serverMenu.SetActive(false);
        createMenu.SetActive(false);
        connectMenu.SetActive(false);
        randomMenu.SetActive(false);
        closeMenu.SetActive(false);
        if (GameSparksManager.instance != null && GameSparksManager.instance.userName != "")
        {
            if (GameSparksManager.instance.userName != "")
                GameObject.Find("NameInput").GetComponentInChildren<Text>().text = GameSparksManager.instance.userName;
            GameObject.Find("IPText").GetComponent<Text>().text += "\r\nEXP: " + GameSparksManager.instance.userEXP + ", Gold: " + GameSparksManager.instance.userGold;
            new LogEventRequest().SetEventKey("FETCH_DECK")
                        .Send((res) =>
                        {
                            GSData data = res.ScriptData.GetGSData("player_deck");

                            int[] dataArray = new int[10];
                            dataArray[0] = int.Parse(data.GetInt("DECK_0").ToString());
                            dataArray[1] = int.Parse(data.GetInt("DECK_1").ToString());
                            dataArray[2] = int.Parse(data.GetInt("DECK_2").ToString());
                            dataArray[3] = int.Parse(data.GetInt("DECK_3").ToString());
                            dataArray[4] = int.Parse(data.GetInt("DECK_4").ToString());
                            dataArray[5] = int.Parse(data.GetInt("DECK_5").ToString());
                            dataArray[6] = int.Parse(data.GetInt("DECK_6").ToString());
                            dataArray[7] = int.Parse(data.GetInt("DECK_7").ToString());
                            dataArray[8] = int.Parse(data.GetInt("DECK_8").ToString());
                            dataArray[9] = int.Parse(data.GetInt("DECK_9").ToString());

                            GameSparksManager.instance.userDeck[0] = dataArray[0];
                            GameSparksManager.instance.userDeck[1] = dataArray[1];
                            GameSparksManager.instance.userDeck[2] = dataArray[2];
                            GameSparksManager.instance.userDeck[3] = dataArray[3];
                            GameSparksManager.instance.userDeck[4] = dataArray[4];
                            GameSparksManager.instance.userDeck[5] = dataArray[5];
                            GameSparksManager.instance.userDeck[6] = dataArray[6];
                            GameSparksManager.instance.userDeck[7] = dataArray[7];
                            GameSparksManager.instance.userDeck[8] = dataArray[8];
                            GameSparksManager.instance.userDeck[9] = dataArray[9];
                        });
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectButton()
    {
        mainMenu.SetActive(false);
        connectMenu.SetActive(true);
    }

    public void CreateButton()
    {
        bool result = false;
        string hostAddress = GameObject.Find("CreateInput").GetComponent<InputField>().text;
        if (hostAddress == "")
            hostAddress = Client_IP;

        try
        {
            Server s = Instantiate(serverPrefab).GetComponent<Server>();
            s.Init();

            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.isHost = true;

            if (clientName == "" && GameSparksManager.instance != null)
                c.clientName = GameSparksManager.instance.userName;
            else
                c.clientName = clientName;

            result = c.ConnectToServer(hostAddress, 6321);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

        createMenu.SetActive(false);
        if (result)
        {
            temp_ip = hostAddress;
            created = true;
            serverMenu.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(true);
        }
    }

    public void HostButton()
    {
        clientName = nameInput.text;

        mainMenu.SetActive(false);
        createMenu.SetActive(true);
        GameObject.Find("CreateInput").GetComponent<InputField>().placeholder.GetComponent<Text>().text = Client_IP;
    }

    public void ConnectToServerButton()
    {
        string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if (hostAddress == "")
            hostAddress = "127.0.0.1";

        try
        {
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.clientName = nameInput.text;
            if (GameSparksManager.instance != null && c.clientName == "")
                c.clientName = GameSparksManager.instance.userName;
            c.ConnectToServer(hostAddress, 6321);
            connectMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            connectMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    public void BackButton()
    {
        Client c = FindObjectOfType<Client>();
        if (c != null)
            Destroy(c.gameObject);

        Server s = FindObjectOfType<Server>();
        if (s != null)
        {
            Debug.Log("Stop");
            s.server.Stop();
            Destroy(s.gameObject);
        }

        created = false;
        mainMenu.SetActive(true);
        serverMenu.SetActive(false);
        createMenu.SetActive(false);
        connectMenu.SetActive(false);
        randomMenu.SetActive(false);
        closeMenu.SetActive(false);
    }

    public void DestroyButton()
    {
        Client c = FindObjectOfType<Client>();
        if (c != null)
            Destroy(c.gameObject);

        Server s = FindObjectOfType<Server>();
        if (s != null)
        {
            Debug.Log("Stop");
            s.server.Stop();
            Destroy(s.gameObject);
        }

        mainMenu.SetActive(true);
        serverMenu.SetActive(false);
        createMenu.SetActive(false);
        connectMenu.SetActive(false);
        randomMenu.SetActive(false);
        closeMenu.SetActive(false);

        if (created)
        {
            new LogEventRequest().SetEventKey("DESTROY_ROOM")
                        .SetEventAttribute("IP", temp_ip)
                        .Send((res) =>
                        {
                            if (!res.HasErrors)
                            {
                                Debug.Log("Room deleted successfully.");
                                created = false;
                            }
                            else
                            {
                                Debug.Log("Deletion failed. : " + res.Errors.JSON.ToString());
                                created = false;
                            }
                        });
        }
    }

    public void HotseatButton()
    {
        SceneManager.LoadScene("ChessGame");
    }

    public void CustomButton()
    {
        SceneManager.LoadScene("Pick");
    }

    public void RandomButton()
    {
        mainMenu.SetActive(false);
        randomMenu.SetActive(true);

        new LogEventRequest().SetEventKey("FIND_ROOM")
                    .SetEventAttribute("IP", Client_IP)
                    .Send((res) =>
                    {
                        if (!res.HasErrors)
                        {
                            
                            GSData data = res.ScriptData.GetGSData("empty_ip");

                            if (data.JSON.ToString().Equals("{\"IP\":\"NULL\"}"))
                            {
                                Debug.Log("No emtpy room in DB.");
                                bool result = false;
                                clientName = nameInput.text;
                                try
                                {
                                    Server s = Instantiate(serverPrefab).GetComponent<Server>();
                                    s.Init();

                                    Client c = Instantiate(clientPrefab).GetComponent<Client>();
                                    c.isHost = true;

                                    if (clientName == "" && GameSparksManager.instance != null)
                                        c.clientName = GameSparksManager.instance.userName;
                                    else
                                        c.clientName = clientName;

                                    result = c.ConnectToServer(Client_IP, 6321);
                                }
                                catch (Exception ex)
                                {
                                    Debug.LogError(ex.Message);
                                }

                                if (result)
                                {
                                    new LogEventRequest().SetEventKey("CREATE_ROOM")
                                                .SetEventAttribute("IP", Client_IP)
                                                .Send((response) =>
                                                {
                                                    if (!response.HasErrors)
                                                    {
                                                        Debug.Log("Room created successfully.");
                                                        temp_ip = Client_IP;
                                                        created = true;
                                                    }
                                                    else
                                                    {
                                                        Debug.Log("Creation failed. : " + response.Errors.JSON.ToString());
                                                        randomMenu.SetActive(false);
                                                        mainMenu.SetActive(true);
                                                    }
                                                });
                                }
                                else
                                {
                                    randomMenu.SetActive(false);
                                    mainMenu.SetActive(true);
                                }
                            }
                            else
                            {
                                empty_ip = data.GetString("IP");
                                Debug.Log("Empty room found successfully. : " + empty_ip);

                                if (empty_ip != "")
                                {
                                    try
                                    {
                                        Client c = Instantiate(clientPrefab).GetComponent<Client>();
                                        c.clientName = nameInput.text;
                                        if (GameSparksManager.instance != null && c.clientName == "")
                                            c.clientName = GameSparksManager.instance.userName;
                                        c.ConnectToServer(empty_ip, 6321);
                                        randomMenu.SetActive(false);
                                        mainMenu.SetActive(true);
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.LogError(ex.Message);
                                        randomMenu.SetActive(false);
                                        mainMenu.SetActive(true);
                                    }
                                }
                                else
                                {
                                    randomMenu.SetActive(false);
                                    mainMenu.SetActive(true);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Finding failed. : " + res.Errors.JSON.ToString());
                        }
                    });
    }

    public void TipButton()
    {
        SceneManager.LoadScene("Tips");
    }

    public void CloseButton()
    {
        mainMenu.SetActive(false);
        closeMenu.SetActive(true);
    }

    public void ExitButton()
    {
        closeMenu.SetActive(false);
        Application.Quit();
    }

    public void StartGame()
    {
        new LogEventRequest().SetEventKey("DESTROY_ROOM")
                    .SetEventAttribute("IP", Client_IP)
                    .Send((res) =>
                    {
                        if (!res.HasErrors)
                        {
                            Debug.Log("Room deleted successfully.");
                            mainMenu.SetActive(true);
                            serverMenu.SetActive(false);
                            createMenu.SetActive(false);
                            connectMenu.SetActive(false);
                            closeMenu.SetActive(false);
                        }
                        else
                        {
                            Debug.Log("Deletion failed. : " + res.Errors.JSON.ToString());
                            mainMenu.SetActive(true);
                            serverMenu.SetActive(false);
                            createMenu.SetActive(false);
                            connectMenu.SetActive(false);
                            closeMenu.SetActive(false);
                        }
                    });

        SceneManager.LoadScene("ChessGame");
    }

}
