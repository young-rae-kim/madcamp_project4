using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Make : MonoBehaviour
{

    public GameObject NewCard;
    public GameObject CardObjectBl;
    public GameObject CardObjectE;
    public GameObject CardObjectG;
    public GameObject CardObjectBr;

    public GameObject newC = null;
    public int pointer = 0;

    public bool iswhite;
    public bool isWhiteTurn;

    public Camera cam;

    private Client client;

    public Transform wantedPos;

    public ArrayList Dlist;
    public ArrayList SparksDlist = new ArrayList();
    public int Sparksnum = 0;
    public int num;

    public int MakeCardCount = 0;

    public Texture2D C0;

    bool drawing;

    private void Start()
    {
        drawing = false;
        client = FindObjectOfType<Client>();
        if (GameObject.Find("ChessBoard") != null)
        {
            iswhite = GameObject.Find("ChessBoard").GetComponent<BoardManager>().isWhite;
            isWhiteTurn = GameObject.Find("ChessBoard").GetComponent<BoardManager>().isWhiteTurn;
        }

        if (GameSparksManager.instance != null)
        {
            for (int i = 0; i < 10; i++)
            {
                Sparksnum += GameSparksManager.instance.userDeck[i];
                for (int j = 0; j < GameSparksManager.instance.userDeck[i]; j++)
                {
                    SparksDlist.Add(i);
                }
            }
        }
    }

    private void Update()
    {
        if (client)
        {
            iswhite = GameObject.Find("ChessBoard").GetComponent<BoardManager>().isWhite;
            isWhiteTurn = GameObject.Find("ChessBoard").GetComponent<BoardManager>().isWhiteTurn;
        }

    }

    public void Large()
    {
        if (client && GameObject.Find("MyDeck").GetComponent<draggable>().Data == 0)
        {
            if ((this.gameObject.name).Contains("Card") == true && this.transform.parent == GameObject.Find("MyDeck").transform)
            {
                this.transform.localScale = new Vector3(1.5308f, 2.6f, 1.1142f);
                this.transform.position = new Vector3(this.transform.position.x, 195, this.transform.position.z);
                pointer = 1;
            }
        }
    }

    public void Small()
    {
        if (client && GameObject.Find("MyDeck").GetComponent<draggable>().Data == 0 && pointer == 1)
        {
            if ((this.gameObject.name).Contains("Card") == true && this.transform.parent == GameObject.Find("MyDeck").transform)
            {
                this.transform.position = new Vector3(this.transform.position.x, -5, this.transform.position.z);
                this.transform.localScale = new Vector3(0.7654598f, 1.295f, 1.1142f);
                pointer = 0;
            }
        }
    }

    void OnMouseUp()
    {
        //if (drawing) {
        //    StartCoroutine("DelayCor");
        //    return;
        //}
        if (client)
        {
            if (GameObject.Find("MyDeck").transform.childCount < 3)
            {
                if (MakeCardCount <= 30)
                {

                    if (GameObject.Find("DataBook") == null)
                    {
                        if (GameSparksManager.instance != null)
                        {
                            int i = Random.Range(0, SparksDlist.Count);
                            num = (int)SparksDlist[i];
                            SparksDlist.Remove(num);

                            Debug.Log("num1 : " + num);

                        }
                        else
                        {
                            num = Random.Range(0, 10);
                            Debug.Log("DataBook이 없음");
                        }
                    }
                    else
                    {
                        int i = Random.Range(0, Dlist.Count);
                        num = (int)Dlist[i];
                        Dlist.Remove(num);

                        Debug.Log("num : " + num);
                        Debug.Log("실행한 후 여기 덱의 카드수는 : " + Dlist.Count);
                        foreach (int ii in Dlist)
                        { Debug.Log("element : " + ii); }
                    }

                    GameObject.Find("CardBackCube 1").transform.Find("Cube").transform.Find("Plane" + num).gameObject.SetActive(true);

                }
                else
                {
                    CancelInvoke();
                    StartCoroutine("NoDeckCor");

                }
            }
            else
            {
                CancelInvoke();
                StartCoroutine("TooManyCardCor");
                //3개보다 많으면
            }



            Debug.Log("num2 : " + num);

            if (iswhite == true && isWhiteTurn == true && this.gameObject == GameObject.Find("WDeck"))
            {
                if (GameObject.Find("ChessBoard").GetComponent<BoardManager>().HCardCount == 0)
                {
                    if (GameObject.Find("DataBook") != null)
                    {
                        if (Dlist.Count < 1)
                        {
                            this.gameObject.GetComponent<Make>().Dlist = (ArrayList)GameObject.Find("DataBook").GetComponent<Next>().DList.Clone();
                        }
                    }
                    InvokeRepeating("CardMove1", 0, 0.03f);
                    MakeCardCount += 1;
                    GameObject.Find("DrawCard").GetComponent<AudioSource>().Play();
                }
                else
                {
                    Debug.Log("카운트가 이미 있어서 뭘 만들지 않을거임");
                    GameObject.Find("CardBackCube 1").transform.Find("Cube").transform.Find("Plane" + num).gameObject.SetActive(false);
                    return;
                }
            }
            else if (iswhite == false && isWhiteTurn == false && this.gameObject == GameObject.Find("BDeck"))
            {
                if (GameObject.Find("ChessBoard").GetComponent<BoardManager>().PCardCount == 0)
                {
                    if (GameObject.Find("DataBook") != null)
                    {
                        if (Dlist.Count < 1)
                        {
                            this.gameObject.GetComponent<Make>().Dlist = (ArrayList)GameObject.Find("DataBook").GetComponent<Next>().DList.Clone();
                        }
                    }
                    InvokeRepeating("CardMove1", 0, 0.03f);
                    MakeCardCount += 1;
                    GameObject.Find("DrawCard").GetComponent<AudioSource>().Play();
                }
                else
                {
                    Debug.Log("카운트가 이미 있어서 뭘 만들지 않을거임"); GameObject.Find("CardBackCube 1").transform.Find("Cube").transform.Find("Plane" + num).gameObject.SetActive(false);
                    return;
                }
            }
            else { return; }
        }
        else if (this.gameObject == GameObject.Find("WDeck") || this.gameObject == GameObject.Find("BDeck"))
        {
            if (GameObject.Find("DataBook") != null)
            {
                if (Dlist.Count < 1)
                {
                    this.gameObject.GetComponent<Make>().Dlist = (ArrayList)GameObject.Find("DataBook").GetComponent<Next>().DList.Clone();
                }
            }
            InvokeRepeating("CardMove1", 0, 0.03f);
            MakeCardCount += 1;
            GameObject.Find("DrawCard").GetComponent<AudioSource>().Play();
        }
        else { }
    }

    public void MakeCard() //카드를 생성하는 부분 
    {
        Debug.Log("MakeCard");
        Debug.Log("num3 : " + num);
        switch (num)
        {
            case 0:
                CardObjectBl.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "증원군";
                CardObjectBl.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "자신의 영토에\r\n폰을 하나 소환한다";
                CardObjectBl.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C0") as Texture2D;
                CardObjectBl.transform.Find("RawImage").transform.Find("Lightning").gameObject.SetActive(true);
                NewCard = Instantiate(CardObjectBl);
                NewCard.GetComponent<draggable>().CardCase = 0;
                break;
            case 1:
                CardObjectG.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "기사도";
                CardObjectG.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "자신의 킹과 나이트의\r\n자리를 스왑한다";
                CardObjectG.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C1") as Texture2D;
                CardObjectBl.transform.Find("RawImage").transform.Find("Lightning").gameObject.SetActive(true);
                NewCard = Instantiate(CardObjectG);
                NewCard.GetComponent<draggable>().CardCase = 1;
                break;
            case 2:
                CardObjectBr.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "반전";
                CardObjectBr.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "자신의 비숍 하나를\r\n좌우로 한 칸 이동한다";
                CardObjectBr.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C2") as Texture2D;
                CardObjectBl.transform.Find("RawImage").transform.Find("Lightning").gameObject.SetActive(false);
                NewCard = Instantiate(CardObjectBr);
                NewCard.GetComponent<draggable>().CardCase = 2;
                break;
            case 3:
                CardObjectBl.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "명예훈장";
                CardObjectBl.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "폰 두개를 희생해\r\n새 나이트를\r\n성에 소환한다";
                CardObjectBl.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C3") as Texture2D;
                CardObjectBl.transform.Find("RawImage").transform.Find("Lightning").gameObject.SetActive(true);
                NewCard = Instantiate(CardObjectBl);
                NewCard.GetComponent<draggable>().CardCase = 3;
                break;
            case 4:
                CardObjectBr.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "방어";
                CardObjectBr.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "전장에 있는 룩을\r\n자신의 영토로\r\n이동시킨다";
                CardObjectBr.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C4") as Texture2D;
                CardObjectBl.transform.Find("RawImage").transform.Find("Lightning").gameObject.SetActive(false);
                NewCard = Instantiate(CardObjectBr);
                NewCard.GetComponent<draggable>().CardCase = 4;
                break;
            case 5:
                CardObjectBr.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "반격";
                CardObjectBr.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "공격당하고 있는\r\n폰을 상대 영토로\r\n이동시킨다";
                CardObjectBr.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C5") as Texture2D;
                CardObjectBl.transform.Find("RawImage").transform.Find("Lightning").gameObject.SetActive(false);
                NewCard = Instantiate(CardObjectBr);
                NewCard.GetComponent<draggable>().CardCase = 5;
                break;
            case 6:
                CardObjectG.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "희생";
                CardObjectG.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "체크 상황일 때,\r\n자신의 폰과 킹을\r\n스왑한다.";
                CardObjectG.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C6") as Texture2D;
                CardObjectBl.transform.Find("RawImage").transform.Find("Lightning").gameObject.SetActive(true);
                NewCard = Instantiate(CardObjectG);
                NewCard.GetComponent<draggable>().CardCase = 6;
                break;
            case 7:
                CardObjectG.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "승부수";
                CardObjectG.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "자신의 성에 있는\r\n나이트를 상대의\r\n나이트와 스왑한다.";
                CardObjectG.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C7") as Texture2D;
                CardObjectBl.transform.Find("RawImage").transform.Find("Lightning").gameObject.SetActive(false);
                NewCard = Instantiate(CardObjectG);
                NewCard.GetComponent<draggable>().CardCase = 7;
                break;
            case 8:
                CardObjectBl.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "매복";
                CardObjectBl.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "전장에 폰을 잡는 함정을\r\n4턴동안 설치한다.";
                CardObjectBl.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C8") as Texture2D;
                CardObjectBl.transform.Find("RawImage").transform.Find("Lightning").gameObject.SetActive(true);
                NewCard = Instantiate(CardObjectBl);
                NewCard.GetComponent<draggable>().CardCase = 8;
                break;
            case 9:
                CardObjectBr.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "격퇴";
                CardObjectBr.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "상대의 퀸을\r\n뒤로 한 칸 움직인다.";
                CardObjectBr.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C9") as Texture2D;
                CardObjectBl.transform.Find("RawImage").transform.Find("Lightning").gameObject.SetActive(false);
                NewCard = Instantiate(CardObjectBr);
                NewCard.GetComponent<draggable>().CardCase = 9;
                break;
        }
        NewCard.transform.localScale = new Vector3(2.3f, 3.6f, 0.01f);
        NewCard.transform.SetParent(GameObject.FindWithTag("Canvas").transform, false);

        if (iswhite)
        {
            NewCard.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("CardBackCube 1").transform.position);
            GameObject.Find("CardBackCube 1").transform.rotation = Quaternion.Euler(new Vector3(90, 0, -180));
            GameObject.Find("CardBackCube 1").transform.position = new Vector3(10.65f, -0.83f, 0.29f);
        }
        else
        {
            NewCard.transform.position = cam.WorldToScreenPoint(GameObject.Find("CardBackCube 1").transform.position);
            GameObject.Find("CardBackCube 1").transform.rotation = Quaternion.Euler(new Vector3(90, 0, -180));
            GameObject.Find("CardBackCube 1").transform.position = new Vector3(-2.76f, -0.83f, 7.92f);
        }

        if (client)
        {
            if (iswhite == true && isWhiteTurn == true)
            {
                GameObject.Find("CardBackCube 1").transform.position = new Vector3(10.65f, -0.83f, 0.29f);
            }
            else if (iswhite == false && isWhiteTurn == false)
            {
                GameObject.Find("CardBackCube 1").transform.position = new Vector3(-2.76f, -0.83f, 7.92f);
            }
            else { return; }
        }

        newC = Instantiate(CardObjectE);
        for (int i = 0; i < GameObject.Find("MyDeck").transform.childCount; i++)
        {
            GameObject.Find("MyDeck").transform.GetChild(i).transform.position = new Vector3(this.transform.position.x, -5, this.transform.position.z);
            GameObject.Find("MyDeck").transform.GetChild(i).transform.localScale = new Vector3(0.7654598f, 1.295f, 1.1142f);
        }
        newC.transform.SetParent(GameObject.Find("MyDeck").transform, false);
        LayoutElement le = newC.AddComponent<LayoutElement>();
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        InvokeRepeating("ToDeck", 1.5f, 0.000011f); // 2초뒤 0.02초주기로 ToDeck 반복 호출
        Debug.Log("num5 : " + num);
    }


    public void ToDeck() //만들어진 카드를 덱으로 이동시키는 부분  애니메이션
    {
        Vector3 dir = newC.transform.position - NewCard.transform.position;

        if (Mathf.Abs(dir.y) > 1.5)
        {
            NewCard.transform.localScale -= new Vector3(0.008f, 0.0129f, 0.0f);
            NewCard.transform.Translate(dir.normalized * 200 * Time.deltaTime, Space.World);
        }
        else
        {
            CancelInvoke();
            NewCard.transform.localScale = new Vector3(0.7f, 1.3f, 1.1142f);

            Destroy(newC.gameObject);

            for (int i = 0; i < GameObject.Find("MyDeck").transform.childCount; i++)
            {
                GameObject.Find("MyDeck").transform.GetChild(i).transform.position = new Vector3(this.transform.position.x, -5, this.transform.position.z);
                GameObject.Find("MyDeck").transform.GetChild(i).transform.localScale = new Vector3(0.7654598f, 1.295f, 1.1142f);
            }

            NewCard.transform.SetParent(GameObject.Find("MyDeck").transform, false);

            Debug.Log("num8 : " + num);
            Debug.Log(GameObject.Find("CardBackCube 1").transform.Find("Cube").transform.Find("Plane" + num));

            GameObject.Find("CardBackCube 1").transform.Find("Cube").transform.Find("Plane" + num).gameObject.SetActive(false);

        }

    }

    public void CardMove1()
    {

        if (GameObject.Find("MyDeck").transform.childCount < 3)
        {
            if (MakeCardCount <= 30)
            {
                drawing = true;

                if (iswhite)
                {
                    GameObject.Find("ChessBoard").GetComponent<BoardManager>().HCardCount += 1;
                    if (GameObject.Find("CardBackCube 1").transform.rotation != Quaternion.Euler(new Vector3(0, -90, -270)))
                    {
                        GameObject.Find("CardBackCube 1").transform.Translate(new Vector3(220 * 0.0045f, 0, 0));
                        GameObject.Find("CardBackCube 1").transform.Rotate(0, 10, 0);
                    }
                    else
                    {
                        CancelInvoke();
                        InvokeRepeating("CardMove2", 0, 0.03f);
                    }
                }
                else
                {

                    GameObject.Find("ChessBoard").GetComponent<BoardManager>().PCardCount += 1;
                    if (GameObject.Find("CardBackCube 1").transform.rotation != Quaternion.Euler(new Vector3(180, -90, -270)))
                    {
                        GameObject.Find("CardBackCube 1").transform.Translate(new Vector3(-220 * 0.0045f, 0, 0));
                        GameObject.Find("CardBackCube 1").transform.Rotate(0, -10, 0);
                    }
                    else
                    {
                        CancelInvoke();
                        InvokeRepeating("CardMove2", 0, 0.03f);
                    }
                }
            }
            else
            {
                CancelInvoke();
                StartCoroutine("NoDeckCor");
            }
        }
        else
        {
            CancelInvoke();
            StartCoroutine("TooManyCardCor");
            //3개보다 많으면
        }
    }
    public void CardMove2()
    {
        if (iswhite)
        {
            if (GameObject.Find("CardBackCube 1").transform.rotation != Quaternion.Euler(new Vector3(-90, -90, -270)))
            {
                GameObject.Find("CardBackCube 1").transform.Translate(new Vector3(50 * 0.0045f, 0, 0));
                GameObject.Find("CardBackCube 1").transform.Rotate(0, 9, 0);
            }
            else
            {
                CancelInvoke();
                InvokeRepeating("CardMove3", 0, 0.02f);
            }
        }
        else
        {
            if (GameObject.Find("CardBackCube 1").transform.rotation != Quaternion.Euler(new Vector3(270, -90, -270)))
            {
                GameObject.Find("CardBackCube 1").transform.Translate(new Vector3(-50 * 0.0045f, 0, 0));
                GameObject.Find("CardBackCube 1").transform.Rotate(0, -9, 0);
            }
            else
            {
                CancelInvoke();
                InvokeRepeating("CardMove3", 0, 0.02f);
            }
        }
    }
    public void CardMove3()
    {
        if (iswhite)
        {
            if (GameObject.Find("CardBackCube 1").transform.rotation != Quaternion.Euler(new Vector3(-130, 0, -360)))
            { GameObject.Find("CardBackCube 1").transform.Rotate(-4, 0, 0); }
            else
            {
                CancelInvoke();
                MakeCard();
            }
        }
        else
        {
            if (GameObject.Find("CardBackCube 1").transform.rotation != Quaternion.Euler(new Vector3(310, 0, -360)))
            { GameObject.Find("CardBackCube 1").transform.Rotate(4, 0, 0); }
            else
            { CancelInvoke(); MakeCard(); }
        }
    }

    IEnumerator NoDeckCor()
    {
        Debug.Log("NoDeckCor");

        GameObject Tx = GameObject.Find("CardWarningUI").transform.Find("NoDeckUI").gameObject;
        Tx.SetActive(true);
        yield return new WaitForSeconds(3f);
        Tx.SetActive(false);
    }
    IEnumerator TooManyCardCor()
    {
        GameObject Tx = GameObject.Find("CardWarningUI").transform.Find("TooManyCardUI").gameObject;
        Tx.SetActive(true);
        yield return new WaitForSeconds(3f);
        Tx.SetActive(false);
    }
    IEnumerator DelayCor()
    {
        yield return new WaitForSeconds(2.5f);
        drawing = false;
    }
}