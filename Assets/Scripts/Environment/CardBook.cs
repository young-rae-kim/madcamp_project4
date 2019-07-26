using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBook : MonoBehaviour
{
    RawImage CardImage;
    Text CardInfo;
    public Texture Card0;
    public Texture Card1;
    public Texture Card2;
    public Texture Card3;
    public Texture Card4;
    public Texture Card5;
    public Texture Card6;
    public Texture Card7;
    public Texture Card8;
    public Texture Card9;

    // Start is called before the first frame update
    void Start()
    {
        CardImage = GameObject.Find("CardImage").gameObject.GetComponent<RawImage>();
        CardInfo = GameObject.Find("CardInfo").gameObject.GetComponent<Text>();
        Index0();
    }

    public void Index0()
    {
        CardImage.texture = Card0;
        CardInfo.text = "\"왕과 나라를 위하여.\"\r\n\r\n"
            + "자신의 영토에 새로운 폰을 소환합니다.\r\n영토에 공간이 없을 경우 사용할 수 없습니다.\r\n\r\n"
            + "팁: 가장 기본이 되는 카드입니다. \r\n다른 기물을 소환하기 위해서라도 많이 확보하는게 좋습니다.\r\n속공카드로써, 카드 사용 후 기물을 움직일 수 있습니다.";
    }
    public void Index1()
    {
        CardImage.texture = Card1;
        CardInfo.text = "\"기사도? \r\n양아치 새끼들의 양아치 규칙?\"\r\n\r\n"
            + "자신의 킹과 나이트를 스왑합니다.\r\n나이트가 없는 경우 사용할 수 없습니다.\r\n\r\n"
            + "팁: 킹이 위험에 빠졌을 때 유용한 카드입니다.\r\n속공카드로써, 카드 사용 후 기물을 움직일 수 있습니다. 나이트의 기동력을 살려서 운영해보세요.";
    }
    public void Index2()
    {
        CardImage.texture = Card2;
        CardInfo.text = "\"나라를 움직이는 것은, \r\n때로는 왕이 아닌 주교이다.\"\r\n\r\n"
            + "자신의 비숍 하나를 좌우로 한 칸 이동합니다.\r\n비숍이 없거나 좌우 공간이 없는 경우 사용할 수 없습니다.\r\n\r\n"
            + "팁: 비숍의 타일 색깔을 바꿀 수 있습니다. \r\n비숍의 영향력이 커지는 후반에 중요한 카드입니다.";
    }
    public void Index3()
    {
        CardImage.texture = Card3;
        CardInfo.text = "\"세상에는 돈으로 살 수 없는게 있지.\"\r\n\r\n"
            + "폰을 두 개 희생하여 자신의 성에 나이트를 소환합니다.\r\n폰이 부족하거나 성에 자리가 없을 경우 사용할 수 없습니다.\r\n\r\n"
            + "팁: 나이트는 독특한 움직임으로 허를 찌를 수 있습니다. 많은 나이트는 빠른 승리로 이어지곤 합니다.\r\n속공카드로써, 카드 사용 후 기물을 움직일 수 있습니다.";
    }
    public void Index4()
    {
        CardImage.texture = Card4;
        CardInfo.text = "\"난공불락이라는 말 들어봤나?\"\r\n\r\n"
            + "전장에 있는 룩을 자신의 영토로 이동합니다.\r\n룩이 없거나 영토에 공간이 없을 경우 사용할 수 없습니다.\r\n\r\n"
            + "팁: 성과 영토가 위험해질 때 이 카드를 써서 위기를 모면하세요.";
    }
    public void Index5()
    {
        CardImage.texture = Card5;
        CardInfo.text = "\"체스에서 가장 무서운건, \r\n마지막에 남은 폰 하나인 법.\"\r\n\r\n"
            + "공격당하고 있는 자신의 폰을 상대 영토로 이동합니다.\r\n공격당하고 있는 폰이 없거나 상대 영토에 자리가 없으면 사용할 수 없습니다.\r\n\r\n"
            + "팁: 발동이 쉬운 가장 강력한 카드 중에 하나입니다. \r\n성에 있는 상대의 주요 기물을 압박하거나, 프로모션을 노려서 형세를 역전시켜보세요.";
    }
    public void Index6()
    {
        CardImage.texture = Card6;
        CardInfo.text = "\"내가 왕이로소이다.\"\r\n\r\n"
            + "자신의 킹이 체크당했을 때 자신의 폰과 스왑합니다.\r\n폰이 없을 경우 사용할 수 없습니다.\r\n\r\n"
            + "팁: 킹을 지키기 좋은 카드지만 안전이 확보된 폰과 스왑하는걸 명심하세요.\r\n속공카드로써, 카드 사용 후 기물을 움직여 반격을 꾀할 수 있습니다.";
    }
    public void Index7()
    {
        CardImage.texture = Card7;
        CardInfo.text = "\"주사위는 던져졌다.\"\r\n\r\n"
            + "자신의 성에 있는 나이트와 상대 나이트를 선택하여 스왑합니다..\r\n상대에 나이트가 없거나, 자신의 성에 나이트가 없는 경우 사용할 수 없습니다.\r\n\r\n"
            + "팁: 일격을 가할 수 있는 카드지만, 자신과는 다르게 상대의 나이트는 무조건 자신의 성에 들어온다는 것을 명심하세요.";
    }
    public void Index8()
    {
        CardImage.texture = Card8;
        CardInfo.text = "\"젠장, 기습이...\"\r\n\r\n"
            + "전장에 상대 폰을 파괴하는 함정을 자신 기준 4턴 동안 설치합니다.\r\n기물이 없는 곳에만 사용할 수 있고, 상대는 함정 위치를 알 수 없습니다. 4턴이 지난 후에는 자동으로 파괴됩니다.\r\n\r\n"
            + "팁: 상대 폰의 움직임을 예측하여 설치해보세요.\r\n속공카드로써, 카드 사용 후 기물을 움직일 수 있습니다. 자신이 움직일 곳에 함정을 설치하고, 그곳으로 움직이는 전략도 있습니다.";
    }
    public void Index9()
    {
        CardImage.texture = Card9;
        CardInfo.text = "\"일단은 물러나도록 하지.\"\r\n\r\n"
            + "상대방의 퀸을 선택하여 한 칸 뒤로 이동시킵니다.\r\n상대 퀸이 없거나, 뒤로 물러날 자리가 없는 경우 사용할 수 없습니다.\r\n\r\n"
            + "팁: 상대 퀸의 압박으로부터 벗어나게 해주는 카드입니다. \r\n상대 퀸을 자신이 유리한 위치로 옮겨보세요.";
    }
    public void ExitBook()
    {
        gameObject.SetActive(false);
    }
}
