using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickManager : MonoBehaviour
{
    GameObject CardDetail;

    // Start is called before the first frame update
    void Start()
    {
        CardDetail = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickCard()
    {
        CardDetail.gameObject.transform.localScale = new Vector3(1.636323f, 2.768322f, 2.381825f);
    }
}
