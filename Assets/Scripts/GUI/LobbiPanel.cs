using System.Collections.Generic;
using CodeTitans.JSon;
using Ucss;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ETableTip
{
    Noob,
    Prof,
    Vip
}
public class LobbiPanel : Menu
{
    public Text NoobsText;
    public Text ProfsText;
    public Text VipText;
    public Button AddmoneyButton;
    public CanvasGroup NoobFon;
    public CanvasGroup ProfFon;
    public CanvasGroup VIPFon;
    public Image NoobF;
    public Image ProfF;
    public Image VIPF;
    private ETableTip _tip;
    public float AlphaFon = 0.6f;
    public void InitPanel()
    {
        //DebugPanel.Log("22 ");
        _tip = ETableTip.Noob;
        // DebugPanel.Log("22 " + _canvasGroup);
        _canvasGroup.interactable = true;
        // DebugPanel.Log("222 " + GameController.Money);
        GetOnline();
        if (GameController.Money < 10)
            AddmoneyButton.gameObject.SetActive(true);
        else
            AddmoneyButton.gameObject.SetActive(false);
        // DebugPanel.Log("2222 ");
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        NoobFon.alpha = AlphaFon;
        ProfFon.alpha = AlphaFon;
        VIPFon.alpha = AlphaFon;
        NoobF.color = new Color(1f, 1f, 1f, 0f);
        ProfF.color = new Color(1f, 1f, 1f, 0f);
        VIPF.color = new Color(1f, 1f, 1f, 0f);
        switch (_tip)
        {
            case ETableTip.Noob:
                NoobFon.alpha = 1f;
                NoobF.color = new Color(1f, 1f, 1f, 1f);
                break;
            case ETableTip.Prof:
                ProfFon.alpha = 1f;
                ProfF.color = new Color(1f, 1f, 1f, 1f);
                break;
            case ETableTip.Vip:
                VIPFon.alpha = 1f;
                VIPF.color = new Color(1f, 1f, 1f, 1f);
                break;
        }
    }
    public void OnPlayNoob()
    {
        _tip = ETableTip.Noob;
        UpdateSelection();
        
    }

    public void OnPlayProf()
    {
        _tip = ETableTip.Prof;
        UpdateSelection();
       
    }

    public void OnPlayVip()
    {
        _tip = ETableTip.Vip;
        UpdateSelection();
        
    }

    public void OnClose()
    {
        GUIController.Instance.HideCurrentPanel();
    }

    public void OnAddMoney()
    {

    }

    public void OnStartSearch()
    {
        CancelInvoke("GetOnline");
        Net.JoinQueue(_tip);
        GUIController.Instance.HideCurrentPanel();
    }
    public void Error(string str)
    {
        Debug.LogError("str");
    }
    private void GetOnline()
    {
         CancelInvoke("GetOnline");
         WWWForm data = new WWWForm();
         data.AddField("table_type_id1", 1);
         UCSS.HTTP.PostForm(Net.ServerURL + "/api/default/getonline", data,
             new EventHandlerHTTPString(OnGetOnline),
             new EventHandlerServiceError(Net.Instance.OnHTTPError),
             new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
    }
    private void OnGetOnline(string text, string transactionid)
    {
        CancelInvoke("GetOnline");
        //    print("OnGetOnline " + text);
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            NoobsText.text = obj["beginner"].StringValue;
            ProfsText.text = obj["pro"].StringValue;
            VipText.text = obj["vip"].StringValue;
        }
        // Invoke("GetOnline", 5f);
    }
}
