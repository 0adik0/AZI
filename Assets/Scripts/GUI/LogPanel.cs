using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{

    public Text LogText;
    public RectTransform ContenTransform;
    public ScrollRect SR;
    public string ShowLog(List<string> str)
    {
        LogText.text = "";
        ContenTransform.transform.localPosition = new Vector3(ContenTransform.transform.localPosition.x, 0, 0);
        foreach (string s in str)
        {
            LogText.text += s + "\r\n-----------------\r\n";
        }
        SR.Rebuild(new CanvasUpdate());
        return LogText.text;
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
