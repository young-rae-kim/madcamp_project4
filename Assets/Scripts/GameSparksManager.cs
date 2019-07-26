using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSparksManager : MonoBehaviour
{
    public static GameSparksManager instance { set; get; } = null;
    public string userID = "";
    public string userName = "";
    public int userEXP = 0;
    public int userGold = 0;
    public int[] userDeck = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
