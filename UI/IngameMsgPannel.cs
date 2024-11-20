using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameMsgPannel : MonoBehaviour
{
    public TMP_Text txt_message;

    public Animator animator;

    private void Start()
    {

    }

    public void PrintMessage(string  message)
    {
        txt_message.text = message;
        //anim.Stop();
        //anim.Play();
        animator.SetTrigger("IsOn");
    }
}
