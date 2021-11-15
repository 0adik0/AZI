using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnderlinedText : MonoBehaviour
{

    public Text PrimaryText;
    public Text UnderText;

    public void SetText(string str)
    {
        if (PrimaryText != null)
            PrimaryText.text = str;
        if (UnderText != null)
        {
            UnderText.text = "";
            foreach (char c in str)
            {
                UnderText.text += "_";
            }
        }
    }
}
