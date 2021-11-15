using System;
using System.Collections.Generic;
using CodeTitans.JSon;
using Ucss;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HistoryPanel : Menu
{

    public HistoryItem HistoryI;
    public List<HistoryItem> News;
    public VerticalLayoutGroup Content;
    private string _str = "{\"history\":[{\"tip\":\"in\",\"date\":\"01.10.201123:23:23\",\"sys\":\"Yandex\",\"sum\":\"356.3\",\"status\":\"end\"},{\"tip\":\"out\",\"date\":\"11.10.201123:23:23\",\"sys\":\"WM\",\"sum\":\"6.3\",\"status\":\"inwork\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"},{\"tip\":\"out\",\"date\":\"12.10.201123:23:23\",\"sys\":\"QIWI\",\"sum\":\"356.3\",\"status\":\"decline\"}]}";
    private IJSonMutableObject _items;
    public DateSelector DateSelector;
    public Text Date1Text;
    public Text Date2Text;

    public void OnDateSelect1()
    {
        DateSelector.Init(true);
        DateSelector.TargetField = Date1Text;
        DateSelector.Separator = "-";
        //print(DateSelector.TopText.text);
        //print(GameController.LangManager.GetTextValue("Selecter.Top").ToString());
        //print("DateSelector.TopText " + GameController.LangManager.GetTextValue("Selecter.Top"));
        DateSelector.TopText.text = GameController.LangManager.GetTextValue("Selecter.Top") +" "+
                               GameController.LangManager.GetTextValue("History.From").ToString();
        DateSelector.gameObject.transform.localPosition = Vector3.zero;
        DateSelector.gameObject.SetActive(true);
    }
    public void OnDateSelect2()
    {
        DateSelector.Init(true);
        DateSelector.TargetField = Date2Text;
        DateSelector.TopText.text = GameController.LangManager.GetTextValue("Selecter.Top").ToString() +" "+
                             GameController.LangManager.GetTextValue("History.To").ToString();
        DateSelector.Separator = "-";
        DateSelector.gameObject.transform.localPosition = Vector3.zero;
        DateSelector.gameObject.SetActive(true);
    }

    public void OnDate1Changed()
    {
        Invoke("OnDateSelect2",0.1f);
    }
    public void OnDate2Changed()
    {
        GetHistory();
    }
    public void ShowList()
    {
       // page--;
        foreach (HistoryItem item1 in News)
        {
            Destroy(item1.gameObject);
        }

        IJSonObject item;
        News = new List<HistoryItem>();
        HistoryItem ni;

        for (int i = 0; i < _items.Count; i++)
        {
            if (i < _items.Count)
            {
                item = _items[i];
                GameObject go = (GameObject)Instantiate(HistoryI.gameObject);
                go.SetActive(true);
                go.GetComponent<RectTransform>().SetParent(Content.GetComponent<RectTransform>());
                ni = go.GetComponent<HistoryItem>();
                News.Add(ni);
                ni.Init(item["tip"].StringValue, item["date"].StringValue, item["sys"].StringValue,
                    item["sum"].StringValue, item["status"].StringValue);
            }
        }
        Content.CalculateLayoutInputVertical();

    }
    public void Start()
    {

    }
    /// <summary>
    /// запрашиваем новости
    /// </summary>
    public void GetHistory()
    {
        print("[NET]GetNews ");
        GUIController.ShowLoading();
       
          if (Net.Instance.LocalTest)
           {
               OnGetHistory(_str, "123");
           }
           else
           {
               WWWForm data = new WWWForm();
               print("history " + Date1Text.text + "  " + Date2Text.text);
               data.AddField("date_from", Date1Text.text);
               data.AddField("date_to", Date2Text.text);
               UCSS.HTTP.PostForm(Net.ServerURL + "/api/default/payment-history?access-token="+GameController.Token, data, new EventHandlerHTTPString(OnGetHistory),
                   new EventHandlerServiceError(OnHTTPError),
                   new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
           }
    }
    private void OnGetHistory(string text, string transactionid)
    {
        print(text);
        //foreach (GameObject o in Btns)
        //{
        //    o.SetActive(false);
        //}
        IJSonMutableObject obj = Net.ReadPack(text);
        //_curPage = 1;
        if (obj != null)
        {
            if (obj.Contains("history"))
            {
                //получили новости парсим
                _items = obj["history"].AsMutable;


                //определяем сколько страниц показываем

               // _totalPages = (int)(Math.Floor((double)(_items.Count / _itemsOnPage)) + 1);
               // PageSlider.Init(_totalPages, 5);
                ShowList();
            }
            else
            {
                Debug.LogError("ошибка получения новостей");
            }
        }
        GUIController.HideLoading();

    }
    void OnHTTPError(string error, string transactionId)
    {
        Debug.LogError("[RegPanel] [OnHTTPError] error = " + error + " (transaction [" + transactionId + "])");
    }

    void OnHTTPTimeOut(string transactionId)
    {
        Debug.LogError("[RegPanel] [OnHTTPTimeOut] transactionId = " + transactionId);
        UCSS.HTTP.RemoveTransaction(transactionId);
    }

    public void Init()
    {
        Date1Text.text = "01-01-2015";
        Date2Text.text = "30-12-2050";
        GetHistory();
    }
}
