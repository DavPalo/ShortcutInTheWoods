using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public string txt = "";
    public Text message;
    public bool txtChange = false;
    void Update()
    {
        if (txtChange) {
            StartCoroutine(HideText());
        }

    }

    IEnumerator HideText()
    {
        message.text = txt;
        yield return new WaitForSeconds(5);
        txtChange = false;
        message.text = "";
    }
}
