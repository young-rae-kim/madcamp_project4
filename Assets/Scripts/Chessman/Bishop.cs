﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        int i, j;

        //TopLeft
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if ((i < 0) || (j > 7))
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (isWhite != c.isWhite)
                    r[i, j] = true;
                break;
            }
        }

        //TopRight
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if ((i > 7) || (j > 7))
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (isWhite != c.isWhite)
                    r[i, j] = true;
                break;
            }
        }

        //DownLeft
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if ((i < 0) || (j < 0))
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (isWhite != c.isWhite)
                    r[i, j] = true;
                break;
            }
        }

        //DownRight
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if ((i > 7) || (j < 0))
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (isWhite != c.isWhite)
                    r[i, j] = true;
                break;
            }
        }


        return r;
    }

    public override bool[,] PossibleMove2()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        int i, j;

        //TopLeft
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if ((i < 0) || (j > 7))
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
               r[i, j] = true;
                break;
            }
        }

        //TopRight
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if ((i > 7) || (j > 7))
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                r[i, j] = true;
                break;
            }
        }

        //DownLeft
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if ((i < 0) || (j < 0))
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                r[i, j] = true;
                break;
            }
        }

        //DownRight
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if ((i > 7) || (j < 0))
                break;

            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                r[i, j] = true;
                break;
            }
        }


        return r;
    }
}
