using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { set; get; }

    public GameObject NewCard;
    public GameObject CardObjectBl;
    public GameObject CardObjectG;
    public GameObject CardObjectBr;

    BoardManager BM;
    BoardHighlights BH;
    draggable DR;
    GameObject ShowCard;
    Chessman[,] chessmans;
    bool[,] allowedPosition { set; get; }
    //private Chessman selectedChessman;

    private int selectionX = -1;
    private int selectionY = -1;

    public Camera MainCamera;
    public Camera SubCamera;
    bool isWhite;
    bool isWhiteTurn;

    public bool cardUsed;

    void Start()
    {
        Instance = this;
        BM = GameObject.FindWithTag("ChessBoard").GetComponent<BoardManager>();
        BH = GameObject.FindWithTag("ChessBoard").GetComponent<BoardHighlights>();
        isWhite = BM.isWhite;
        cardUsed = false;
    }

    void Update()
    {
        chessmans = BM.Chessmans;
        isWhiteTurn = BM.isWhiteTurn;
        UpdateSelection();
    }

    public void ActiveCard(int cardName)
    {
        BM.selectedChessman = null;
        BH.Hidehighlights();
        GameObject.Find("CardActive").GetComponent<AudioSource>().Play();
        DR = GameObject.Find("ShowCard").transform.GetChild(0).GetComponent<draggable>();
        ShowCard = GameObject.Find("ShowCard").transform.GetChild(0).gameObject;
        if (cardUsed)
            DR.CheckTurnEnd("fail", ShowCard);
        switch (cardName)
        {
            case 0:
                Debug.Log("Reinforcements: 소환할 위치를 클릭해주세요");
                StartCoroutine("ReinforcementsCor");
                break;
            case 1:
                Debug.Log("Chivalry: 위치를 바꿀 Knight를 클릭해주세요");
                StartCoroutine("ChivalryCor");
                break;
            case 2:
                Debug.Log("Reflection: 이동할 Bishop과 위치를 선택해주세요");
                StartCoroutine("ReflectionCor");
                break;
            case 3:
                Debug.Log("Medal: 희생할 Pawn 2개와 Knight를 소환할 위치를 선택해주세요");
                StartCoroutine("MedalCor");
                break;
            case 4:
                Debug.Log("Defense: 이동할 Rook과 위치를 선택해주세요");
                StartCoroutine("DefenseCor");
                break;
            case 5:
                Debug.Log("Counter: 이동할 Pawn과 위치를 선택해주세요");
                StartCoroutine("CounterCor");
                break;
            case 6:
                Debug.Log("Sacrifice: 이동할 Pawn을 선택해주세요");
                StartCoroutine("SacrificeCor");
                break;
            case 7:
                Debug.Log("DoOrDie: 스왑할 자신의 Knight와 상대의 Knight를 선택해주세요");
                StartCoroutine("DoOrDieCor");
                break;
            case 8:
                Debug.Log("Ambush: 전장에 함정을 설치할 위치를 선택해주세요");
                StartCoroutine("AmbushCor");
                break;
            case 9:
                Debug.Log("Repulse: 뒤로 이동시킬 상대의 Queen을 선택해주세요");
                StartCoroutine("RepulseCor");
                break;
        }
    }

    private IEnumerator ReinforcementsCor()
    {
        BH.Hidehighlights();
        allowedPosition = new bool[8, 8];
        if (isWhiteTurn)
        {
            for (int i = 0; i < 8; i++){
                if (chessmans[i, 1] == null) { allowedPosition[i, 1] = true; }
                else { allowedPosition[i, 1] = false; }
            }
        } else {
            for (int i = 0; i < 8; i++){
                if (chessmans[i, 6] == null) { allowedPosition[i, 6] = true; }
                else { allowedPosition[i, 6] = false; }
            }
        }
        BH.HighlightAllowedMoves(allowedPosition);

        bool noPos = false;
        for (int k=0; k<8; k++)
        {
            if (allowedPosition[k, 1] || allowedPosition[k, 6])
                noPos = true;
        }
        if (!noPos) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true))
            {
                BH.Hidehighlights();
                if (selectionY == 1)
                {
                    BM.SpawnChessman(5, selectionX, selectionY);
                    DR.CheckTurnEnd("pass", ShowCard);
                    //BM.EndTurn();
                    cardUsed = true;
                    if (BM.client)
                    {
                        string msg = "CCRD|Reinforcements|";
                        msg += 0 + "|";
                        msg += 0 + "|";
                        msg += selectionX + "|";
                        msg += selectionY + "|";
                        msg += (BM.client.isHost ? 1 : 0);
                        BM.client.Send(msg);
                    }
                    BM.LogMessage(2, "Reinforcements", 0, 0, selectionX, selectionY, (BM.isWhite ? 1 : 0));
                    break;
                }
                else
                {
                    BM.SpawnChessman(11, selectionX, selectionY);
                    DR.CheckTurnEnd("pass", ShowCard);
                    //BM.EndTurn();
                    cardUsed = true;
                    if (BM.client)
                    {
                        string msg = "CCRD|Reinforcements|";
                        msg += 0 + "|";
                        msg += 0 + "|";
                        msg += selectionX + "|";
                        msg += selectionY + "|";
                        msg += (BM.client.isHost ? 1 : 0);
                        BM.client.Send(msg);
                    }
                    BM.LogMessage(2, "Reinforcements", 0, 0, selectionX, selectionY, (BM.isWhite ? 1 : 0));
                    break;
                }
            }
            else { yield return null; }
        }

        

        yield return null;
    }

    private IEnumerator ChivalryCor()
    {
        BH.Hidehighlights();
        allowedPosition = new bool[8, 8];

        for (int i = 0; i < 8; i++) {
            for (int j=0; j<8; j++) {
                if ((chessmans[i, j]!=null) && (chessmans[i, j].GetType() == typeof(Knight)) && (chessmans[i,j].isWhite == isWhiteTurn))
                    allowedPosition[i, j] = true; 
                else
                    allowedPosition[i, j] = false; 
            }
        }

        BH.HighlightAllowedMoves(allowedPosition);

        bool noPos = false;
        for (int k = 0; k < 8; k++) {
            for (int t=0; t<8; t++){
                if (allowedPosition[k, t])
                    noPos = true;
            }
        }
        if (!noPos) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true))
            {
                BH.Hidehighlights();
                if (isWhiteTurn)
                {
                    int[] wpos = BM.WhiteKingPos;
                    if (BM.client)
                    {
                        string msg = "CCRD|Chivalry|";
                        msg += wpos[0] + "|";
                        msg += wpos[1] + "|";
                        msg += selectionX + "|";
                        msg += selectionY + "|";
                        msg += (BM.client.isHost ? 1 : 0);
                        BM.client.Send(msg);
                    }
                    BM.LogMessage(2, "Chivalry", wpos[0], wpos[1], selectionX, selectionY, (BM.isWhite ? 1 : 0));
                    BM.Swap(selectionX, selectionY, wpos[0], wpos[1]);
                    BM.WhiteKingPos[0] = selectionX;
                    BM.WhiteKingPos[1] = selectionY;
                    DR.CheckTurnEnd("pass", ShowCard);
                    //BM.EndTurn();
                    cardUsed = true;
                    break;
                }
                else
                {
                    int[] bpos = BM.BlackKingPos;
                    if (BM.client)
                    {
                        string msg = "CCRD|Chivalry|";
                        msg += bpos[0] + "|";
                        msg += bpos[1] + "|";
                        msg += selectionX + "|";
                        msg += selectionY + "|";
                        msg += (BM.client.isHost ? 1 : 0);
                        BM.client.Send(msg);
                    }
                    BM.LogMessage(2, "Chivalry", bpos[0], bpos[1], selectionX, selectionY, (BM.isWhite ? 1 : 0));
                    BM.Swap(selectionX, selectionY, bpos[0], bpos[1]);
                    BM.BlackKingPos[0] = selectionX;
                    BM.BlackKingPos[1] = selectionY;
                    DR.CheckTurnEnd("pass", ShowCard);
                    //BM.EndTurn();
                    cardUsed = true;
                    break;
                }
            }
            else { yield return null; }
        }

        yield return null;
    }

    private IEnumerator ReflectionCor()
    {
        BH.Hidehighlights();
        allowedPosition = new bool[8, 8];

        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if ((chessmans[i, j] != null) && (chessmans[i, j].GetType() == typeof(Bishop)) && (chessmans[i, j].isWhite == isWhiteTurn))
                    allowedPosition[i, j] = true;
                else
                    allowedPosition[i, j] = false;
            }
        }

        BH.HighlightAllowedMoves(allowedPosition);

        bool noPos = false;
        for (int k = 0; k < 8; k++){
            for (int t = 0; t < 8; t++){
                if (allowedPosition[k, t])
                    noPos = true;
            }
        }
        if (!noPos) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true))
            {
                BH.Hidehighlights();
                Chessman selectedBishop = chessmans[selectionX, selectionY];
                int x = selectedBishop.CurrentX;
                int y = selectedBishop.CurrentY;

                bool[,] bishopMove = new bool[8, 8];

                if ((x > 0) && (chessmans[x - 1, y] == null)) { bishopMove[x - 1, y] = true; }
                if ((x < 7) && (chessmans[x + 1, y] == null)) { bishopMove[x + 1, y] = true; }

                BH.HighlightAllowedMoves(bishopMove);

                bool noMov = false;
                for (int k = 0; k < 8; k++){
                    for (int t = 0; t < 8; t++){
                        if (bishopMove[k, t])
                            noMov = true;
                    }
                }
                if (!noMov) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

                while (true) {
                    if (Input.GetMouseButtonDown(0) && (bishopMove[selectionX, selectionY] == true)){
                        BH.Hidehighlights();

                        BM.Move(x, y, selectionX, selectionY);
                        DR.CheckTurnEnd("pass", ShowCard);
                        cardUsed = true;
                        BM.EndTurn();
                        if (BM.client)
                        {
                            string msg = "CCRD|Reflection|";
                            msg += x + "|";
                            msg += y + "|";
                            msg += selectionX + "|";
                            msg += selectionY + "|";
                            msg += (BM.client.isHost ? 1 : 0);
                            BM.client.Send(msg);  
                        }
                        BM.LogMessage(2, "Reflection", x, y, selectionX, selectionY, (BM.isWhite ? 1 : 0));
                        break;

                    }
                    else { yield return null; }
                }
                break;
            }
            else { yield return null; }
        }

        yield return null;
    }

    private IEnumerator MedalCor()
    {
        BH.Hidehighlights();
        allowedPosition = new bool[8, 8];

        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if ((chessmans[i, j] != null) && (chessmans[i, j].GetType() == typeof(Pawn)) && (chessmans[i, j].isWhite == isWhiteTurn))
                    allowedPosition[i, j] = true;
                else
                    allowedPosition[i, j] = false;
            }
        }

        BH.HighlightAllowedMoves(allowedPosition);

        int noPos = 0;
        for (int k = 0; k < 8; k++){
            for (int t = 0; t < 8; t++){
                if (allowedPosition[k, t])
                    noPos += 1;
            }
        }
        if (noPos < 2) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true))
            {
                Chessman selectedPawn1 = chessmans[selectionX, selectionY];
                int x1 = selectedPawn1.CurrentX;
                int y1 = selectedPawn1.CurrentY;
                allowedPosition[x1, y1] = false;
                BH.Hidehighlights();
                BH.HighlightAllowedMoves(allowedPosition);

                while (true)
                {
                    if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true))
                    {
                        BH.Hidehighlights();
                        Chessman selectedPawn2 = chessmans[selectionX, selectionY];
                        int x2 = selectedPawn2.CurrentX;
                        int y2 = selectedPawn2.CurrentY;

                        BH.Hidehighlights();


                        bool[,] knightSpawn = new bool[8, 8];
                        for (int k = 0; k < 8; k++){
                            for (int t = 0; t < 8; t++){
                                if (isWhiteTurn && (chessmans[k, 0] == null)) { knightSpawn[k, 0] = true; }
                                else if (!isWhiteTurn && (chessmans[k, 7] == null)) { knightSpawn[k, 7] = true; }
                                else knightSpawn[k, t] = false;
                            }
                        }

                        BH.HighlightAllowedMoves(knightSpawn);
                        bool noMov = false;
                        for (int k = 0; k < 8; k++){
                            for (int t = 0; t < 8; t++){
                                if (knightSpawn[k, t])
                                    noMov = true;
                            }
                        }
                        if (!noMov) { DR.CheckTurnEnd("fail", ShowCard); yield break; }


                        while (true)
                        {
                            if (Input.GetMouseButtonDown(0) && (knightSpawn[selectionX, selectionY] == true))
                            {
                                BH.Hidehighlights();
                                if (isWhiteTurn)
                                {
                                    BM.Delete(x1, y1);
                                    BM.Delete(x2, y2);
                                    BM.SpawnChessman(4, selectionX, selectionY);
                                    DR.CheckTurnEnd("pass", ShowCard);
                                    //BM.EndTurn();
                                    cardUsed = true;
                                    if (BM.client)
                                    {
                                        string msg = "CCRD3|Medal|";
                                        msg += x1 + "|";
                                        msg += y1 + "|";
                                        msg += x2 + "|";
                                        msg += y2 + "|";
                                        msg += selectionX + "|";
                                        msg += selectionY + "|";
                                        msg += (BM.client.isHost ? 1 : 0);
                                        BM.client.Send(msg);
                                    }
                                    BM.LogMessage3(2, "Medal", x1, y1, x2, y2, selectionX, selectionY, (BM.isWhite ? 1 : 0));
                                } else{
                                    BM.Delete(x1, y1);
                                    BM.Delete(x2, y2);
                                    BM.SpawnChessman(10, selectionX, selectionY);
                                    DR.CheckTurnEnd("pass", ShowCard);
                                    //BM.EndTurn();
                                    cardUsed = true;
                                    if (BM.client)
                                    {
                                        string msg = "CCRD3|Medal|";
                                        msg += x1 + "|";
                                        msg += y1 + "|";
                                        msg += x2 + "|";
                                        msg += y2 + "|";
                                        msg += selectionX + "|";
                                        msg += selectionY + "|";
                                        msg += (BM.client.isHost ? 1 : 0);
                                        BM.client.Send(msg);
                                    }
                                    BM.LogMessage3(2, "Medal", x1, y1, x2, y2, selectionX, selectionY, (BM.isWhite ? 1 : 0));
                                }
                                break;

                            }
                            else { yield return null; }

                        }
                    }
                    else { yield return null; }
                }
            }
            else { yield return null; }
        }
    }

    private IEnumerator DefenseCor()
    {
        BH.Hidehighlights();
        allowedPosition = new bool[8, 8];

        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if ((chessmans[i, j] != null) && (chessmans[i, j].GetType() == typeof(Rook)) && (chessmans[i, j].isWhite == isWhiteTurn) && (j > 1) && (j < 6))
                    allowedPosition[i, j] = true;
                else
                    allowedPosition[i, j] = false;
            }
        }

        BH.HighlightAllowedMoves(allowedPosition);

        bool noPos = false;
        for (int k = 0; k < 8; k++){
            for (int t = 0; t < 8; t++){
                if (allowedPosition[k, t])
                    noPos = true;
            }
        }
        if (!noPos) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true))
            {
                BH.Hidehighlights();
                Chessman selectedRook = chessmans[selectionX, selectionY];
                int x = selectedRook.CurrentX;
                int y = selectedRook.CurrentY;

                bool[,] RookMove = new bool[8, 8];

                for (int r = 0; r < 8; r++){
                    if (isWhiteTurn && (chessmans[r, 1] == null)) { RookMove[r, 1] = true; }
                    else if (!isWhiteTurn && (chessmans[r, 6] == null)) { RookMove[r, 6] = true; }
                }
                BH.HighlightAllowedMoves(RookMove);

                bool noMov = false;
                for (int k = 0; k < 8; k++){
                    for (int t = 0; t < 8; t++){
                        if (RookMove[k, t])
                            noMov = true;
                    }
                }
                if (!noMov) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

                while (true)
                {
                    if (Input.GetMouseButtonDown(0) && (RookMove[selectionX, selectionY] == true))
                    {
                        BH.Hidehighlights();
                        BM.Move(x, y, selectionX, selectionY);
                        DR.CheckTurnEnd("pass", ShowCard);
                        cardUsed = true;
                        BM.EndTurn();

                        if (BM.client)
                        {
                            string msg = "CCRD|Defense|";
                            msg += x + "|";
                            msg += y + "|";
                            msg += selectionX + "|";
                            msg += selectionY + "|";
                            msg += (BM.client.isHost ? 1 : 0);
                            BM.client.Send(msg);
                        }
                        BM.LogMessage(2, "Defense", x, y, selectionX, selectionY, (BM.isWhite ? 1 : 0));
                        break;

                    }
                    else { yield return null; }
                }
                break;
            }
            else { yield return null; }
        }

        yield return null;
    }

    private IEnumerator CounterCor()
    {
        BH.Hidehighlights();
        allowedPosition = new bool[8, 8];

        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if ((chessmans[i, j] != null) && (chessmans[i, j].GetType() == typeof(Pawn)) && (chessmans[i, j].isWhite == isWhiteTurn) && BM.isAttacked(i, j, isWhiteTurn))
                    allowedPosition[i, j] = true;
                else
                    allowedPosition[i, j] = false;
            }
        }

        BH.HighlightAllowedMoves(allowedPosition);

        bool noPos = false;
        for (int k = 0; k < 8; k++){
            for (int t = 0; t < 8; t++){
                if (allowedPosition[k, t])
                    noPos = true;
            }
        }
        if (!noPos) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true))
            {
                BH.Hidehighlights();
                Chessman selectedPawn = chessmans[selectionX, selectionY];
                int x = selectedPawn.CurrentX;
                int y = selectedPawn.CurrentY;

                bool[,] PawnMove = new bool[8, 8];

                for (int r = 0; r < 8; r++)
                {
                    if (isWhiteTurn && (chessmans[r, 6] == null)) { PawnMove[r, 6] = true; }
                    else if (!isWhiteTurn && (chessmans[r, 1] == null)) { PawnMove[r, 1] = true; }
                }
                BH.HighlightAllowedMoves(PawnMove);

                bool noMov = false;
                for (int k = 0; k < 8; k++){
                    for (int t = 0; t < 8; t++){
                        if (PawnMove[k, t])
                            noMov = true;
                    }
                }
                if (!noMov) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

                while (true)
                {
                    if (Input.GetMouseButtonDown(0) && (PawnMove[selectionX, selectionY] == true))
                    {
                        BH.Hidehighlights();
                        BM.Move(x, y, selectionX, selectionY);
                        DR.CheckTurnEnd("pass", ShowCard);
                        cardUsed = true;
                        BM.EndTurn();
                        if (BM.client)
                        {
                            string msg = "CCRD|Counter|";
                            msg += x + "|";
                            msg += y + "|";
                            msg += selectionX + "|";
                            msg += selectionY + "|";
                            msg += (BM.client.isHost ? 1 : 0);
                            BM.client.Send(msg);
                        }
                        BM.LogMessage(2, "Counter", x, y, selectionX, selectionY, (BM.isWhite ? 1 : 0));
                        break;

                    }
                    else { yield return null; }
                }
                break;
            }
            else { yield return null; }
        }

        yield return null;
    }

    private IEnumerator SacrificeCor()
    {
        BH.Hidehighlights();
        allowedPosition = new bool[8, 8];

        if (isWhiteTurn && !BM.isAttacked(BM.WhiteKingPos[0], BM.WhiteKingPos[1], true)) { DR.CheckTurnEnd("fail", ShowCard); yield break; }
        if (!isWhiteTurn && !BM.isAttacked(BM.BlackKingPos[0], BM.BlackKingPos[1], false)) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if ((chessmans[i, j] != null) && (chessmans[i, j].GetType() == typeof(Pawn)) && (chessmans[i, j].isWhite == isWhiteTurn))
                    allowedPosition[i, j] = true;
                else
                    allowedPosition[i, j] = false;
            }
        }

        BH.HighlightAllowedMoves(allowedPosition);

        bool noPos = false;
        for (int k = 0; k < 8; k++){
            for (int t = 0; t < 8; t++){
                if (allowedPosition[k, t])
                    noPos = true;
            }
        }
        if (!noPos) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true))
            {
                BH.Hidehighlights();
                if (isWhiteTurn)
                {
                    int[] wpos = BM.WhiteKingPos;
                    Debug.Log("Sacrifice: " + selectionX + selectionY + wpos[0] + wpos[1]);
                    if (BM.client)
                    {
                        string msg = "CCRD|Sacrifice|";
                        msg += selectionX + "|";
                        msg += selectionY + "|";
                        msg += wpos[0] + "|";
                        msg += wpos[1] + "|";
                        msg += (BM.client.isHost ? 1 : 0);
                        BM.client.Send(msg);
                    }
                    BM.LogMessage(2, "Sacrifice", selectionX, selectionY, wpos[0], wpos[1], (BM.isWhite ? 1 : 0));
                    BM.Swap(selectionX, selectionY, wpos[0], wpos[1]);
                    BM.WhiteKingPos[0] = selectionX;
                    BM.WhiteKingPos[1] = selectionY;
                    DR.CheckTurnEnd("pass", ShowCard);
                    //BM.EndTurn();
                    cardUsed = true;
                    break;
                }
                else
                {
                    int[] bpos = BM.BlackKingPos;
                    if (BM.client)
                    {
                        string msg = "CCRD|Sacrifice|";
                        msg += selectionX + "|";
                        msg += selectionY + "|";
                        msg += bpos[0] + "|";
                        msg += bpos[1] + "|";
                        msg += (BM.client.isHost ? 1 : 0);
                        BM.client.Send(msg);
                    }
                    BM.LogMessage(2, "Sacrifice", selectionX, selectionY, bpos[0], bpos[1], (BM.isWhite ? 1 : 0));
                    BM.Swap(selectionX, selectionY, bpos[0], bpos[1]);
                    BM.BlackKingPos[0] = selectionX;
                    BM.BlackKingPos[1] = selectionY;
                    DR.CheckTurnEnd("pass", ShowCard);
                    //BM.EndTurn();
                    cardUsed = true;
                    break;
                }
            }
            else { yield return null; }
        }

        yield return null;
    }

    private IEnumerator DoOrDieCor()
    {
        BH.Hidehighlights();
        allowedPosition = new bool[8, 8];

        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if ((chessmans[i, j] != null) && (chessmans[i, j].GetType() == typeof(Knight)) && (chessmans[i, j].isWhite == isWhiteTurn) && (j == 0))
                    allowedPosition[i, j] = true;
                else if ((chessmans[i, j] != null) && (chessmans[i, j].GetType() == typeof(Knight)) && (chessmans[i, j].isWhite == isWhiteTurn) && (j == 7))
                    allowedPosition[i, j] = true;
                else
                    allowedPosition[i, j] = false;
            }
        }

        BH.HighlightAllowedMoves(allowedPosition);

        bool noPos = false;
        for (int k = 0; k < 8; k++){
            for (int t = 0; t < 8; t++){
                if (allowedPosition[k, t])
                    noPos = true;
            }
        }
        if (!noPos) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true)){
                BH.Hidehighlights();
                Chessman selectedKnight = chessmans[selectionX, selectionY];
                int x = selectedKnight.CurrentX;
                int y = selectedKnight.CurrentY;

                bool[,] KnightMove = new bool[8, 8];

                for (int r = 0; r < 8; r++){
                    for (int t=0; t<8; t++){
                        if ((chessmans[r, t] != null) && (chessmans[r, t].GetType() == typeof(Knight)) && (chessmans[r, t].isWhite != isWhiteTurn))
                            KnightMove[r, t] = true;
                        else
                            KnightMove[r, t] = false;
                    }
                }
                BH.HighlightAllowedMoves(KnightMove);

                bool noMov = false;
                for (int k = 0; k < 8; k++){
                    for (int t = 0; t < 8; t++){
                        if (KnightMove[k, t])
                            noMov = true;
                    }
                }
                if (!noMov) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

                while (true) {
                    if (Input.GetMouseButtonDown(0) && (KnightMove[selectionX, selectionY] == true))
                    {
                        BH.Hidehighlights();
                        BM.Swap(x, y, selectionX, selectionY);
                        DR.CheckTurnEnd("pass", ShowCard);
                        cardUsed = true;
                        BM.EndTurn();
                        if (BM.client)
                        {
                            string msg = "CCRD|DoOrDie|";
                            msg += x + "|";
                            msg += y + "|";
                            msg += selectionX + "|";
                            msg += selectionY + "|";
                            msg += (BM.client.isHost ? 1 : 0);
                            BM.client.Send(msg);
                        }
                        BM.LogMessage(2, "DoOrDie", x, y, selectionX, selectionY, (BM.isWhite ? 1 : 0));
                        break;
                    }
                    else { yield return null; }
                }
                break;
            }
            else { yield return null; }
        }

        yield return null;
    }

    private IEnumerator AmbushCor()
    {
        BH.Hidehighlights();
        if ((isWhiteTurn) && (BM.TrapPosW[0] + BM.TrapPosW[1] >= 0)) { DR.CheckTurnEnd("fail", ShowCard); yield break; }
        if ((!isWhiteTurn) && (BM.TrapPosB[0] + BM.TrapPosB[1] >= 0)) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        allowedPosition = new bool[8, 8];

        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if ((chessmans[i, j] == null) && (j > 1) && (j < 6))
                    allowedPosition[i, j] = true;
            }
        }

        BH.HighlightAllowedMoves(allowedPosition);

        bool noPos = false;
        for (int k = 0; k < 8; k++){
            for (int t = 0; t < 8; t++){
                if (allowedPosition[k, t])
                    noPos = true;
            }
        }
        if (!noPos) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true))
            {
                BH.Hidehighlights();
                if (BM.client)
                    BM.SpawnTrap(selectionX, selectionY, 8, isWhiteTurn);
                else
                    BM.UpdateTrap(selectionX, selectionY, 8, (isWhiteTurn ? 1 : 0));
                BM.LogMessage(2, "Ambush", 0, 0, selectionX, selectionY, (BM.isWhite ? 1 : 0));
                DR.CheckTurnEnd("pass", ShowCard);
                break;
            }
            else { yield return null; }
        }



        yield return null;
    }

    private IEnumerator RepulseCor()
    {
        BH.Hidehighlights();
        allowedPosition = new bool[8, 8];

        for (int i = 0; i < 8; i++){
            for (int j = 0; j < 8; j++){
                if ((chessmans[i, j] != null) && (chessmans[i, j].GetType() == typeof(Queen)) && (chessmans[i, j].isWhite != isWhiteTurn))
                    allowedPosition[i, j] = true;
                else
                    allowedPosition[i, j] = false;
            }
        }

        BH.HighlightAllowedMoves(allowedPosition);

        bool noPos = false;
        for (int k = 0; k < 8; k++){
            for (int t = 0; t < 8; t++){
                if (allowedPosition[k, t])
                    noPos = true;
            }
        }
        if (!noPos) { DR.CheckTurnEnd("fail", ShowCard); yield break; }

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && (allowedPosition[selectionX, selectionY] == true))
            {
                BH.Hidehighlights();
                if (isWhiteTurn)
                {
                    int newY;
                    if (selectionY == 7)
                        newY = 7;
                    else
                        newY = selectionY + 1;
                    if (chessmans[selectionX, newY] == null)
                    {
                        BM.Move(selectionX, selectionY, selectionX, newY);
                        if (BM.client)
                        {
                            string msg = "CCRD|Repulse|";
                            msg += selectionX + "|";
                            msg += selectionY + "|";
                            msg += selectionX + "|";
                            msg += newY + "|";
                            msg += (BM.client.isHost ? 1 : 0);
                            BM.client.Send(msg);
                        }
                        BM.LogMessage(2, "Repulse", selectionX, selectionY, selectionX, newY, (BM.isWhite ? 1 : 0));
                        DR.CheckTurnEnd("pass", ShowCard);
                        cardUsed = true;
                        BM.EndTurn();
                        break;
                    }
                    else
                    {
                        DR.CheckTurnEnd("fail", ShowCard); yield break;
                    }
                }
                else
                {
                    int newY;
                    if (selectionY == 0)
                        newY = 0;
                    else
                        newY = selectionY - 1;
                    if (chessmans[selectionX, newY] == null)
                    {
                        BM.Move(selectionX, selectionY, selectionX, newY);
                        if (BM.client)
                        {
                            string msg = "CCRD|Repulse|";
                            msg += selectionX + "|";
                            msg += selectionY + "|";
                            msg += selectionX + "|";
                            msg += newY + "|";
                            msg += (BM.client.isHost ? 1 : 0);
                            BM.client.Send(msg);
                        }
                        BM.LogMessage(2, "Repulse", selectionX, selectionY, selectionX, newY, (BM.isWhite ? 1 : 0));
                        DR.CheckTurnEnd("pass", ShowCard);
                        cardUsed = true;
                        BM.EndTurn();
                        break;
                    }
                    else
                    {
                        DR.CheckTurnEnd("fail", ShowCard); yield break;
                    }
                }
            }
            else { yield return null; }
        }

        yield return null;
    }


    public void UpdateCard(string type, int prevX, int prevY, int x, int y)
    {
        if (type == "")
            return;

        switch (type)
        {
            case "Reinforcements":
                CardObjectBl.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "증원군";
                CardObjectBl.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "자신의 영토에\r\n폰을 하나 소환한다";
                NewCard = Instantiate(CardObjectBl);
                UpdateShow();
                if (BM.Chessmans[x, y] != null)
                    break;
                if (BM.isWhite)
                    BM.SpawnChessman(11, x, y);
                else
                    BM.SpawnChessman(5, x, y);
                break;
            case "Chivalry":
                CardObjectG.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "기사도";
                CardObjectG.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "킹과 나이트의\r\n자리를 스왑한다";
                NewCard = Instantiate(CardObjectG);
                UpdateShow();
                BM.Swap(prevX, prevY, x, y);
                break;
            case "Reflection":
                CardObjectBr.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "반전";
                CardObjectBr.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "비숍 하나를\r\n좌우로 한 칸 이동한다";
                NewCard = Instantiate(CardObjectBr);
                UpdateShow();
                BM.Move(prevX, prevY, x, y);
                BM.EndTurn();
                break;
            case "Defense":
                CardObjectBr.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "방어";
                CardObjectBr.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "전장에 있는 룩을\r\n자신의 영토로 이동시킨다";
                NewCard = Instantiate(CardObjectBr);
                UpdateShow();
                BM.Move(prevX, prevY, x, y);
                BM.EndTurn();
                break;
            case "Counter":
                CardObjectBr.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "반격";
                CardObjectBr.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "공격당하고 있는\r\n폰을 상대 영토로\r\n이동시킨다";
                NewCard = Instantiate(CardObjectBr);
                UpdateShow();
                BM.Move(prevX, prevY, x, y);
                BM.EndTurn();
                break;
            case "Sacrifice":
                CardObjectG.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "희생";
                CardObjectG.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "체크 상황일 때,\r\n폰과 킹을 스왑한다.";
                NewCard = Instantiate(CardObjectG);
                UpdateShow();
                BM.Swap(prevX, prevY, x, y);
                break;
            case "DoOrDie":
                CardObjectG.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "승부수";
                CardObjectG.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "자신의 나이트를\r\n상대의 것과 스왑한다.";
                NewCard = Instantiate(CardObjectG);
                UpdateShow();
                BM.Swap(prevX, prevY, x, y);
                BM.EndTurn();
                break;
            case "Repulse":
                CardObjectBr.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "격퇴";
                CardObjectBr.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "상대의 퀸을\r\n뒤로 한 칸 움직인다.";
                NewCard = Instantiate(CardObjectBr);
                UpdateShow();
                BM.Move(prevX, prevY, x, y);
                BM.EndTurn();
                break;
            default:
                break;
        }

        
    }

    public void UpdateCard3(string type, int prevX1, int prevY1, int prevX2, int prevY2, int x, int y)
    {
        if (type == "")
            return;

        switch (type)
        {
            case "Medal":
                CardObjectBl.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "명예훈장";
                CardObjectBl.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "폰 두개로\r\n새로운 나이트 소환한다";
                NewCard = Instantiate(CardObjectBl);
                UpdateShow();
                BM.Delete(prevX1, prevY1);
                BM.Delete(prevX2, prevY2);
                if (BM.isWhite)
                    BM.SpawnChessman(10, x, y);
                else
                    BM.SpawnChessman(4, x, y);
                break;
            default:
                break;
        }

        //BM.isWhiteTurn = !isWhiteTurn;
    }

    public void TrapCard()
    {
        CardObjectBl.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "매복";
        CardObjectBl.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "전장에 함정을\r\n설치한다.";
        NewCard = Instantiate(CardObjectBl);
        UpdateShow();
    }

    private void UpdateSelection()
    {
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

    private void UpdateShow()
    {
        NewCard.transform.SetParent(GameObject.Find("ShowCard").transform, false);              //카드 위치를 canvas로
        NewCard.transform.localScale = new Vector3(1.636323f, 2.768322f, 2.381825f);            //카드 크기를 크게
        NewCard.transform.localPosition = new Vector3(400, 0, 0.0f);                            //위치 약간 오른쪽으로        

        Destroy(NewCard.gameObject, 1.5f);
    }

}
