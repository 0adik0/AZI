using System.Collections.Generic;
using CodeTitans.JSon;
using Ucss;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestPanel : MonoBehaviour
{
    public List<TestUser> Users;
    public int Inited = 0;
    private Hashtable Logins;
    public void Init()
    {
        Logins = new Hashtable();
        for (int i = 1; i <= 15; i++)
        {
            Check("user" + i);
        }
    }

    public void Check(string login)
    {
        WWWForm data = new WWWForm();
        data.AddField("username", login);

        string str = UCSS.HTTP.PostForm(Net.ServerURL + "/api/default/checkonline", data, new EventHandlerHTTPString(Answ), new EventHandlerServiceError(Net.Instance.OnHTTPError), new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
     //   print(login+" "+str);
        Logins.Add(str, login);
    }
    public void Answ(string text, string transactionid)
    {
        IJSonMutableObject obj = Net.ReadPack(text);
      //  print(text);
        if (obj != null)
        {
            if (Inited < 6)
            {
                if (obj["status"].StringValue == "offline")
                {
                    Users[Inited].gameObject.SetActive(true);
                    Users[Inited].Login = Logins[transactionid].ToString();
                    Users[Inited].Pass = Logins[transactionid].ToString();
                    Users[Inited].AuthButton.GetComponentInChildren<Text>().text = Logins[transactionid].ToString();
                    Inited++;
                }
            }
        }
    }
}
