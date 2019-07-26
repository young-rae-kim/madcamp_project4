using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentGold : MonoBehaviour
{
    int gold;
    Text GoldTx;

    // Start is called before the first frame update
    void Start()
    {
        gold = GameSparksManager.instance.userGold;
        GoldTx = GameObject.Find("Gold").GetComponent<Text>();
        GoldTx.text = "현재 골드: " + gold +"G";
    }

    public void updateTx()
    {
        StartCoroutine("updateCor");
    }

    IEnumerator updateCor()
    {
        yield return new WaitForSeconds(1f);
        gold = GameSparksManager.instance.userGold;
        GoldTx.text = "현재 골드: " + gold + "G";
    }
}
