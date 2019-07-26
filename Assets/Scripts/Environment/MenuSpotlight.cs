using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpotlight : MonoBehaviour
{
    Light lt;
    public float lerpTime = 0f;
    public float fadeTime = 3f;
    public float MaxAngle = 0f;
    public float MaxIntensity = 0f;
    public float WaittingTime = 0f;

    private float timer = 0f;

    public bool isMain = false;

    // Start is called before the first frame update
    void Start()
    {
        lt = GetComponent<Light>();
        lt.type = LightType.Spot;
        lt.spotAngle = 0f;
        lt.intensity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= WaittingTime)
        {
            lt.intensity = MaxIntensity;
            if (!isMain)
                controlLight();
            else
                controlLight2();
        }
    }

    private void controlLight()
    {
        lerpTime += (Time.deltaTime / fadeTime);
        lt.spotAngle = Mathf.Lerp(0f, MaxAngle, lerpTime);
    }

    private void controlLight2()
    {
        lt.spotAngle = MaxAngle;
        lerpTime += (Time.deltaTime / fadeTime);
        lt.intensity = Mathf.Lerp(0f, MaxIntensity, lerpTime);
    }
}
