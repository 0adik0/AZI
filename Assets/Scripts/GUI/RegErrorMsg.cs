using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegErrorMsg : MonoBehaviour, IPointerClickHandler
{

    public Text MsgText;
    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("Button hint" + data.pointerId + " up");
    }
    /// <summary>
    /// показываем панель с ошибкой
    /// </summary>
    /// <param name="field"></param>
    public void ShowError(RegErrorField field)
    {
        MsgText.text = field.Msg;
    }

    public void HideError()
    {
        gameObject.SetActive(false);
    }
}
