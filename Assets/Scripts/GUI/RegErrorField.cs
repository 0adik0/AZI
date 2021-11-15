using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegErrorField : MonoBehaviour, IPointerClickHandler
{
    public bool ActiveError = false;
    public string Msg = "";
    private Text _text;
    public RegErrorMsg MsgPanel;
    void Start()
    {
        _text = GetComponent<Text>();
    }
    public void SetError(string txt)
    {
        if (txt == "")
        {
            RemoveError();
        }
        else
        {
            Msg = txt;
            _text.color = Color.red;
            ActiveError = true;    
        }
        
    }

    public void RemoveError()
    {
        _text.color=Color.black;
        ActiveError = false;
    }
    public void OnPointerClick(PointerEventData data)
    {
        if (ActiveError)
        {
            MsgPanel.gameObject.SetActive(true);
            MsgPanel.ShowError(this);
        }
        else
            MsgPanel.HideError();
    }

}
