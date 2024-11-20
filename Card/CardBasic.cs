using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardBasic : MonoBehaviour
{
    public Card cardData;

    public Image selfImage;
    public TMP_Text selfText;

    public void SetCardApear(Sprite img, Color32 color, int number)
    {
        selfImage.sprite = img;
        selfImage.color = color;
        selfText.text = number.ToString();
    }

    public void SetNumber(int number)
    {
        selfText.text = number.ToString();
    }

    public void SetImage(Sprite img)
    { 
        selfImage.sprite = img; 
    }

    public void SetColor(Color32 color)
    { 
        selfText.color = color;
    }
}
