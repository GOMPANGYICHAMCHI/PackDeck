using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAdditionalPannel : MonoBehaviour
{
    public TMP_Text txt_a;
    public TMP_Text txt_b;
    public TMP_Text txt_c;

    public void SetText(float a,float b,float c)
    {
        txt_a.text = a.ToString("F1");
        txt_b.text = b.ToString("F1");
        txt_c.text = c.ToString("F1");
    }
}
