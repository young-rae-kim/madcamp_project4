using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour
{
    public int restart = 0;
    public int port = 6321;
    public List<ServerClient> clients;
    public List<ServerClient> disconnectList;

    public TcpListener server;
    public bool serverStarted;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
        }
        catch (Exception ex)
        {
            Debug.Log("Socket Error: " + ex.Message);
        }
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener) ar.AsyncState;

        string allUsers = "";
        foreach (ServerClient i in clients)
        {
            if (i.clientName != "")
                allUsers += i.clientName + '|';
        }

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);
        StartListening();

        Broadcast("SWHO|" + allUsers, clients[clients.Count - 1]);
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    // Server Send
    private void Broadcast(string data, ServerClient c)
    {
        List<ServerClient> sc = new List<ServerClient> { c };
        Broadcast(data, sc);
    }
    private void Broadcast(string data, List<ServerClient> cl)
    {
        foreach (ServerClient sc in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception ex)
            {
                Debug.LogError("Write error: " + ex.Message);
            }
        }
    }

    // Server Read
    private void OnIncomingData(ServerClient c, string data)
    {
        Debug.Log("Server: " + data);
        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "CWHO":
                c.clientName = aData[1];
                c.isHost = (aData[2] == "0") ? false : true;
                Broadcast("SCNN|" + c.clientName, clients);
                break;
            case "CMOV":
                Broadcast("SMOV|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4] + "|" + aData[5] + "|" + aData[6], clients);
                break;
            case "CMSG":
                Broadcast("SMSG|" + c.clientName + ": " + aData[1], clients);
                break;
            case "CPRO":
                if (int.Parse(aData[4]) == 0) 
                    Broadcast("SPRO|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4], clients[0]);
                else
                    Broadcast("SPRO|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4], clients[1]);
                break;
            case "CCRD":
                if (int.Parse(aData[6]) == 0)
                    Broadcast("SCRD|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4] + "|" + aData[5] + "|" + aData[6], clients[0]);
                else
                    Broadcast("SCRD|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4] + "|" + aData[5] + "|" + aData[6], clients[1]);
                break;
            case "CCRD3":
                if (int.Parse(aData[8]) == 0)
                    Broadcast("SCRD3|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4] +
                        "|" + aData[5] + "|" + aData[6] + "|" + aData[7] + "|" + aData[8], clients[0]);
                else
                    Broadcast("SCRD3|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4] +
                        "|" + aData[5] + "|" + aData[6] + "|" + aData[7] + "|" + aData[8], clients[1]);
                break;
            case "CTRP":
                Broadcast("STRP|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4], clients);
                break;
            case "CRMT":
                if ((++restart) == 2)
                {
                    Broadcast("SRMT|", clients);
                    restart = 0;
                }
                break;
            case "CQUT":
                Broadcast("SQUT|", clients);
                Debug.Log("Stop");
                server.Stop();
                break;
        }
    }

    public void OnServerInitialized()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!serverStarted)
            return;

        if (clients.Count == 0)
            return;

        foreach (ServerClient c in clients)
        {
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                        OnIncomingData(c, data);
                }
            }
        }

        if (disconnectList.Count > 0)
        {
            for (int i = 0; i < disconnectList.Count; i++)
            {
                clients.Remove(disconnectList[i]);
                disconnectList.RemoveAt(i);
            }

            Broadcast("SQUT|", clients);
            Debug.Log("Stop");
            server.Stop();
        }
    }
}

public class ServerClient
{
    public string clientName;
    public bool isHost;
    public TcpClient tcp;

    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }


}
