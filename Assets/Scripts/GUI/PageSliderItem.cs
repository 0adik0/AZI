using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSliderItem : MonoBehaviour, IPointerClickHandler
{
    public int Page;
    private PageSlider _pageSlider;
    public Text Text;
    public void Init(PageSlider ps, int page)
    {
        Page = page;
        _pageSlider = ps;
        Text.text = page.ToString();
    }
    public void OnPointerClick(PointerEventData data)
    {
        _pageSlider.ShowPage(Page);
        Select();
    }

    public void Select()
    {
        Text.fontStyle=FontStyle.Bold;
    }
    public void UnSelect()
    {
        Text.fontStyle = FontStyle.Normal;
    }
}
