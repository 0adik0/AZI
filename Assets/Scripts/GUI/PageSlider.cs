using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PageSlider : MonoBehaviour
{
    public PageSliderItem Item;
    public Button NextButton;
    public Button LastButton;
    public HistoryPanel HistoryPanel;
    public List<PageSliderItem> Items = new List<PageSliderItem>();
    private int _curPage = 0;
    private int _maxPages = 0;

    private int _sideStep = 0;
    private int _colPages = 0;
    private CanvasGroup _canvasGroupe;

    void Start()
    {
        _canvasGroupe = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// инициализируем список
    /// </summary>
    /// <param name="colPages"></param>
    /// <param name="maxPages">лучше указывать не четное число</param>
    public void Init(int colPages, int maxPages)
    {
        print("Init " + colPages + ":" + maxPages);
        _maxPages = maxPages;
        _colPages = colPages;
        PageSliderItem item;
        GameObject go;
        _sideStep = (int)Math.Floor((double)(maxPages / 2));
        foreach (PageSliderItem sliderItem in Items)
        {
            Destroy(sliderItem.gameObject);
        }
        for (int i = 1; i <= colPages; i++)
        {
            go = (GameObject)Instantiate(Item.gameObject);
            go.transform.SetParent(this.gameObject.transform);

            item = go.GetComponent<PageSliderItem>();
            item.Init(this, i);
            Items.Add(item);

        }
        if (colPages > 0)
        {
            _canvasGroupe.interactable = true;
            _canvasGroupe.alpha = 1;
            ShowPage(1);
        }
        else
        {
            _canvasGroupe.interactable = false;
            _canvasGroupe.alpha = 0;
        }
    }
    public void ShowPage(int page)
    {
        print("ShowPage " + page);
        HistoryPanel.ShowList();
        _curPage = page;
        CalcPos();
        Items[page - 1].Select();
    }

    public void ShowBtns(int startPos)
    {
        print("ShowBtns " + startPos);
        bool started = false;
        int left = _maxPages;
        for (int i = 1; i <= Items.Count; i++)
        {
            Items[i - 1].UnSelect();
            Items[i - 1].gameObject.SetActive(false);
            if (i == startPos)
            {
                started = true;
            }
            if (started && left > 0)
            {
                Items[i - 1].gameObject.SetActive(true);
                left--;
            }
        }
        NextButton.transform.SetAsLastSibling();
        LastButton.transform.SetAsLastSibling();
    }
    /// <summary>
    /// показываем выбранную страницу в центре списка, или с краю если это конец
    /// </summary>
    public void CalcPos()
    {
        print("CalcPos " + _sideStep + ":" + _curPage + ":" + _maxPages + ":" + _colPages);
        if (_maxPages >= _colPages)
        {
            //показываем все
            ShowBtns(1);
        }
        else
        {
            if (_curPage - _sideStep <= 1)
            {
                ShowBtns(1);
            }
            else if (_curPage + _sideStep > _colPages)
            {
                ShowBtns(_colPages - _maxPages + 1);
            }
            else
            {
                ShowBtns(_curPage - _sideStep);
            }
        }
    }
    public void ShowFirstPage()
    {
        ShowPage(1);
    }

    public void ShowLastPage()
    {
        ShowPage(_colPages);
    }

    public void ShowPrev()
    {
        if (_curPage - 1 >= 1)
        {
            ShowPage(_curPage - 1);
        }
    }

    public void ShowNext()
    {
        if (_curPage + 1 <= _colPages)
        {
            ShowPage(_curPage + 1);
        }
    }
}
