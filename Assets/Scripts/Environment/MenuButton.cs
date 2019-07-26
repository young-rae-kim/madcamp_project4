using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public void OnMouseEnter()
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.3f);
    }

    public void OnMouseExit()
    {
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
}
