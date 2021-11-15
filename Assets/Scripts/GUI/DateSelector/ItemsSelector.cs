using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class ItemsSelector : DateSelector
{
    /// <summary>
    /// ссылки на конпки
    /// </summary>
    private List<DropDownItem> _buttonsList;

    public int SelectedID = 1;



    public VerticalLayoutGroup Content;
    public string[] ItemsList;
    public List<string> DList { get; set; }

    public void InitList(string[] str)
    {
        ItemsList = str;
        DList = new List<string>();
        DList.Add("");
        for (int i = 0; i < ItemsList.Length; i++)
        {
            DList.Add(ItemsList[i]);
        }
        ActiveItem = ItemsList[0];
        DList.Add("");
        InitCountryList(DList);

    }




    /// <summary>
    /// инициализируем кнопки в меню
    /// </summary>
    /// <param name="items"></param>
    public new void InitCountryList(List<string> items)
    {
        //Items = items;
        if (_buttonsList != null)
        {
            foreach (DropDownItem button in _buttonsList)
            {
                Destroy(button.gameObject);
            }
        }
        _buttonsList = new List<DropDownItem>();
        GameObject go;
        DropDownItem ddi;
        int i = 0;
        foreach (string s in items)
        {
            go = (GameObject)Instantiate(ItemLink);
            go.transform.SetParent(Content.gameObject.transform);
            go.gameObject.SetActive(true);
            ddi = go.GetComponent<DropDownItem>();
            ddi.Id = i;
            ddi.LabelText.text = s;
            _buttonsList.Add(ddi);
            i++;
        }

    }




    public override void SelectItem(ETip tip, int target)
    {
        if (tip == ETip.Item)
        {
            print("!! !" + target + ":" + ItemsList.Count());
            ActiveItem = ItemsList[target - 1];
            SelectedID = target - 1;
        }
    }

    public override void OnOk()
    {
        TargetField.text = ActiveItem;
        gameObject.SetActive(false);
    }

    public string ActiveItem { get; set; }

    public override void OnCancel()
    {
        gameObject.SetActive(false);
    }

}
