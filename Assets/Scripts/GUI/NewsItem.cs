using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class NewsItem : MonoBehaviour
{

    public Text TopText;
    public Text BodyText;
    public Image SeparatorImage;

    public void Init(string top, string date, string body)
    {
        TopText.text = "<b><size=50>" + top + "</size> </b>\n";
        TopText.text += "<size=50>" +body+"</size>";
    }
}
