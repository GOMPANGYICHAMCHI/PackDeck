using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinker : MonoBehaviour
{
    public float d;
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        d += Time.deltaTime;

        if(d >= 0.5f)
        {
            d = 0;
            image.enabled = !image.IsActive();
        }
    }
}
