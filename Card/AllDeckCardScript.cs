using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllDeckCardScript : CardBasic
{
    public GameObject coverObject;

    private void Start()
    {
        coverObject = transform.GetChild(2).gameObject;
    }

    public void ControlCover(bool isEnable)
    {
        coverObject.SetActive(isEnable);
    }
}
