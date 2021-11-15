using System.Collections.Generic;
using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class DebugPanel : Menu
{
    private List<string> logs= new List<string>();
    public Text Msg;
    public int MaxLogs = 30;
    private static DebugPanel _instance;
    public GameObject LogGameObject;
    private void Start()
    {
        _instance = this;
        transform.localPosition = Vector3.zero;
    }
    public static void Log(string str)
    {
        _instance.logs.Insert(0, str);
        _instance.Msg.text = "";
        //print(_instance.logs.Count);
        foreach (string s in _instance.logs)
        {
            _instance.Msg.text += s + "\n";
        }
    }
    public void OnClose()
    {
        LogGameObject.SetActive(false);
    }


    public void OnOpen()
    {
        LogGameObject.SetActive(true);
    }
}
