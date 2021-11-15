using SmartLocalization;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LangSwitcher : MonoBehaviour
{

    public Text LangText;
    private Slider _slider;
    public string CurLang = "ru";
    public void Start()
    {
        _slider = GetComponent<Slider>();
        ShowBtn();
    }

    public void OnChange()
    {
        print("OnChange "+_slider.value);
        if (_slider.value <1)
        {
            OnEng();
        }
        else
        {
            OnRu();
        }
    }
    public void ShowBtn()
    {
        if (CurLang == "ru")
        {
            LangText.text = "Ru";
            //  _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        }
        else
        {
            LangText.text = "En";
        }
    }

    public void OnRu()
    {
        CurLang = "en";
        ShowBtn();
        GUIController.Instance.RegPanel.SendMessage("HideErrorMsg", SendMessageOptions.DontRequireReceiver);
        GameController.LangManager.ChangeLanguage(CurLang);
    }
    public void OnEng()
    {
        CurLang = "ru";
        ShowBtn();
        GUIController.Instance.RegPanel.SendMessage("HideErrorMsg", SendMessageOptions.DontRequireReceiver);
        GameController.LangManager.ChangeLanguage(CurLang);
    }
}
