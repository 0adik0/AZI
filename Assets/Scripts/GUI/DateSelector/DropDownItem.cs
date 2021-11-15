using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DropDownItem : MonoBehaviour
{
    public int Id;
    public Button ButtonInstance;
    public DateSelector Panel;
    public Text LabelText;
    private LayoutElement LE;
    public void Start()
    {
        ButtonInstance = GetComponent<Button>();
        //LE = GetComponent<LayoutElement>();
        //LE.CalculateLayoutInputVertical();
        gameObject.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
    }
   /* public void OnClick()
    {
        switch (Tip)
        {
            case ETip.Date:
                Panel.SetActiveItemDay(Id);
                LabelText.fontStyle = FontStyle.Bold;
                break;
            case ETip.Month:
                Panel.SetActiveItemMonth(Id);
                LabelText.fontStyle = FontStyle.Bold;
                break;
            case ETip.Year:
                Panel.SetActiveItemYear(Id);
                LabelText.fontStyle = FontStyle.Bold;
                break;
        }

    }*/
}
