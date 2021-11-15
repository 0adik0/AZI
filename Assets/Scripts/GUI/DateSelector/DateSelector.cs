using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class DateSelector : MonoBehaviour
{
    /// <summary>
    /// ссылки на конпки
    /// </summary>
    private List<DropDownItem> _buttonsListDay;
    private List<DropDownItem> _buttonsListMonth;
    private List<DropDownItem> _buttonsListYear;
    /// <summary>
    /// ссылка на панель с кнопками
    /// </summary>
    public Image DropDownListDay;
    public Image DropDownListMonth;
    public Image DropDownListYear;

    public string Separator = ".";
    /// <summary>
    /// ссылка на эталонный элемент списка
    /// </summary>
    public GameObject ItemLink;

    private int ActiveItemDayID = 0;
    private string ActiveItemDayName = "1";
    private int ActiveItemMonthID = 0;
    private string ActiveItemMonthName = "1";
    private int ActiveItemYearID = 0;
    private string _activeItemYearName = "2000";
    private string ActiveItemYearName
    {
        get { return _activeItemYearName; }
        set
        {
            _activeItemYearName = value;
            print("_activeItemYearName "+value);
        }
    }

    public int StartYear = 17;
    public VerticalLayoutGroup ContentDay;
    public VerticalLayoutGroup ContentMonth;
    public VerticalLayoutGroup ContentYear;
    public ScrollPos YearssScrollPos;
    /// <summary>
    /// ссылка на поле в которое передадим итоговое значение
    /// </summary>
    public Text TargetField;
    public Image MaskImage;
    public Text TopText;
    public List<string> YearsList { get; set; }

    public void Init(bool fromNow=false)
    {

        DateList = new List<string>();
        DateList.Add("");
        for (int i = 1; i <= 31; i++)
        {
            DateList.Add(i.ToString());
        }
        DateList.Add("");
        InitDayList(DateList);
        MonthList = new List<string>();
        MonthList.Add("");
        for (int i = 1; i <= 12; i++)
        {
            MonthList.Add(i.ToString());
        }
        MonthList.Add("");
        InitMonthList(MonthList);
        print("!!init "+ YearsList);
        YearsList = new List<string>();
        YearsList.Add("");
        var n = System.DateTime.Now;
        int y = 0;
        if (fromNow)
        {
            y = n.Year-2000;
            for (int i = StartYear; i <= y; i++)
            {
                YearsList.Add(i.ToString());
            }
            ActiveItemYearName = n.Year.ToString();
        }
        else
        {
            y = n.Year - 2000-StartYear;
            for (int i = y; i >= 0; i--)
            {
                if (i < 10)
                    YearsList.Add("0" + i.ToString());
                else
                    YearsList.Add(i.ToString());
            }
            for (int i = 99; i > 60; i--)
            {
                YearsList.Add(i.ToString());
            }
            ActiveItemYearName = (n.Year - StartYear).ToString();
        }
        
       
        //for (int i = 01; i <= 79; i++)
        //{
        //    if (i < 10)
        //        YearsList.Add("0" + i.ToString());
        //    else
        //        YearsList.Add(i.ToString());
        //}
      //  ActiveItemYearName = YearsList[1].ToString();
       // YearsList.Add("00");
        YearsList.Add("");
        InitYearList(YearsList);
       
        YearssScrollPos.InitList(YearsList.Count);
    }

    public List<string> DateList { get; set; }

    public List<string> MonthList { get; set; }

    /// <summary>
    /// инициализируем кнопки в меню
    /// </summary>
    /// <param name="items"></param>
    public void InitDayList(List<string> items)
    {
        //Items = items;
        if (_buttonsListDay != null)
        {
            foreach (DropDownItem button in _buttonsListDay)
            {
                Destroy(button.gameObject);
            }
        }
        _buttonsListDay = new List<DropDownItem>();
        GameObject go;
        DropDownItem ddi;
        int i = 0;
        foreach (string s in items)
        {
            go = (GameObject)Instantiate(ItemLink);
            if (ContentDay!=null)
                go.transform.SetParent(ContentDay.gameObject.transform);
            go.gameObject.SetActive(true);
            ddi = go.GetComponent<DropDownItem>();
            ddi.Id = i;
            ddi.LabelText.text = s;
            _buttonsListDay.Add(ddi);
            i++;
        }
      
    }


  public void InitMonthList(List<string> items)
    {
        //Items = items;
        if (_buttonsListMonth != null)
        {
            foreach (DropDownItem button in _buttonsListMonth)
            {
                Destroy(button.gameObject);
            }
        }
        _buttonsListMonth = new List<DropDownItem>();
        GameObject go;
        DropDownItem ddi;
        int i = 0;
        foreach (string s in items)
        {
            go = (GameObject)Instantiate(ItemLink);
            if (ContentMonth!=null)
                go.transform.SetParent(ContentMonth.gameObject.transform);
            go.gameObject.SetActive(true);
            ddi = go.GetComponent<DropDownItem>();
            ddi.Id = i;
            ddi.LabelText.text = s;
            _buttonsListMonth.Add(ddi);
            i++;
        }
      
  }

    
    public void InitYearList(List<string> items)
    {
        //Items = items;
        if (_buttonsListYear != null)
        {
            foreach (DropDownItem button in _buttonsListYear)
            {
                Destroy(button.gameObject);
            }
        }
        _buttonsListYear = new List<DropDownItem>();
        GameObject go;
        DropDownItem ddi;
        int i = 0;
        foreach (string s in items)
        {
            go = (GameObject)Instantiate(ItemLink);
            if (ContentYear!=null)
             go.transform.SetParent(ContentYear.gameObject.transform);
            go.gameObject.SetActive(true);
            ddi = go.GetComponent<DropDownItem>();
            ddi.Id = i;
            ddi.LabelText.text = s;
            _buttonsListYear.Add(ddi);
            i++;
        }
     //   ActiveItemYearName = items[0];
    }



    virtual  public void SelectItem(ETip tip, int target)
    {
      //  print("!");
        string str = "";
        if (target < 10)
            str = "0" + target;
        else
            str = target.ToString();
        switch (tip)
        {
            case ETip.Date:
              
                ActiveItemDayName = str;
                break;
            case ETip.Month:
                ActiveItemMonthName = str;
                break;
            case ETip.Year:
                str = YearsList[target];
               // print("!!! "+str+":"+target);
                int ii;
                if (int.TryParse(str, out ii))
                {
                    //str = "19" + str;
                    if (ii > 60)
                        str = "19" + str;
                    else
                        str = "20" + str;
                }
                ActiveItemYearName = str;

                break;
        }
    }
   
    virtual public void OnOk()
    {
        TargetField.text = ActiveItemYearName+ Separator + ActiveItemMonthName + Separator + ActiveItemDayName;
        TargetField.gameObject.SendMessage("OnChanged",SendMessageOptions.DontRequireReceiver);
        gameObject.SetActive(false);
    }

    virtual public void OnCancel()
    {
        gameObject.SetActive(false);
    }

}
