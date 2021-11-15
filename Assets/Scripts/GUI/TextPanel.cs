using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class TextPanel : Menu
{

    public Text TopText;
    public Text Msg;

    public void OnClose()
    {
        GUIController.Instance.ShowLoginPanel();
    }

    public void ShowRuls()
    {
        TopText.text = GameController.LangManager.GetTextValue("Text.TopRuls");
        Msg.text = GameController.LangManager.GetTextValue("Text.Ruls");
    }
    public void ShowRulsIn()
    {
        TopText.text = GameController.LangManager.GetTextValue("Text.TopRulsIn");
        Msg.text = GameController.LangManager.GetTextValue("Text.RulsIn");
    }
    public void ShowRulsOut()
    {
        TopText.text = GameController.LangManager.GetTextValue("Text.TopRulsOut");
        Msg.text = GameController.LangManager.GetTextValue("Text.RulsOut");
    }
}
