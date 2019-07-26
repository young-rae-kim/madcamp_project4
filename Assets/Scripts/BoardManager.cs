using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameSparks.Api.Requests;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }

    public int HCardCount = 0;
    public int PCardCount = 0;

    public Chessman[,] Chessmans { set; get; }          //chessmans in board
    public Chessman selectedChessman;
    private bool[,] allowedMoves { set; get; }
    public List<GameObject> ActiveChessman;


    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public int[] WhiteKingPos = new int[2];
    public int[] BlackKingPos = new int[2];
    private bool[,] attacking { set; get; }
    public bool CheckW = false;
    public bool CheckB = false;

    public List<GameObject> chessmanPrefabs;
    public ArrayList SparksDlist = new ArrayList();

    public int[] EnPassantMove { set; get; }
    public bool isWhite = true;
    public bool isWhiteTurn = true;

    private int promotion;
    public bool waitCondition = false;

    public int[] TrapPosW = new int[3];
    public int[] TrapPosB = new int[3];
    public GameObject Trap;
    private GameObject trapInstantW;
    private GameObject trapInstantB;

    public Client client;

    //WK(0), WQ(1), BK(2), BQ(3)
    public bool[] CastleHasMoved = new bool[] { false, false, false, false };

    public Camera MainCamera;
    public Camera SubCamera;

    public Transform chatMessageContainer1;
    public Transform chatMessageContainer2;
    public Transform logContainer1;
    public Transform logContainer2;
    public GameObject quitMenu1;
    public GameObject quitMenu2;
    public GameObject startMenu1;
    public GameObject startMenu2;
    public GameObject messagePrefab;
    public GameObject logPrefab;

    public GameObject HostDeck;
    public GameObject PlayerDeck;

    public GameObject HostNCbtn;
    public GameObject PlayerNCbtn;

    public GameObject HelpMakeW;
    public GameObject HelpMakeB;

    public Camera cam;

    private void Start()
    {
        GameObject Canvas = HostDeck.transform.parent.parent.gameObject;
        GameObject PlayerCanvas = PlayerDeck.transform.parent.parent.gameObject;
        
        LineRenderer lineRenderer1 = gameObject.AddComponent<LineRenderer>();
        lineRenderer1.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer1.material.color = Color.gray;
        lineRenderer1.widthMultiplier = 0.05f;

        isWhite = true;

        WhiteKingPos[0] = 4;
        WhiteKingPos[1] = 0;
        BlackKingPos[0] = 4;
        BlackKingPos[1] = 7;

        TrapPosW[0] = -1;
        TrapPosW[1] = -1;
        TrapPosW[2] = -1;
        TrapPosB[0] = -1;
        TrapPosB[1] = -1;
        TrapPosB[2] = -1;

        client = FindObjectOfType<Client>();
        if (client)
        {
            isWhite = client.isHost;
        }
        else
        {
            GameObject messageInput = GameObject.Find("MessageInput");
            GameObject chatPanel = GameObject.Find("Chat");
            GameObject sendButton = GameObject.Find("SendButton");
            messageInput.SetActive(false);
            chatPanel.SetActive(false);
            sendButton.SetActive(false);
        }

        if (GameSparksManager.instance != null)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < GameSparksManager.instance.userDeck[i]; j++)
                {
                    SparksDlist.Add(i);
                }
            }
        }

        if (isWhite)
        {
            MainCamera.enabled = true; SubCamera.enabled = false;
            Canvas.SetActive(true); PlayerCanvas.SetActive(false);
            //GameObject.Find("CardDel").transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("mesh_Brazier_03W").transform.position+new Vector3(0.2f, 0.5f, 0));
            if (GameObject.Find("DataBook") != null)
            {
                GameObject.Find("WDeck").GetComponent<Make>().Dlist = (ArrayList)GameObject.Find("DataBook").GetComponent<Next>().DList.Clone();
            }
            else if (GameSparksManager.instance != null)
            {
                GameObject.Find("WDeck").GetComponent<Make>().Dlist = (ArrayList) SparksDlist.Clone();
            }
        }
        else
        {
            SubCamera.enabled = true; MainCamera.enabled = false;
            Canvas.SetActive(false); PlayerCanvas.SetActive(true);
            //GameObject.Find("CardDel").transform.position = cam.WorldToScreenPoint(GameObject.Find("mesh_Brazier_03B").transform.position + new Vector3(0.2f, 0.5f, 0));
            if (GameObject.Find("DataBook") != null)
            {
                GameObject.Find("BDeck").GetComponent<Make>().Dlist = (ArrayList)GameObject.Find("DataBook").GetComponent<Next>().DList.Clone();
            }
            else if (GameSparksManager.instance != null)
            {
                GameObject.Find("BDeck").GetComponent<Make>().Dlist = (ArrayList) SparksDlist.Clone();
            }
        }

        GameObject goLog = Instantiate(logPrefab) as GameObject;

        if (client != null && client.isHost)
        {
            goLog.transform.SetParent(logContainer1);
        }
        else if (client != null && !client.isHost)
        {
            goLog.transform.SetParent(logContainer2);
        }
        else if (!client)
        {
            goLog.transform.SetParent(logContainer1);
        }

        Instance = this;
        SpawnAllChessmans();

    }

    private void Update()
    {
        if (isWhite != isWhiteTurn)
        {
            selectionX = -1;
            selectionY = -1;
        }
        if (isWhiteTurn) {
            GameObject.Find("WhiteCandle").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("BlackCandle").transform.GetChild(0).gameObject.SetActive(false);
        } else {
            GameObject.Find("WhiteCandle").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("BlackCandle").transform.GetChild(0).gameObject.SetActive(true);
        }

        GameObject Canvas = HostDeck.transform.parent.parent.gameObject;
        GameObject PlayerCanvas = PlayerDeck.transform.parent.parent.gameObject;


        UpdateSelection();
        DrawChessboard();

        if (client)
        {
            if (isWhite && isWhiteTurn)
            {
                HelpMakeW.SetActive(true);
               if (HostNCbtn.activeSelf==false) { HCardCount = 0; HostNCbtn.SetActive(true);  }
             //   PCardCount = 1;
                for (int i = 0; i < HostDeck.transform.childCount; i++)
                { HostDeck.transform.GetChild(i).GetComponent<draggable>().enabled = true; }
            }
            else if (!isWhite && !isWhiteTurn)
            {
                HelpMakeB.SetActive(true);
                if (PlayerNCbtn.activeSelf == false) { PCardCount = 0; PlayerNCbtn.SetActive(true); }
                //   HCardCount = 1;
                for (int i = 0; i < PlayerDeck.transform.childCount; i++)
                    if (PlayerDeck.transform.GetChild(i) != null) { PlayerDeck.transform.GetChild(i).GetComponent<draggable>().enabled = true; }

            }
            else
            {
                HelpMakeW.SetActive(false);
                HelpMakeB.SetActive(false);
                //  HCardCount = 1; PCardCount = 1;
                PlayerNCbtn.SetActive(false);   HostNCbtn.SetActive(false);
                for (int i = 0; i < PlayerDeck.transform.childCount; i++)
                { PlayerDeck.transform.GetChild(i).GetComponent<draggable>().enabled = false; }

                for (int i = 0; i < HostDeck.transform.childCount; i++)
                { HostDeck.transform.GetChild(i).GetComponent<draggable>().enabled = false; }
                
            }
        }
        else
        {

        }

        if (client)
        {
            if (((isWhite && isWhiteTurn) || (!isWhite && !isWhiteTurn)) && Input.GetMouseButtonDown(0))
            {
                //if valid mouse input
                if ((selectionX >= 0) && (selectionY >= 0))
                {
                    if (selectedChessman == null)
                    {
                        //Chessman is not selected yet. So select Chessman
                        SelectChessman(selectionX, selectionY);
                    }
                    else
                    {
                        //Chessman is already selected. So move Chessman
                        MoveChessman(selectionX, selectionY);
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                //if valid mouse input
                if ((selectionX >= 0) && (selectionY >= 0))
                {
                    if (selectedChessman == null)
                    {
                        //Chessman is not selected yet. So select Chessman
                        SelectChessman(selectionX, selectionY);
                    }
                    else
                    {
                        //Chessman is already selected. So move Chessman
                        MoveChessman(selectionX, selectionY);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
            SendMessage();
        


    }

    private void SelectChessman(int x, int y)
    {
        //no available chessman in click position
        if (Chessmans[x, y] == null)
            return;

        //if not appropriate turn
        if (Chessmans[x, y].isWhite != isWhiteTurn)
            return;


        //get possible place to move of selected chessman
        allowedMoves = Chessmans[x, y].PossibleMove();              //IMPORTANT: set allowedMoves depends on type of chessman
        if (Chessmans[x, y].GetType() == typeof(King))
        {
            for (int i=0; i<8; i++){
                for (int j=0; j<8; j++){
                    if (isAttacked(i, j, Chessmans[x, y].isWhite))
                        allowedMoves[i, j] = false;
                }
            }

        }

        //if no available move, then don't allow to select it
        bool hasAtleastOneMove = false;
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (allowedMoves[i, j])
                    hasAtleastOneMove = true;
        if (!hasAtleastOneMove)
            return;


        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);

        //select chessman in position (x,y)
        selectedChessman = Chessmans[x, y];

    }

    public void UpdateChessman(string type, int prevX, int prevY, int x, int y)
    {
        if (type == "")
            return;

        Chessman prevMan = Chessmans[prevX, prevY];
        selectedChessman = Chessmans[prevX, prevY];
        switch (type)
        {
            case "rook":
                if (prevMan == null || prevMan.GetType() != typeof(Rook))
                    return;
                selectedChessman = Chessmans[prevX, prevY];                
                break;
            case "queen":
                if (prevMan == null || prevMan.GetType() != typeof(Queen))
                    return;
                selectedChessman = Chessmans[prevX, prevY];
                break;
            case "pawn":
                if (prevMan == null || prevMan.GetType() != typeof(Pawn))
                    return;
                selectedChessman = Chessmans[prevX, prevY];
                break;
            case "knight":
                if (prevMan == null || prevMan.GetType() != typeof(Knight))
                    return;
                selectedChessman = Chessmans[prevX, prevY];
                break;
            case "king":
                if (prevMan == null || prevMan.GetType() != typeof(King))
                    return;
                selectedChessman = Chessmans[prevX, prevY];
                break;
            case "bishop":
                if (prevMan == null || prevMan.GetType() != typeof(Bishop))
                    return;
                selectedChessman = Chessmans[prevX, prevY];
                break;
            default:
                return;
        }

        if ((selectedChessman.isWhite) && (selectedChessman.GetType() == typeof(Pawn)) && (x == TrapPosB[0]) && (y == TrapPosB[1]))
        {
            LogMessage(2, "Destroy", 0, 0, x, y, (selectedChessman.isWhite ? 1 : 0));
            ActiveChessman.Remove(selectedChessman.gameObject);
            Destroy(selectedChessman.gameObject);
            TrapPosB[0] = -1;
            TrapPosB[1] = -1;
            TrapPosB[2] = -1;
            selectedChessman = null;
            BoardHighlights.Instance.Hidehighlights();
            CardManager.Instance.TrapCard();
            playSound("EndTurn");
            isWhiteTurn = !isWhiteTurn;
            CardManager.Instance.cardUsed = false;
            refreshTrap();

            return;
        }

        if ((!selectedChessman.isWhite) && (selectedChessman.GetType() == typeof(Pawn)) && (x == TrapPosW[0]) && (y == TrapPosW[1]))
        {
            LogMessage(2, "Destroy", 0, 0, x, y, (selectedChessman.isWhite ? 1 : 0));
            ActiveChessman.Remove(selectedChessman.gameObject);
            Destroy(selectedChessman.gameObject);
            TrapPosW[0] = -1;
            TrapPosW[1] = -1;
            TrapPosW[2] = -1;
            selectedChessman = null;
            BoardHighlights.Instance.Hidehighlights();
            CardManager.Instance.TrapCard();
            playSound("EndTurn");
            isWhiteTurn = !isWhiteTurn;
            CardManager.Instance.cardUsed = false;
            refreshTrap();

            return;
        }

        Chessman c = Chessmans[x, y];                   //chessman that exist on place where selected chessman want to move
        if ((c != null) && c.isWhite != isWhiteTurn)    //if there is something in moving position, and it's enemy
        {
            if (c.GetType() == typeof(King))            //CHECK or CHECKMATE
            {
                EndGame();
                return;
            }

            ActiveChessman.Remove(c.gameObject);        //update active chessman array and delete it
            Destroy(c.gameObject);
        }

        //////////////EnPassant Attack//////////////////
        if ((x == EnPassantMove[0]) && (y == EnPassantMove[1]))
        {
            if (isWhiteTurn)     //white
                c = Chessmans[x, y - 1];                  //Pawn that could be attacked by EnPassant Move
            else                 //black
                c = Chessmans[x, y + 1];
            ActiveChessman.Remove(c.gameObject);
            Destroy(c.gameObject);
        }
        ////////////////////////////////////////////////


        EnPassantMove[0] = -1;  //clear
        EnPassantMove[1] = -1;  //clear
        if (selectedChessman.GetType() == typeof(Pawn))
        {
            ////if Pawn, Set EnPassant array. Only valid right after first move////
            if ((selectedChessman.CurrentY == 1) && (y == 3))
            {
                EnPassantMove[0] = x;
                EnPassantMove[1] = y - 1;
            }
            else if ((selectedChessman.CurrentY == 6) && (y == 4))
            {
                EnPassantMove[0] = x;
                EnPassantMove[1] = y + 1;
            }
            //////////////////////////////////////////////////////////////////////

        }

        /////////////////Promotion/////////////////
        if ((y == 7) && (selectedChessman.GetType() == typeof(Pawn)))             //White
        {
            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;     //original position is now cleared
            //selectedChessman.transform.position = GetTileCenter(x, y);                  //change position to (x,y)
            StartCoroutine(moveAnimation(selectedChessman.gameObject, GetTileCenter(x, y), 0.5f));
            selectedChessman.SetPosition(x, y);                                         //update current Position
            Chessmans[x, y] = selectedChessman;                                         //update Chessmans array info
            selectedChessman.Moved = true;
            return;
        }
        else if ((y == 0) && (selectedChessman.GetType() == typeof(Pawn)))        //Black
        {
            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;     //original position is now cleared
            //selectedChessman.transform.position = GetTileCenter(x, y);                  //change position to (x,y)
            StartCoroutine(moveAnimation(selectedChessman.gameObject, GetTileCenter(x, y), 0.5f));
            selectedChessman.SetPosition(x, y);                                         //update current Position
            Chessmans[x, y] = selectedChessman;                                         //update Chessmans array info
            selectedChessman.Moved = true;
            return;
        }
        ///////////////////////////////////////////

        if (selectedChessman.GetType() == typeof(Rook))
        {
            if (isWhiteTurn)
            {
                if (selectedChessman.CurrentX == 7) { CastleHasMoved[0] = true; }       //White KingSide Rook
                else if (selectedChessman.CurrentX == 0) { CastleHasMoved[1] = true; }  //White QueenSide Rook
            }
            else
            {
                if (selectedChessman.CurrentX == 7) { CastleHasMoved[2] = true; }       //Black KingSide Rook
                else if (selectedChessman.CurrentX == 0) { CastleHasMoved[3] = true; }  //Black QueenSide Rook
            }
        }


        if ((selectedChessman.GetType() == typeof(King)) && (Math.Abs(selectedChessman.CurrentX - x) == 2))   //if King is castling now
        {
            if (isWhiteTurn)
            {
                switch (x)
                {
                    case 6:             //WK
                        Chessman WK = Chessmans[7, 0];
                        Chessmans[7, 0] = null;
                        //WK.transform.position = GetTileCenter(5, 0);
                        StartCoroutine(moveAnimation(WK.gameObject, GetTileCenter(5, 0), 0.5f));
                        WK.SetPosition(5, 0);
                        Chessmans[5, 0] = WK;
                        WK.Moved = true;
                        break;
                    case 2:             //WQ
                        Chessman WQ = Chessmans[0, 0];
                        Chessmans[0, 0] = null;
                        //WQ.transform.position = GetTileCenter(3, 0);
                        StartCoroutine(moveAnimation(WQ.gameObject, GetTileCenter(3, 0), 0.5f));
                        WQ.SetPosition(3, 0);
                        Chessmans[3, 0] = WQ;
                        WQ.Moved = true;
                        break;
                }
            }
            else
            {
                switch (x)
                {
                    case 6:             //BK
                        Chessman BK = Chessmans[7, 7];
                        Chessmans[7, 7] = null;
                        //BK.transform.position = GetTileCenter(5, 7);
                        StartCoroutine(moveAnimation(BK.gameObject, GetTileCenter(5, 7), 0.5f));
                        BK.SetPosition(5, 7);
                        Chessmans[5, 7] = BK;
                        BK.Moved = true;
                        break;
                    case 2:             //BQ
                        Chessman BQ = Chessmans[0, 7];
                        Chessmans[0, 7] = null;
                        //BQ.transform.position = GetTileCenter(3, 7);
                        StartCoroutine(moveAnimation(BQ.gameObject, GetTileCenter(3, 7), 0.5f));
                        BQ.SetPosition(3, 7);
                        Chessmans[3, 7] = BQ;
                        BQ.Moved = true;
                        break;
                }
            }
        }

        Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;     //original position is now cleared
        //selectedChessman.transform.position = GetTileCenter(x, y);                  //change position to (x,y)
        StartCoroutine(moveAnimation(selectedChessman.gameObject, GetTileCenter(x, y), 1f));
        selectedChessman.SetPosition(x, y);                                         //update current Position
        Chessmans[x, y] = selectedChessman;                                         //update Chessmans array info

        /////////////////////////////CHECK/////////////////////////////////
        CheckW = false;
        CheckB = false;
        if (selectedChessman.GetType() == typeof(King))
        {
            if (isWhiteTurn) { WhiteKingPos[0] = x; WhiteKingPos[1] = y; }
            else { BlackKingPos[0] = x; BlackKingPos[1] = y; }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Chessmans[i, j] != null)
                {
                    attacking = Chessmans[i, j].PossibleMove();
                    if (Chessmans[i, j].isWhite && attacking[BlackKingPos[0], BlackKingPos[1]]) { StartCoroutine("CheckBTx"); ; CheckB = true; }
                    else if (!Chessmans[i, j].isWhite && attacking[WhiteKingPos[0], WhiteKingPos[1]]) { StartCoroutine("CheckWTx"); ; CheckW = true; }
                }
            }
        }
        ///////////////////////////////////////////////////////////////////

        TrapPosW[2] -= 1;
        TrapPosB[2] -= 1;
        refreshTrap();
        selectedChessman.Moved = true;                                              //This chessman has moved
        isWhiteTurn = !isWhiteTurn;                                                 //change turn
        CardManager.Instance.cardUsed = false;
        playSound("EndTurn");
        Debug.Log("updateChessman turn change");
        
        selectedChessman = null;                                                        //clear selected chessman


    }

    private void MoveChessman(int x, int y)
    {
        //if selected chessman is possible to move to position (x,y), move it to position and attack enemy
        if (allowedMoves[x, y])
        {
            if (client)
            {
                string msg = "CMOV|";
                msg += ParseType() + "|";
                msg += selectedChessman.CurrentX + "|";
                msg += selectedChessman.CurrentY + "|";
                msg += x + "|";
                msg += y + "|";
                msg += (isWhite ? 1 : 0);
                client.Send(msg);
            }
            else
            {
                LogMessage(0, ParseType(), selectedChessman.CurrentX, selectedChessman.CurrentY, x, y, (isWhiteTurn ? 1 : 0));
            }

            if ((selectedChessman.isWhite) && (selectedChessman.GetType() == typeof(Pawn)) && (x == TrapPosB[0]) && (y == TrapPosB[1]))
            {
                LogMessage(2, "Destroy", 0, 0, x, y, (selectedChessman.isWhite ? 1 : 0));
                ActiveChessman.Remove(selectedChessman.gameObject);
                Destroy(selectedChessman.gameObject);
                TrapPosB[0] = -1;
                TrapPosB[1] = -1;
                TrapPosB[2] = -1;
                selectedChessman = null;
                BoardHighlights.Instance.Hidehighlights();
                CardManager.Instance.TrapCard();
                isWhiteTurn = !isWhiteTurn;
                CardManager.Instance.cardUsed = false;
                playSound("EndTurn");
                refreshTrap();

                return;
            }

            if ((!selectedChessman.isWhite) && (selectedChessman.GetType() == typeof(Pawn)) && (x == TrapPosW[0]) && (y == TrapPosW[1]))
            {
                LogMessage(2, "Destroy", 0, 0, x, y, (selectedChessman.isWhite ? 1 : 0));
                ActiveChessman.Remove(selectedChessman.gameObject);
                Destroy(selectedChessman.gameObject);
                TrapPosW[0] = -1;
                TrapPosW[1] = -1;
                TrapPosW[2] = -1;
                selectedChessman = null;
                BoardHighlights.Instance.Hidehighlights();
                CardManager.Instance.TrapCard();
                isWhiteTurn = !isWhiteTurn;
                CardManager.Instance.cardUsed = false;
                playSound("EndTurn");
                refreshTrap();

                return;
            }


            Chessman c = Chessmans[x, y];                   //chessman that exist on place where selected chessman want to move
            if ((c != null) && c.isWhite != isWhiteTurn)    //if there is something in moving position, and it's enemy
            {
                if (c.GetType() == typeof(King))            //CHECK or CHECKMATE
                {
                    EndGame();
                    return;
                }

                ActiveChessman.Remove(c.gameObject);        //update active chessman array and delete it
                Destroy(c.gameObject);
                Debug.Log("Destroy");
            }

            //////////////EnPassant Attack//////////////////
            if ((x == EnPassantMove[0]) && (y == EnPassantMove[1]))
            {
                if (isWhiteTurn)     //white
                    c = Chessmans[x, y - 1];                  //Pawn that could be attacked by EnPassant Move
                else                 //black
                    c = Chessmans[x, y + 1];
                ActiveChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            ////////////////////////////////////////////////


            EnPassantMove[0] = -1;  //clear
            EnPassantMove[1] = -1;  //clear
            if (selectedChessman.GetType() == typeof(Pawn))
            {
                ////if Pawn, Set EnPassant array. Only valid right after first move////
                if ((selectedChessman.CurrentY == 1) && (y == 3))
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y - 1;
                }
                else if ((selectedChessman.CurrentY == 6) && (y == 4))
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y + 1;
                }
                //////////////////////////////////////////////////////////////////////


                /////////////////Promotion/////////////////
                if (y == 7)              //White
                {
                    StartCoroutine(Promotion(x, y, selectedChessman));
                    TrapPosW[2] -= 1;
                    TrapPosB[2] -= 1;
                    refreshTrap();

                    return;
                }
                else if (y == 0)        //Black
                {
                    StartCoroutine(Promotion(x, y, selectedChessman));
                    TrapPosW[2] -= 1;
                    TrapPosB[2] -= 1;
                    refreshTrap();

                    return;
                }
                ///////////////////////////////////////////
            }

            ////////////////checking Rook for Castling///////////////////
            if (selectedChessman.GetType() == typeof(Rook))
            {
                if (isWhiteTurn)
                {
                    if (selectedChessman.CurrentX == 7) { CastleHasMoved[0] = true; }       //White KingSide Rook
                    else if (selectedChessman.CurrentX == 0) { CastleHasMoved[1] = true; }  //White QueenSide Rook
                }
                else
                {
                    if (selectedChessman.CurrentX == 7) { CastleHasMoved[2] = true; }       //Black KingSide Rook
                    else if (selectedChessman.CurrentX == 0) { CastleHasMoved[3] = true; }  //Black QueenSide Rook
                }
            }
            //////////////////////////////////////////////////////////////


            /////////////////////////////////CASTLING////////////////////////////////////
            if ((selectedChessman.GetType() == typeof(King)) && (Math.Abs(selectedChessman.CurrentX - x) == 2))   //if King is castling now
            {
                if (isWhiteTurn)
                {
                    switch (x)
                    {
                        case 6:             //WK
                            Chessman WK = Chessmans[7, 0];
                            Chessmans[7, 0] = null;
                            //WK.transform.position = GetTileCenter(5, 0);
                            StartCoroutine(moveAnimation(WK.gameObject, GetTileCenter(5, 0), 0.5f));
                            WK.SetPosition(5, 0);
                            Chessmans[5, 0] = WK;
                            WK.Moved = true;
                            break;
                        case 2:             //WQ
                            Chessman WQ = Chessmans[0, 0];
                            Chessmans[0, 0] = null;
                            //WQ.transform.position = GetTileCenter(3, 0);
                            StartCoroutine(moveAnimation(WQ.gameObject, GetTileCenter(3, 0), 0.5f));
                            WQ.SetPosition(3, 0);
                            Chessmans[3, 0] = WQ;
                            WQ.Moved = true;
                            break;
                    }
                }
                else
                {
                    switch (x)
                    {
                        case 6:             //BK
                            Chessman BK = Chessmans[7, 7];
                            Chessmans[7, 7] = null;
                            //BK.transform.position = GetTileCenter(5, 7);
                            StartCoroutine(moveAnimation(BK.gameObject, GetTileCenter(5, 7), 0.5f));
                            BK.SetPosition(5, 7);
                            Chessmans[5, 7] = BK;
                            BK.Moved = true;
                            break;
                        case 2:             //BQ
                            Chessman BQ = Chessmans[0, 7];
                            Chessmans[0, 7] = null;
                            //BQ.transform.position = GetTileCenter(3, 7);
                            StartCoroutine(moveAnimation(BQ.gameObject, GetTileCenter(3, 7), 0.5f));
                            BQ.SetPosition(3, 7);
                            Chessmans[3, 7] = BQ;
                            BQ.Moved = true;
                            break;
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////////////////




            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;     //original position is now cleared
            //selectedChessman.transform.position = GetTileCenter(x, y);                  //change position to (x,y)
            StartCoroutine(moveAnimation(selectedChessman.gameObject, GetTileCenter(x, y), 0.5f));
            selectedChessman.SetPosition(x, y);                                         //update current Position
            Chessmans[x, y] = selectedChessman;                                         //update Chessmans array info
            selectedChessman.Moved = true;                                              //This chessman has moved



            /////////////////////////////CHECK/////////////////////////////////
            CheckW = false;
            CheckB = false;
            if (selectedChessman.GetType() == typeof(King))
            {
                if (isWhiteTurn) { WhiteKingPos[0] = x; WhiteKingPos[1] = y; }
                else { BlackKingPos[0] = x; BlackKingPos[1] = y; }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Chessmans[i, j] != null)
                    {
                        attacking = Chessmans[i, j].PossibleMove();
                        if (Chessmans[i, j].isWhite && attacking[BlackKingPos[0], BlackKingPos[1]]) { StartCoroutine("CheckBTx"); CheckB = true; }
                        else if (!Chessmans[i, j].isWhite && attacking[WhiteKingPos[0], WhiteKingPos[1]]) { StartCoroutine("CheckWTx"); CheckW = true; }
                    }
                }
            }
            ///////////////////////////////////////////////////////////////////

            TrapPosW[2] -= 1;
            TrapPosB[2] -= 1;
            refreshTrap();
            selectedChessman.Moved = true;                                              //This chessman has moved
            isWhiteTurn = !isWhiteTurn;                                                 //change turn
            CardManager.Instance.cardUsed = false;

            playSound("EndTurn");
            Debug.Log("moveChessman turn change");
        }

        BoardHighlights.Instance.Hidehighlights();                                      //after move, delete all highlights
        selectedChessman = null;                                                        //clear selected chessman

    }

    public IEnumerator Promotion(int x, int y, Chessman selectedChessman)
    {
        GameObject promotionUI = GameObject.FindWithTag("Canvas").transform.Find("PromotionUI").gameObject;

        promotionUI.SetActive(true);
        waitCondition = false;
        while (!waitCondition)
        {
            yield return null;
        }
        promotionUI.SetActive(false);

        Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;     //original position is now cleared

        ActiveChessman.Remove(selectedChessman.gameObject);
        Destroy(selectedChessman.gameObject);
        if (isWhite) { SpawnChessman(promotion, x, y); }
        else { SpawnChessman(promotion + 6, x, y); }
        selectedChessman = null;

        if (client)
        {
            string msg = "CPRO|";
            msg += promotion + "|";
            msg += x + "|";
            msg += y + "|";
            msg += (client.isHost ? 1 : 0);
            client.Send(msg);
            LogMessage(1, promotion.ToString(), x, y, 0, 0, (client.isHost ? 1 : 0));
            EndTurn();
        }
        else
        {
            LogMessage(1, promotion.ToString(), x, y, 0, 0, (isWhiteTurn ? 1 : 0));
            EndTurn();
        }

        BoardHighlights.Instance.Hidehighlights();
    }

    public void PromotionSelectR() { promotion = 2; waitCondition = true; }
    public void PromotionSelectKN() { promotion = 4; waitCondition = true; }
    public void PromotionSelectB() { promotion = 3; waitCondition = true; }
    public void PromotionSelectQ() { promotion = 1; waitCondition = true; }

    public void UpdatePromotion(int promotion, int x, int y)
    {
        TrapPosW[2] -= 1;
        TrapPosB[2] -= 1;
        refreshTrap();

        Chessman removedChessman = Chessmans[x, y];
        ActiveChessman.Remove(removedChessman.gameObject);
        Destroy(removedChessman.gameObject);

        if (!isWhite) { SpawnChessman(promotion, x, y); }
        else { SpawnChessman(promotion + 6, x, y); }

        playSound("EndTurn");
        isWhiteTurn = !isWhiteTurn;
        CardManager.Instance.cardUsed = false;
    }

    private void UpdateSelection()
    {
        //if (!Camera.main)
        //    return;

        if (isWhite)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25f, LayerMask.GetMask("ChessPlane")))
            {
                selectionX = (int)hit.point.x;
                selectionY = (int)hit.point.z;
            }
            else
            {
                selectionX = -1;
                selectionY = -1;
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(SubCamera.ScreenPointToRay(Input.mousePosition), out hit, 25f, LayerMask.GetMask("ChessPlane")))
            {
                selectionX = (int)hit.point.x;
                selectionY = (int)hit.point.z;
            }
            else
            {
                selectionX = -1;
                selectionY = -1;
            }
        }
    }

    public void SpawnChessman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x, y), chessmanPrefabs[index].transform.rotation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        ActiveChessman.Add(go);
    }

    private void SpawnAllChessmans()
    {
        ActiveChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        EnPassantMove = new int[2] { -1, -1 };

        // Spawn white team
        SpawnChessman(0, 4, 0);  //King
        SpawnChessman(1, 3, 0);  //Queen
        SpawnChessman(2, 0, 0);  //Rook
        SpawnChessman(2, 7, 0);  //Rook
        SpawnChessman(3, 2, 0);  //Bishop
        SpawnChessman(3, 5, 0);  //Bishop
        SpawnChessman(4, 1, 0);  //Knight
        SpawnChessman(4, 6, 0);  //Knight
        for (int i = 0; i < 8; i++)
            SpawnChessman(5, i, 1);  //Pawns
        WhiteKingPos[0] = 4;
        WhiteKingPos[1] = 0;

        // Spawn black team
        SpawnChessman(6, 4, 7);  //King
        SpawnChessman(7, 3, 7);  //Queen
        SpawnChessman(8, 0, 7);  //Rook
        SpawnChessman(8, 7, 7);  //Rook
        SpawnChessman(9, 2, 7);  //Bishop
        SpawnChessman(9, 5, 7);  //Bishop
        SpawnChessman(10, 1, 7);  //Knight
        SpawnChessman(10, 6, 7);  //Knight
        for (int i = 0; i < 8; i++)
            SpawnChessman(11, i, 6);  //Pawns
        BlackKingPos[0] = 4;
        BlackKingPos[1] = 7; 
    }

    IEnumerator moveAnimation(GameObject obj, Vector3 target, float overTime)
    {
        playSound("MovePiece");
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        obj.transform.position = target;
    }


    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        //Draw selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, Vector3.forward * selectionY + Vector3.right * selectionX);
            lineRenderer.SetPosition(1, Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));
        }

    }

    private void checkingCheck()
    {
        refreshKingPos();
        if (isAttacked(WhiteKingPos[0], WhiteKingPos[1], true)) { StartCoroutine("CheckWTx"); }
        if (isAttacked(BlackKingPos[0], BlackKingPos[1], false)) { StartCoroutine("CheckBTx"); }
    }

    private void refreshKingPos()
    {
        for (int i=0; i<8; i++){
            for (int j=0; j<8; j++){
                if ((Chessmans[i,j] != null) && (Chessmans[i,j].GetType() == typeof(King))){
                    if (Chessmans[i, j].isWhite == true) { WhiteKingPos[0] = i; WhiteKingPos[1] = j; }
                    else { BlackKingPos[0] = i; BlackKingPos[1] = j; }
                }
            }
        }
    }

    public void Swap(int a, int b, int x, int y)
    {
        Chessman tempChessman1 = Chessmans[a, b];
        Chessman tempChessman2 = Chessmans[x, y];

        Chessmans[a, b] = null;
        Chessmans[x, y] = null;
        //tempChessman1.transform.position = GetTileCenter(x, y);
        //tempChessman2.transform.position = GetTileCenter(a, b);
        StartCoroutine(moveAnimation(tempChessman1.gameObject, GetTileCenter(x, y), 0.5f));
        StartCoroutine(moveAnimation(tempChessman2.gameObject, GetTileCenter(a, b), 0.5f));
        tempChessman1.SetPosition(x, y);
        tempChessman2.SetPosition(a, b);
        Chessmans[x, y] = tempChessman1;
        Chessmans[a, b] = tempChessman2;
        tempChessman1.Moved = true;
        tempChessman2.Moved = true;

        checkingCheck();
    }

    public void Move(int a, int b, int x, int y)
    {
        Chessman tempChessman = Chessmans[a, b];
        Chessmans[a, b] = null;
        //tempChessman.transform.position = GetTileCenter(x, y);
        StartCoroutine(moveAnimation(tempChessman.gameObject, GetTileCenter(x, y), 0.5f));
        tempChessman.SetPosition(x, y);
        Chessmans[x, y] = tempChessman;
        tempChessman.Moved = true;

        checkingCheck();
    }

    public void Delete(int x, int y)
    {
        Chessman tempChessman = Chessmans[x, y];
        ActiveChessman.Remove(tempChessman.gameObject);
        Destroy(tempChessman.gameObject);
        checkingCheck();
    }

    public void SpawnTrap(int x, int y, int timer, bool White)
    {
        if (White)
        {
            if (client)
            {
                string msg = "CTRP|";
                msg += x + "|";
                msg += y + "|";
                msg += timer + "|";
                msg += 1;
                client.Send(msg);
            }
            else
            {
                TrapPosW[0] = x;
                TrapPosW[1] = y;
                TrapPosW[2] = timer;

            }
        } else
        {
            if (client)
            {
                string msg = "CTRP|";
                msg += x + "|";
                msg += y + "|";
                msg += timer + "|";
                msg += 0;
                client.Send(msg);
            }
            else
            {
                TrapPosB[0] = x;
                TrapPosB[1] = y;
                TrapPosB[2] = timer;
            }
        }
    }

    public void UpdateTrap(int x, int y, int timer, int White)
    {
        if ((White == 1 ? true : false))
        {
            TrapPosW[0] = x;
            TrapPosW[1] = y; 
            TrapPosW[2] = timer;
            if (isWhite) {
                trapInstantW = Instantiate(Trap, GetTileCenter(x, y) + new Vector3(0, 0.005f, 0), Quaternion.identity);
            }
        }
        else
        {
            TrapPosB[0] = x;
            TrapPosB[1] = y;
            TrapPosB[2] = timer;
            if (!isWhite)
            {
                trapInstantB = Instantiate(Trap, GetTileCenter(x, y) + new Vector3(0, 0.005f, 0), Quaternion.identity);
            }
        }

        //EndTurn();
        CardManager.Instance.cardUsed = true;
        Debug.Log("WTrap: " + TrapPosW[0] + ", " + TrapPosW[1] + ", " + TrapPosW[2]);
        Debug.Log("BTrap: " + TrapPosB[0] + ", " + TrapPosB[1] + ", " + TrapPosB[2]);
    }


    private void refreshTrap()
    {
        if (TrapPosW[2] <= 0)
        {
            TrapPosW[0] = -1;
            TrapPosW[1] = -1;
            TrapPosW[2] = -1;
            Destroy(trapInstantW);
        }
        if (TrapPosB[2] <= 0)
        {
            TrapPosB[0] = -1;
            TrapPosB[1] = -1;
            TrapPosB[2] = -1;
            Destroy(trapInstantB);
        }
    }

    public bool isAttacked(int x, int y, bool White)
    {
        if (White)
        {
            for (int i = 0; i < 8; i++){
                for (int j = 0; j < 8; j++){
                    if ((Chessmans[i, j] != null) && (!Chessmans[i, j].isWhite))
                    {
                        attacking = Chessmans[i, j].PossibleMove2();
                        if ((Chessmans[i, j].GetType() == typeof(Pawn)) && (x == i-1) && (y == j-1)) { attacking[x, y] = true; }
                        if ((Chessmans[i, j].GetType() == typeof(Pawn)) && (x == i+1) && (y == j-1)) { attacking[x, y] = true; }
                        if ((Chessmans[i, j].GetType() == typeof(Pawn)) && (x == i) && (y == j - 1)) { attacking[x, y] = false; }
                        if (attacking[x, y]) { Debug.Log("Black " +Chessmans[i,j].GetType()+" is attacking (" + x + ", " + y + ") position"); return true; }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 8; i++){
                for (int j = 0; j < 8; j++){
                    if ((Chessmans[i, j] != null) && (Chessmans[i, j].isWhite))
                    {
                        attacking = Chessmans[i, j].PossibleMove2();
                        if ((Chessmans[i, j].GetType() == typeof(Pawn)) && (x == i-1) && (y == j+1)) { attacking[x, y] = true; }
                        if ((Chessmans[i, j].GetType() == typeof(Pawn)) && (x == i+1) && (y == j+1)) { attacking[x, y] = true; }
                        if ((Chessmans[i, j].GetType() == typeof(Pawn)) && (x == i) && (y == j + 1)) { attacking[x, y] = false; }
                        if (attacking[x, y]) { Debug.Log("White " + Chessmans[i, j].GetType() + "  is attacking (" + x + ", " + y + ") position"); return true; }
                    }
                }
            }
        }
        return false;
    }

    void playSound(string snd)
    {
        GameObject.Find(snd).GetComponent<AudioSource>().Play();
    }


    public void ChatMessage(string msg)
    {
        GameObject go = Instantiate(messagePrefab) as GameObject;
        if (client != null && client.isHost)
        {
            go.transform.SetParent(chatMessageContainer1);
        }
        else if (client != null && !client.isHost)
        {
            go.transform.SetParent(chatMessageContainer2);
        }
        
        go.GetComponentInChildren<Text>().text = msg;
    }

    public void LogMessage(int act, string type, int prevX, int prevY, int x, int y, int isWhite)
    {
        GameObject goLog = Instantiate(logPrefab) as GameObject;

        if (client != null && client.isHost)
        {
            goLog.transform.SetParent(logContainer1);
        }
        else if (client != null && !client.isHost)
        {
            goLog.transform.SetParent(logContainer2);
        }
        else if (!client)
        {
            goLog.transform.SetParent(logContainer1);
        }

        Text[] newText = goLog.GetComponentsInChildren<Text>();

        switch (act)
        {
            case 0:
                newText[0].text = ((isWhite == 0) ? "Black" : "White");
                newText[1].text = "Moved " + type + ":";
                newText[2].text = "(" + prevX + ", " + prevY + ") to (" + x + ", " + y + ")";
                break;
            case 1:
                string promotedMan = "";
                int promotion = int.Parse(type);
                if (promotion > 6)
                    promotion -= 6;
                switch (promotion)
                {
                    case 1:
                        promotedMan = "queen";
                        break;
                    case 2:
                        promotedMan = "rook";
                        break;
                    case 3:
                        promotedMan = "bishop";
                        break;
                    case 4:
                        promotedMan = "knight";
                        break;
                }
                newText[0].text = ((isWhite == 0) ? "Black" : "White");
                newText[1].text = "Promoted to " + promotedMan + ":";
                newText[2].text = "at (" + prevX + ", " + prevY + ")";
                break;
            case 2:
                newText[0].text = ((isWhite == 0) ? "Black" : "White");
                switch (type)
                {
                    case "Reinforcements":
                        newText[1].text = "증원군:";
                        newText[2].text = "Pawn is spawned at (" + x + ", " + y + ")";
                        break;
                    case "Chivalry":
                        newText[1].text = "기사도:";
                        newText[2].text = "Swapped (" + prevX + ", " + prevY +") with (" + x + ", " + y + ")";
                        break;
                    case "Reflection":
                        newText[1].text = "반전:";
                        newText[2].text = "Bishop moved from (" + prevX + ", " + prevY + ") to (" + x + ", " + y + ")";
                        break;
                    case "Defense":
                        newText[1].text = "방어:";
                        newText[2].text = "Rook moved from (" + prevX + ", " + prevY + ") to (" + x + ", " + y + ")";
                        break;
                    case "Counter":
                        newText[1].text = "반격:";
                        newText[2].text = "Pawn moved from (" + prevX + ", " + prevY + ") to (" + x + ", " + y + ")";
                        break;
                    case "Sacrifice":
                        newText[1].text = "희생:";
                        newText[2].text = "Swapped (" + prevX + ", " + prevY + ") with (" + x + ", " + y + ")";
                        break;
                    case "DoOrDie":
                        newText[1].text = "승부수:";
                        newText[2].text = "Swapped (" + prevX + ", " + prevY + ") with (" + x + ", " + y + ")";
                        break;
                    case "Ambush":
                        newText[1].text = "매복:";
                        newText[2].text = "Trap set at (" + x + ", " + y + ")";
                        break;
                    case "Destroy":
                        newText[1].text = "매복:";
                        newText[2].text = "Trap activated at (" + x + ", " + y + "). " + newText[0].text + "'s pawn is caught.";
                        break;
                    case "Repulse":
                        newText[1].text = "격퇴:";
                        newText[2].text = ((isWhite == 0) ? "White" : "Black") + "'s Queen moved from (" + prevX + ", " + prevY + ") to (" + x + ", " + y + ")";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    public void LogMessage3(int act, string type, int prevX1, int prevY1, int prevX2, int prevY2, int x, int y, int isWhite)
    {
        GameObject goLog = Instantiate(logPrefab) as GameObject;

        if (client != null && client.isHost)
        {
            goLog.transform.SetParent(logContainer1);
        }
        else if (client != null && !client.isHost)
        {
            goLog.transform.SetParent(logContainer2);
        }
        else if (!client)
        {
            goLog.transform.SetParent(logContainer1);
        }

        Text[] newText = goLog.GetComponentsInChildren<Text>();

        switch (act)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                newText[0].text = ((isWhite == 0) ? "Black" : "White");
                switch (type)
                {
                    case "Medal":
                        newText[1].text = "명예훈장:";
                        newText[2].text = "Knight is spawned at (" + x + ", " + y + ") by sacrificing two pawns";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    public IEnumerator CheckWTx()
    {
        GameObject goLog = Instantiate(logPrefab) as GameObject;

        if (client != null && client.isHost)
        {
            goLog.transform.SetParent(logContainer1);
        }
        else if (client != null && !client.isHost)
        {
            goLog.transform.SetParent(logContainer2);
        }
        else if (!client)
        {
            goLog.transform.SetParent(logContainer1);
        }

        Text[] newText = goLog.GetComponentsInChildren<Text>();
        newText[0].text = "White";
        newText[1].text = "King is in Check:";
        newText[2].text = "at (" + WhiteKingPos[0] + ", " + WhiteKingPos[1] + ")";

        playSound("CheckSound");

        GameObject Tx = GameObject.Find("CheckUI").transform.Find("WhiteCheckTx").gameObject;
        Tx.SetActive(true);
        yield return new WaitForSeconds(3f);
        Tx.SetActive(false);
    }

    public IEnumerator CheckBTx()
    {
        GameObject goLog = Instantiate(logPrefab) as GameObject;

        if (client != null && client.isHost)
        {
            goLog.transform.SetParent(logContainer1);
        }
        else if (client != null && !client.isHost)
        {
            goLog.transform.SetParent(logContainer2);
        }
        else if (!client)
        {
            goLog.transform.SetParent(logContainer1);
        }

        Text[] newText = goLog.GetComponentsInChildren<Text>();
        newText[0].text = "Black";
        newText[1].text = "King is in Check:";
        newText[2].text = "at (" + BlackKingPos[0] + ", " + BlackKingPos[1] + ")";

        playSound("CheckSound");

        GameObject Tx = GameObject.Find("CheckUI").transform.Find("BlackCheckTx").gameObject;
        Tx.SetActive(true);
        yield return new WaitForSeconds(3f);
        Tx.SetActive(false);
    }

    public void SendMessage()
    {
        InputField i = GameObject.Find("MessageInput").GetComponent<InputField>();

        if (!client || i.text == "")
            return;

        client.Send("CMSG|" + i.text);
        i.text = "";
    }

    private string ParseType()
    {
        if (selectedChessman != null)
        {
            if (selectedChessman.GetType() == typeof(Rook))
            {
                return "rook";
            }
            else if (selectedChessman.GetType() == typeof(Queen))
            {
                return "queen";
            }
            else if (selectedChessman.GetType() == typeof(Pawn))
            {
                return "pawn";
            }
            else if (selectedChessman.GetType() == typeof(Knight))
            {
                return "knight";
            }
            else if (selectedChessman.GetType() == typeof(King))
            {
                return "king";
            }
            else if (selectedChessman.GetType() == typeof(Bishop))
            {
                return "bishop";
            }
        }
        return "";
    }

    public void EndTurn()
    {
        TrapPosW[2] -= 1;
        TrapPosB[2] -= 1;
        refreshTrap();

        BoardHighlights.Instance.Hidehighlights();
        selectedChessman = null;
        isWhiteTurn = !isWhiteTurn;
        CardManager.Instance.cardUsed = false;
        playSound("EndTurn");
        Debug.Log("EndTurn turn change");
    }

    private void EndGame()
    {
        GameObject goLog = Instantiate(logPrefab) as GameObject;

        if (client != null && client.isHost)
        {
            goLog.transform.SetParent(logContainer1);
        }
        else if (client != null && !client.isHost)
        {
            goLog.transform.SetParent(logContainer2);
        }
        else if (!client)
        {
            goLog.transform.SetParent(logContainer1);
        }

        Text[] newText = goLog.GetComponentsInChildren<Text>();

        if (client)
        {
            if ((isWhite && isWhiteTurn) || (!isWhite && !isWhiteTurn))
            {
                newText[0].text = isWhite ? "White" : "Black";
                newText[1].text = "You wins!";
                newText[2].text = "Congratulations!";

                if (client.isHost)
                {
                    startMenu1.SetActive(true);
                }
                else
                {
                    startMenu2.SetActive(true);
                }
                GameObject.Find("MainTheme").GetComponent<AudioSource>().Stop();
                playSound("Victory");
                playSound("Victory2");
                GameObject.Find("WinText").GetComponent<Text>().text = "You wins!";

                new LogEventRequest().SetEventKey("WIN_GAME")
                        .Send((res) =>
                        {
                            if (!res.HasErrors)
                            {
                                GameSparksManager.instance.userEXP += 10;
                                GameSparksManager.instance.userGold += 100;
                            }
                            else
                            {
                                Debug.Log("Update Win Error: " + res.Errors.JSON.ToString());
                                GameSparksManager.instance.userEXP += 10;
                                GameSparksManager.instance.userGold += 100;
                            }
                        });
            }
            else
            {
                newText[0].text = isWhite ? "White" : "Black";
                newText[1].text = "Defeated.";
                newText[2].text = "You've done well.";

                if (client.isHost)
                {
                    startMenu1.SetActive(true);
                }
                else
                {
                    startMenu2.SetActive(true);
                }
                GameObject.Find("MainTheme").GetComponent<AudioSource>().Stop();
                playSound("Defeat");
                playSound("Defeat2");
                GameObject.Find("WinText").GetComponent<Text>().text = "Defeated.";

                if (GameSparksManager.instance.userGold >= 100)
                {
                    new LogEventRequest().SetEventKey("LOSE_GAME")
                            .Send((res) =>
                            {
                                if (!res.HasErrors)
                                {
                                    GameSparksManager.instance.userEXP += 5;
                                    GameSparksManager.instance.userGold -= 100;
                                }
                                else
                                {
                                    Debug.Log("Update Lose Error: " + res.Errors.JSON.ToString());
                                    GameSparksManager.instance.userEXP += 5;
                                    GameSparksManager.instance.userGold -= 100;
                                }
                            });
                }
            }
        }
        else
        {
            if (isWhiteTurn)
            {
                Debug.Log("White wins");
                newText[0].text = "White";
                newText[1].text = "White wins!";
                newText[2].text = "Congratulations!";
                startMenu1.SetActive(true);
                GameObject.Find("WinText").GetComponent<Text>().text = "White wins!";
            }
            else
            {
                Debug.Log("Black wins");
                newText[0].text = "Black";
                newText[1].text = "Black wins!";
                newText[2].text = "Congratulations!";
                startMenu1.SetActive(true);
                GameObject.Find("WinText").GetComponent<Text>().text = "Black wins!";
            }
            GameObject.Find("MainTheme").GetComponent<AudioSource>().Stop();
            playSound("Victory");
            playSound("Victory2");
        }
        GameObject.Find("RematchButton").GetComponentInChildren<Text>().text = "Rematch";

        foreach (GameObject go in ActiveChessman)
            Destroy(go);

        isWhiteTurn = true;
        BoardHighlights.Instance.Hidehighlights();
        GameObject[] clones = GameObject.FindGameObjectsWithTag("EditorOnly");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }

        // Need waiting scene to start over
        // Invoke("RestartGame", 0.5f);
    }

    public void RestartGame()
    {
        //GameObject goLogStart = Instantiate(logPrefab) as GameObject;

        //if (client != null && client.isHost)
        //{
        //    startMenu1.SetActive(false);
        //    goLogStart.transform.SetParent(logContainer1);
        //}
        //else if (client != null && !client.isHost)
        //{
        //    startMenu2.SetActive(false);
        //    goLogStart.transform.SetParent(logContainer2);
        //}
        //else if (!client)
        //{
        //    startMenu1.SetActive(false);
        //    goLogStart.transform.SetParent(logContainer1);
        //}
        //SpawnAllChessmans();

        SceneManager.LoadScene("ChessGame");
    }

    public void QuitButton()
    {
        if (client != null && !client.isHost)
        {
            quitMenu2.SetActive(true);
        }
        else
        {
            quitMenu1.SetActive(true);
        }
    }

    public void CancelButton()
    {
        if (client != null && !client.isHost)
        {
            quitMenu2.SetActive(false);
        }
        else
        {
            quitMenu1.SetActive(false);
        }
    }

    public void YesButton()
    {
        if (client)
        {
            string msg = "CQUT|";
            client.Send(msg);
            client.players.Clear();
            SceneManager.LoadScene("Menu");
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void RestartButton()
    {
        if (client)
        {
            string msg = "CRMT|";
            client.Send(msg);
            GameObject.Find("RematchButton").GetComponentInChildren<Text>().text = "Waiting...";
        }
        else
        {
            RestartGame();
        }
    }
}

/*
Debug.DrawLine(
    Vector3.forward * selectionY + Vector3.right * selectionX,
    Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));
Debug.DrawLine(
    Vector3.forward * (selectionY+1) + Vector3.right * selectionX,
    Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
*/
