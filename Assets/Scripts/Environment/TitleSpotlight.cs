using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSpotlight : MonoBehaviour
{
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TitleCor");
    }

    IEnumerator TitleCor()
    {
        yield return new WaitForSeconds(time);
        transform.Find("Title").gameObject.SetActive(true);
    }
}
