using GameSparks.Api.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Next : MonoBehaviour
{
    public ArrayList DList = new ArrayList();

    public GameObject ErrorPanel;

    public int Picknum;
    public int PickCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckList()
    {
        DList = (GameObject.Find("PickCanvas")).GetComponent<Setting>().DeckList;

        int[] deck = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < DList.Count; i++)
        {
            deck[int.Parse(DList[i].ToString())]++;
        }

        if (GameSparksManager.instance != null)
        {
            int gold = GameSparksManager.instance.userGold;
            for (int j = 0; j < 10; j++)
            {
                if (deck[j] > GameSparksManager.instance.userDeck[j])
                    gold -= 50 * (deck[j] - GameSparksManager.instance.userDeck[j]);
            }

            if (gold < 0)
            {
                // Cannot change deck
                ErrorPanel.SetActive(true);
                Invoke("ClosePanelWithLoad", 1);
                return;
            }
            else
            {
                new LogEventRequest().SetEventKey("SET_GOLD")
                    .SetEventAttribute("playerGold", gold)
                    .Send((res) => 
                    {
                        if (!res.HasErrors)
                        {
                            Debug.Log("Gold updated successfully.");
                            GameSparksManager.instance.userGold = gold;
                        }
                        else
                        {
                            Debug.Log("Update failed. " + res.Errors.JSON.ToString());
                        }
                    });

                new LogEventRequest().SetEventKey("SAVE_DECK")
                        .SetEventAttribute("DECK_0", deck[0])
                        .SetEventAttribute("DECK_1", deck[1])
                        .SetEventAttribute("DECK_2", deck[2])
                        .SetEventAttribute("DECK_3", deck[3])
                        .SetEventAttribute("DECK_4", deck[4])
                        .SetEventAttribute("DECK_5", deck[5])
                        .SetEventAttribute("DECK_6", deck[6])
                        .SetEventAttribute("DECK_7", deck[7])
                        .SetEventAttribute("DECK_8", deck[8])
                        .SetEventAttribute("DECK_9", deck[9])
                        .Send((res) =>
                        {
                            if (!res.HasErrors)
                            {
                                Debug.Log("Deck saved successfully.");
                                ErrorPanel.SetActive(true);
                                ErrorPanel.GetComponentInChildren<Text>().text = "Deck saved successfully.";
                                Invoke("ClosePanel", 1);
                                for (int i = 0; i < 10; i++)
                                {
                                    GameSparksManager.instance.userDeck[i] = deck[i];
                                }
                            }
                            else
                            {
                                Debug.Log("Saving failed. " + res.Errors.JSON.ToString());
                            }
                        });
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                GameSparksManager.instance.userDeck[i] = deck[i];
            }
            ErrorPanel.SetActive(true);
            ErrorPanel.GetComponentInChildren<Text>().text = "Deck saved successfully.";
            Invoke("ClosePanel", 1);
        }
    }

    public void ClosePanel()
    {
        ErrorPanel.SetActive(false);
    }

    public void ClosePanelWithLoad()
    {
        ErrorPanel.SetActive(false);
        SceneManager.LoadScene("Pick");
    }
}
