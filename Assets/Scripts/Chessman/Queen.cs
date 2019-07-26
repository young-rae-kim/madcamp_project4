using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        int i, j;

        //Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i > 7)
                break;

            c = BoardManager.Instance.Chessmans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;
                break;
            }
        }

        //Left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.Chessmans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;
                break;
            }
        }

        //Up
        i = CurrentY;
        while (true)
        {
            i++;
            if (i > 7)
                break;

            c = BoardManager.Instance.Chessmans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[CurrentX, i] = true;
                break;
            }
        }

        //Down
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.Chessmans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[CurrentX, i] = true;
                break;
            }
        }
       
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

        //Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i > 7)
                break;

            c = BoardManager.Instance.Chessmans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                r[i, CurrentY] = true;
                break;
            }
        }

        //Left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.Chessmans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                r[i, CurrentY] = true;
                break;
            }
        }

        //Up
        i = CurrentY;
        while (true)
        {
            i++;
            if (i > 7)
                break;

            c = BoardManager.Instance.Chessmans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                r[CurrentX, i] = true;
                break;
            }
        }

        //Down
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.Chessmans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                r[CurrentX, i] = true;
                break;
            }
        }

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
