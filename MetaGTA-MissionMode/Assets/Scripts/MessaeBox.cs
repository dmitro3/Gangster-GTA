using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MessaeBox : MonoBehaviour
{
    public static MessaeBox insta;
    [SerializeField] GameObject msgBoxUI;
    [SerializeField] GameObject okBtn;
    [SerializeField] TMP_Text msgText;



    private void Awake()
    {
        insta = this;
        msgBoxUI.SetActive(false);
    }
    public void showMsg(string _msg, bool showBtn)
    {
        StopAllCoroutines();

        msgBoxUI.SetActive(true);
        if (showBtn) okBtn.SetActive(true);
        else okBtn.SetActive(false);

        msgText.text = _msg;

        StartCoroutine(WaitToShowOk());
    }


    IEnumerator WaitToShowOk()
    {
        if (!okBtn.activeSelf)
        {
            yield return new WaitForSeconds(40);
            okBtn.SetActive(true);
        }

    }

    public void OkButton()
    {
        StopAllCoroutines();
        msgBoxUI.SetActive(false);
    }

    internal void hideMsg()
    {
        msgBoxUI.SetActive(false);
    }
}
