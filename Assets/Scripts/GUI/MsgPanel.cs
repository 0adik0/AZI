using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class MsgPanel : MonoBehaviour
{
    public Text Text;
    public Sprite GreySprite;
    public Sprite WhiteSprite;
    public Image FonImage;
    public void ShowMsgWhite(string str)
    {
        //gameObject.SetActive(true);
        //Text.text = str;
        //Text.color=Color.black;
        //FonImage.sprite = WhiteSprite;
        //Text.GetComponent<Shadow>().effectColor = Color.white;
    }
    public void ShowMsgGray(string str)
    {
        //gameObject.SetActive(true);
        //Text.text = str;
        //Text.color = new Color(180f,180f,180f,1f);
        //FonImage.sprite = GreySprite;
        //Text.GetComponent<Shadow>().effectColor=Color.black;
    }
    public void HideMsg()
    {
        gameObject.SetActive(false);
    }
}
