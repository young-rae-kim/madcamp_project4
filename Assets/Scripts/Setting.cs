using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using GameSparks.Api.Requests;
using GameSparks.Core;

public class Setting : MonoBehaviour
{
    public ArrayList DeckList = new ArrayList();

    public int Picknum;
    public int PickCount;

    public GameObject PointerEnObj;

    void Awake()
    {
        DontDestroyOnLoad(GameObject.Find("DataBook"));            
    }

    // Start is called before the first frame update
    void Start()
    {
        if (this.name == "PickCanvas")
        {
            new LogEventRequest().SetEventKey("FETCH_DECK")
                        .Send((res) =>
                        {
                            GSData data = res.ScriptData.GetGSData("player_deck");

                            int[] dataArray = new int[10];
                            dataArray[0] = int.Parse(data.GetInt("DECK_0").ToString());
                            dataArray[1] = int.Parse(data.GetInt("DECK_1").ToString());
                            dataArray[2] = int.Parse(data.GetInt("DECK_2").ToString());
                            dataArray[3] = int.Parse(data.GetInt("DECK_3").ToString());
                            dataArray[4] = int.Parse(data.GetInt("DECK_4").ToString());
                            dataArray[5] = int.Parse(data.GetInt("DECK_5").ToString());
                            dataArray[6] = int.Parse(data.GetInt("DECK_6").ToString());
                            dataArray[7] = int.Parse(data.GetInt("DECK_7").ToString());
                            dataArray[8] = int.Parse(data.GetInt("DECK_8").ToString());
                            dataArray[9] = int.Parse(data.GetInt("DECK_9").ToString());

                            GameSparksManager.instance.userDeck[0] = dataArray[0];
                            GameObject.Find("CardBl").transform.GetComponent<Setting>().PickCount = dataArray[0];
                            GameObject.Find("CardBl").transform.Find("Text").GetComponent<Text>().text = "" + dataArray[0];
                            GameSparksManager.instance.userDeck[1] = dataArray[1];
                            GameObject.Find("CardG").transform.GetComponent<Setting>().PickCount = dataArray[1];
                            GameObject.Find("CardG").transform.Find("Text").GetComponent<Text>().text = "" + dataArray[1];
                            GameSparksManager.instance.userDeck[2] = dataArray[2];
                            GameObject.Find("CardBr").transform.GetComponent<Setting>().PickCount = dataArray[2];
                            GameObject.Find("CardBr").transform.Find("Text").GetComponent<Text>().text = "" + dataArray[2];
                            GameSparksManager.instance.userDeck[3] = dataArray[3];
                            GameObject.Find("CardBl (2)").transform.GetComponent<Setting>().PickCount = dataArray[3];
                            GameObject.Find("CardBl (2)").transform.Find("Text").GetComponent<Text>().text = "" + dataArray[3];
                            GameSparksManager.instance.userDeck[4] = dataArray[4];
                            GameObject.Find("CardBr (3)").transform.GetComponent<Setting>().PickCount = dataArray[4];
                            GameObject.Find("CardBr (3)").transform.Find("Text").GetComponent<Text>().text = "" + dataArray[4];
                            GameSparksManager.instance.userDeck[5] = dataArray[5];
                            GameObject.Find("CardBr (2)").transform.GetComponent<Setting>().PickCount = dataArray[5];
                            GameObject.Find("CardBr (2)").transform.Find("Text").GetComponent<Text>().text = "" + dataArray[5];
                            GameSparksManager.instance.userDeck[6] = dataArray[6];
                            GameObject.Find("CardG (2)").transform.GetComponent<Setting>().PickCount = dataArray[6];
                            GameObject.Find("CardG (2)").transform.Find("Text").GetComponent<Text>().text = "" + dataArray[6];
                            GameSparksManager.instance.userDeck[7] = dataArray[7];
                            GameObject.Find("CardG (1)").transform.GetComponent<Setting>().PickCount = dataArray[7];
                            GameObject.Find("CardG (1)").transform.Find("Text").GetComponent<Text>().text = "" + dataArray[7];
                            GameSparksManager.instance.userDeck[8] = dataArray[8];
                            GameObject.Find("CardBl (1)").transform.GetComponent<Setting>().PickCount = dataArray[8];
                            GameObject.Find("CardBl (1)").transform.Find("Text").GetComponent<Text>().text = "" + dataArray[8];
                            GameSparksManager.instance.userDeck[9] = dataArray[9];
                            GameObject.Find("CardBr (1)").transform.GetComponent<Setting>().PickCount = dataArray[9];
                            GameObject.Find("CardBr (1)").transform.Find("Text").GetComponent<Text>().text = "" + dataArray[9];

                            for (int i = 0; i < 10; i++)
                            {
                                PickCount += dataArray[i];
                                for (int j = 0; j < dataArray[i]; j++)
                                {
                                    DeckList.Add(i);
                                }
                            }
                        });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((GameObject.Find("PickCanvas")).GetComponent<Setting>().PointerEnObj == this.gameObject) {
            if (Input.GetMouseButtonUp(1)) //우
            {
                if (PickCount > 0 )
                {
                    GameObject.Find("UndrawCard").GetComponent<AudioSource>().Play();
                    (GameObject.Find("PickCanvas")).GetComponent<Setting>().RemoveToCard(this.gameObject.GetComponent<Setting>().Picknum);
                }
            }
            if (Input.GetMouseButtonUp(0)) //좌
            {
                if ((GameObject.Find("PickCanvas")).GetComponent<Setting>().PickCount < 20)
                {
                    GameObject.Find("DrawCard").GetComponent<AudioSource>().Play();
                    (GameObject.Find("PickCanvas")).GetComponent<Setting>().AddToCard(this.Picknum);
                }
            }
        }
    }
    

    public void POINTERENTER()
    {
        //Debug.Log("POINTERENTER gameObject : " + this.gameObject);
        (GameObject.Find("PickCanvas")).GetComponent<Setting>().PointerEnObj = this.gameObject;
    }

    public void POINTEREXIT()
    {
        //Debug.Log("POINTEREXIT gameObject : " + this.gameObject);
        (GameObject.Find("PickCanvas")).GetComponent<Setting>().PointerEnObj = null;
    }

    void AddToCard(int num)
    {
        DeckList.Add(num);
        PickCount += 1;
        PointerEnObj.GetComponent<Setting>().PickCount += 1;
        //GameSparksManager.instance.userDeck[num] = PointerEnObj.gameObject.GetComponent<Setting>().PickCount;
        PointerEnObj.gameObject.transform.Find("Text").GetComponent<Text>().text = "" + PointerEnObj.gameObject.GetComponent<Setting>().PickCount;
    }

    void RemoveToCard(int num)
    {
        DeckList.Remove(num);
        PickCount -= 1;
        PointerEnObj.gameObject.GetComponent<Setting>().PickCount -= 1;
        //GameSparksManager.instance.userDeck[num] = PointerEnObj.gameObject.GetComponent<Setting>().PickCount;
        PointerEnObj.gameObject.transform.Find("Text").GetComponent<Text>().text = "" + PointerEnObj.gameObject.GetComponent<Setting>().PickCount;
    }
    
    public void NextScene() {
        GameObject.Find("DataBook").GetComponent<Next>().PickCount = PickCount;
        GameObject.Find("DataBook").GetComponent<Next>().DList = DeckList;
        SceneManager.LoadScene("Menu");
    }

    public void ClickBook()
    {
        GameObject.Find("BookFlip").GetComponent<AudioSource>().Play();
        transform.Find("CardBook").gameObject.SetActive(true);
    }

}
