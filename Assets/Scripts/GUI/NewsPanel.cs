using System.Collections.Generic;

using CodeTitans.JSon;

using Ucss;

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class NewsPanel : Menu
{
    public Text TopText;
    public VerticalLayoutGroup Content;
    public void OnClose()
    {
        GUIController.Instance.ShowLoginPanel();
    }

   
    private string _str =
        "{\"news\":[{\"top\":\"1новость\",\"date\":\"10.01.2015\",\"body\":\"тут описание \\nновости\"},{\"top\":\"2новость\",\"date\":\"10.01.2015\",\"body\":\"тутописаниеновости<b>внесколькострок</b>\"},{\"top\":\"2новость\",\"date\":\"10.01.2015\",\"body\":\"тутописаниеновости<b>внесколькострок</b>\"},{\"top\":\"2новость\",\"date\":\"10.01.2015\",\"body\":\"тутописаниеновости<b>внесколькострок</b>\"}]}";
    /// <summary>
    /// запрашиваем новости
    /// </summary>
   
    public  void GetNews()
    {
        print("[NET]GetNews ");
        GUIController.ShowLoading();
        if (Net.Instance.LocalTest)
        {
            OnGetNews(_str, "123");
        }
        else
        {
            WWWForm data = new WWWForm();
            data.AddField("lang", GameController.LangManager.LoadedLanguage);
            UCSS.HTTP.PostForm(Net.ServerURL + "/api/default/news", data, new EventHandlerHTTPString(OnGetNews),
                new EventHandlerServiceError(OnHTTPError),
                new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
        }
    }
    private void OnGetNews(string text, string transactionid)
    {
        GUIController.HideLoading();
        IJSonMutableObject obj = Net.ReadPack(text);
        TopText.text = "";
        if (obj != null)
        {
            if (obj.Contains("news"))
            {
               //получили новости парсим
                IJSonMutableObject news = obj["news"].AsMutable;
                IJSonObject item;
                
                for (int i = 0; i < news.Count; i++)
                {
                    item = news[i];
                    TopText.text += "<b><size=30>" + item["top"].StringValue + "</size></b>\n";
                    TopText.text +=item["date"].StringValue + "\n";
                    TopText.text += item["body"].StringValue + "\n";
                  
                }
                //Content.CalculateLayoutInputVertical();
            }
            else
            {
                Debug.LogError("ошибка получения новостей");
            }
        }
      

    }
    void OnHTTPError(string error, string transactionId)
    {
        GUIController.HideLoading();
        TopText.text = "";
        Debug.LogError("[RegPanel] [OnHTTPError] error = " + error + " (transaction [" + transactionId + "])");
    }

    void OnHTTPTimeOut(string transactionId)
    {
        GUIController.HideLoading();
        TopText.text = "";
        Debug.LogError("[RegPanel] [OnHTTPTimeOut] transactionId = " + transactionId);
        UCSS.HTTP.RemoveTransaction(transactionId);
    }
}
