using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCard : MonoBehaviour
{
    public GameObject[] Cards;
    // Start is called before the first frame update
    void Start()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject Card in Cards)
        {
            int num = Card.GetComponent<Setting>().Picknum;
            
            switch (num)
            {
                case 0:
                    Card.gameObject.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "증원군";
                    Card.gameObject.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "자신의 영토에\r\n폰을 하나 소환한다";
                    Card.gameObject.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C0") as Texture2D;
                    break;
                case 1:
                    Card.gameObject.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "기사도";
                    Card.gameObject.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "자신의 킹과 나이트의\r\n자리를 스왑한다";
                    Card.gameObject.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C1") as Texture2D;
                    break;
                case 2:
                    Card.gameObject.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "반전";
                    Card.gameObject.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "자신의 비숍 하나를\r\n좌우로 한 칸 이동한다";
                    Card.gameObject.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C2") as Texture2D;
                    break;
                case 3:
                    Card.gameObject.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "명예훈장";
                    Card.gameObject.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "폰 두개를 희생해\r\n새 나이트 성에 소환한다";
                    Card.gameObject.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C3") as Texture2D;
                    break;
                case 4:
                    Card.gameObject.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "방어";
                    Card.gameObject.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "전장에 있는 룩을\r\n자신의 영토로\r\n이동시킨다";
                    Card.gameObject.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C4") as Texture2D;
                    break;
                case 5:
                    Card.gameObject.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "반격";
                    Card.gameObject.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "공격당하고 있는\r\n폰을 상대 영토로\r\n이동시킨다";
                    Card.gameObject.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C5") as Texture2D;
                    break;
                case 6:
                    Card.gameObject.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "희생";
                    Card.gameObject.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "체크 상황일 때,\r\n자신의 폰과 킹을\r\n스왑한다.";
                    Card.gameObject.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C6") as Texture2D;
                    break;
                case 7:
                    Card.gameObject.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "승부수";
                    Card.gameObject.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "자신의 성에 있는\r\n나이트를 상대의\r\n나이트와 스왑한다.";
                    Card.gameObject.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C7") as Texture2D;
                    break;
                case 8:
                    Card.gameObject.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "매복";
                    Card.gameObject.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "전장에 폰을 잡는 함정을\r\n4턴동안 설치한다.";
                    Card.gameObject.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C8") as Texture2D;
                    break;
                case 9:
                    Card.gameObject.transform.Find("TitleImage").Find("CardTitle").GetComponent<Text>().text = "격퇴";
                    Card.gameObject.transform.Find("DesscriptionImage").Find("CardDescription").GetComponent<Text>().text = "상대의 퀸을\r\n뒤로 한 칸 움직인다.";
                    Card.gameObject.transform.Find("RawImage").Find("MainImage").GetComponent<RawImage>().texture = Resources.Load("Images/C9") as Texture2D;
                    break;
            }
        }
    }

}
