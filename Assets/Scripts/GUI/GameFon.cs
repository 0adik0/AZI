using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameFon : MonoBehaviour
{
    public Image Noob;
    public Image Prof;
    public Image Vip;

    void Awake()
    {
        //ShowFon("");
    }
    public void ShowFon(string tip)
    {
        Noob.enabled = false;
        Prof.enabled = false;
        Vip.enabled = false;
        switch (tip)
        {
           default:
                Noob.enabled = true;
                break;
            case "2":
                Prof.enabled = true;
                break;
            case "3":
                Vip.enabled = true;
                break;
        }
    }

    public void HideFon()
    {
        Noob.enabled = false;
        Prof.enabled = false;
        Vip.enabled = false;
    }
}
