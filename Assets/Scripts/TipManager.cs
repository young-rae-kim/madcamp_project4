using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TipManager : MonoBehaviour
{
    public GameObject TipsPanel1;
    public GameObject TipsPanel2;
    public GameObject TipsPanel3;
    private int index = 1;

    // Start is called before the first frame update
    void Start()
    {
        TipsPanel1.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrevButton()
    {
        switch (index)
        {
            case 1:
                break;
            case 2:
                TipsPanel1.SetActive(true);
                TipsPanel2.SetActive(false);
                TipsPanel3.SetActive(false);
                index--;
                break;
            case 3:
                TipsPanel1.SetActive(false);
                TipsPanel2.SetActive(true);
                TipsPanel3.SetActive(false);
                index--;
                break;
        }
    }

    public void NextButton()
    {
        switch (index)
        {
            case 1:
                TipsPanel1.SetActive(false);
                TipsPanel2.SetActive(true);
                TipsPanel3.SetActive(false);
                index++;
                break;
            case 2:
                TipsPanel1.SetActive(false);
                TipsPanel2.SetActive(false);
                TipsPanel3.SetActive(true);
                index++;
                break;
            case 3:
                break;
        }
    }

    public void CloseButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
