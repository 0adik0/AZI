using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class AziPanel : MonoBehaviour
{

    public Text MoneyText;
    private Menu _menu;

    void Awake()
    {
        _menu = GetComponent<Menu>();
    }

    public void Show(double sum)
    {
        MoneyText.text = GameController.LangManager.GetTextAsset("AziPanel.Msg").ToString()+sum + " $";
        _menu.IsOpen = true;
    }
    public void OnAccept()
    {
        _menu.IsOpen = false;
        Net.SendAziAnswer(true);
    }

    public void OnCancel()
    {
        _menu.IsOpen = false;
      //  GUIController.Instance.EndGame();
        Net.SendAziAnswer(false);
    }
}
