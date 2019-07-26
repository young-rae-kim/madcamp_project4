using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman
{

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        int i, j;

        //TopSide
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 7)
        {
            for (int k=0; k<3; k++)
            {
                if ((i > 0) || (i < 8))
                {
                    c = BoardManager.Instance.Chessmans[i, j];
                    if (c == null)                      //if no chessman 
                        r[i, j] = true;
                    else if (isWhite != c.isWhite)      //or enemy
                        r[i, j] = true;
                }

                i++;
            }
        }

        //DownSide
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int k = 0; k < 3; k++)
            {
                if ((i > 0) || (i < 8))
                {
                    c = BoardManager.Instance.Chessmans[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isWhite != c.isWhite)
                        r[i, j] = true;
                }

                i++;
            }
        }

        //Left
        if (CurrentX != 0)
        {
            c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY];
            if (c == null)
                r[CurrentX - 1, CurrentY] = true;
            else if (isWhite != c.isWhite)
                r[CurrentX - 1, CurrentY] = true;
        }

        //Right
        if (CurrentX != 7)
        {
            c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY];
            if (c == null)
                r[CurrentX + 1, CurrentY] = true;
            else if (isWhite != c.isWhite)
                r[CurrentX + 1, CurrentY] = true;
        }


        //Castling
        if (Moved == false)
        {
            bool[] castleMoved = BoardManager.Instance.CastleHasMoved;
 
            if (isWhite && !BoardManager.Instance.CheckW)
            {
                if ((castleMoved[0] == false) && (BoardManager.Instance.Chessmans[5, 0] == null) && (BoardManager.Instance.Chessmans[6, 0] == null) && (!BoardManager.Instance.isAttacked(5, 0, isWhite)) && (!BoardManager.Instance.isAttacked(6, 0, isWhite)))
                {   //if KingSide castle not moved & no chessmans & no attack in route
                    r[CurrentX + 2, CurrentY] = true;
                }
                if ((castleMoved[1] == false) && (BoardManager.Instance.Chessmans[3, 0] == null) && (BoardManager.Instance.Chessmans[2, 0] == null) && (BoardManager.Instance.Chessmans[1, 0] == null) && (!BoardManager.Instance.isAttacked(3, 0, isWhite)) && (!BoardManager.Instance.isAttacked(2, 0, isWhite)))
                {   //if QueenSide castle not moved & no chessmans & no attack in route
                    r[CurrentX - 2, CurrentY] = true;
                }
            } else if (!isWhite && !BoardManager.Instance.CheckB)
            {
                if ((castleMoved[2] == false) && (BoardManager.Instance.Chessmans[5, 7] == null) && (BoardManager.Instance.Chessmans[6, 7] == null) && (!BoardManager.Instance.isAttacked(5, 7, isWhite)) && (!BoardManager.Instance.isAttacked(6, 7, isWhite)))
                {   //if KingSide castle not moved & no chessmans & no attack in route
                    r[CurrentX + 2, CurrentY] = true;
                }
                if ((castleMoved[3] == false) && (BoardManager.Instance.Chessmans[3, 7] == null) && (BoardManager.Instance.Chessmans[2, 7] == null) && (BoardManager.Instance.Chessmans[1, 7] == null) && (!BoardManager.Instance.isAttacked(3, 7, isWhite)) && (!BoardManager.Instance.isAttacked(2, 7, isWhite)))
                {   //if QueenSide castle not moved & no chessmans & no attack in route
                    r[CurrentX - 2, CurrentY] = true;
                }
            }
        }


        return r;

    }

    public override bool[,] PossibleMove2()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        int i, j;

        //TopSide
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 7)
        {
            for (int k = 0; k < 3; k++)
            {
                if ((i > 0) || (i < 8))
                {
                    c = BoardManager.Instance.Chessmans[i, j];
                    if (c == null)                      //if no chessman 
                        r[i, j] = true;
                    else      //or enemy
                        r[i, j] = true;
                }

                i++;
            }
        }

        //DownSide
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int k = 0; k < 3; k++)
            {
                if ((i > 0) || (i < 8))
                {
                    c = BoardManager.Instance.Chessmans[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else
                        r[i, j] = true;
                }

                i++;
            }
        }

        //Left
        if (CurrentX != 0)
        {
            c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY];
            if (c == null)
                r[CurrentX - 1, CurrentY] = true;
            else 
                r[CurrentX - 1, CurrentY] = true;
        }

        //Right
        if (CurrentX != 7)
        {
            c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY];
            if (c == null)
                r[CurrentX + 1, CurrentY] = true;
            else
                r[CurrentX + 1, CurrentY] = true;
        }


        return r;

    }
}
