using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool WTurn;

    public GameObject NewCard;
    public GameObject CardObjectBl;
    public GameObject CardObjectE;
    public GameObject CardObjectG;
    public GameObject CardObjectBr;

    public int CardCase;
    public int Data = 0;

    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;

    public GameObject newC = null;
    public GameObject placeholder = null;

    public Text MainText;
    public Text Text;
    private Client client;

    public bool isWhite;

    private void Start()
    {
        client = FindObjectOfType<Client>();
        /*
        client = FindObjectOfType<Client>();
        if (client)
            isWhite = client.isHost;
            */
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 하기 위해서 클릭했을때
    {
        if (this.name.Contains("Card"))
        {
            this.transform.localScale = new Vector3(1.5308f, 2.6f, 1.1142f);
            GameObject.Find("MyDeck").GetComponent<draggable>().Data = 1 ;

            // placeholder이라는 이름으로 GameObject를 생성한다.
            placeholder = new GameObject();
            placeholder.transform.SetParent(this.transform.parent);
            LayoutElement le = placeholder.AddComponent<LayoutElement>();
            le.preferredWidth = this.GetComponent<ILayoutElement>().preferredWidth;
            le.preferredHeight = this.GetComponent<ILayoutElement>().preferredHeight;
            le.flexibleWidth = 0;
            le.flexibleHeight = 0;

            placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex()); //Sibling 중에서 자신의 위치를 가지고 온다

            parentToReturnTo = this.transform.parent;
            placeholderParent = parentToReturnTo;
            this.transform.SetParent(this.transform.parent.parent.parent);  // 위치를 parent로 옮긴다

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)  // 드래그 하기 위해서 클릭한 후 움직일때
    {
        if (this.name.Contains("Card"))
        {
            this.transform.position = eventData.position; //위치를 마우스의 위치로 수정

            this.transform.localScale = new Vector3(1.5308f, 2.6f, 1.1142f);

            if (placeholder.transform.parent != placeholderParent)  // placeholder의 부모 위치를 알맞게 수정
                placeholder.transform.SetParent(placeholderParent);

            int newSiblingIndex = placeholderParent.childCount;

            for (int i = 0; i < parentToReturnTo.childCount; i++)
            {  //위치찾기
                if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
                {
                    newSiblingIndex = i;

                    if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                        newSiblingIndex--;

                    break;
                }
            }

            placeholder.transform.SetSiblingIndex(newSiblingIndex);
        }
    }

    public void OnEndDrag(PointerEventData eventData) // 드래그 하기 위해서 클릭한 후 손가락 뗀 뒤
    {
        if (this.name.Contains("Card"))
        {
            GameObject.Find("MyDeck").GetComponent<draggable>().Data = 0;
            this.transform.SetParent(parentToReturnTo);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            this.transform.localScale = new Vector3(0.7654598f, 1.295f, 1.1142f); //크기 다시 작게

            Destroy(placeholder);  // 위치를 정해줬던 placeholder없애기

            if (this.transform.parent.name == "CardDel")  //카드 지울때
            { Destroy(this.gameObject); }


            //////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////
            if (this.transform.parent.name == "CardDropArea")  //체스판 위에 카드를 놓았을때
            {
                CheckCard();
            }
            else if (this.transform.parent.name == "ShowCard") { CheckTurnEnd("fail", this.gameObject); }

            

        }

    }

    public void CheckCard()
    {
        

        if (GameObject.Find("MyDeck").transform.childCount > 3)
        {

            StartCoroutine("TooManyCardCor");

            CheckTurnEnd("fail", this.gameObject);

        }
        else 
        {
            //GameObject.Find("ChessBoard").GetComponent<BoardManager>().EndTurn();                     //턴 넘기기     
            //this.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

            //순서 바꿈

            this.gameObject.transform.SetParent(GameObject.Find("ShowCard").transform, false);              //카드 위치를 ShowCard로
            this.gameObject.transform.localScale = new Vector3(1.636323f, 2.768322f, 2.381825f);             //카드 크기를 크게
            GameObject.FindWithTag("ChessBoard").GetComponent<CardManager>().ActiveCard(CardCase);
        }

    }


    public void CheckTurnEnd(string CheckTE, GameObject gameObj) //카드를 없애냐 돌려보내냐 결정하는 함수
    {
        switch (CheckTE)
        {
            case "fail": //조건을 만족하지 못해서 다시 손 안으로 돌아가게

                gameObj.transform.SetParent(GameObject.Find("MyDeck").transform, false);                      
                gameObj.transform.localScale = new Vector3(0.7654598f, 1.295f, 1.1142f);
                break;

            case "pass":  //조건을 만족해서 카드 삭제

                Destroy(this.gameObject, 1.5f);            
                break;
        }
        
    }

    IEnumerator TooManyCardCor()
    {
        GameObject Tx = GameObject.Find("CardWarningUI").transform.Find("TooManyCardUI").gameObject;
        Tx.SetActive(true);
        yield return new WaitForSeconds(3f);
        Tx.SetActive(false);
    }
}
