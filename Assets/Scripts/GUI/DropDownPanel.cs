using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownPanel : MonoBehaviour
{
    /// <summary>
    /// список элементов
    /// </summary>
    public string[] Items;
    /// <summary>
    /// ссылки на конпки
    /// </summary>
    private List<DropDownItem> _buttonsList;
    /// <summary>
    /// ссылка на панель с кнопками
    /// </summary>
    public Image DropDownList;
    /// <summary>
    /// количество элементов в выпадающем меню для показа
    /// </summary>
    public int CountItemsInMenu = 5;
    /// <summary>
    /// ссылка на эталонный элемент списка
    /// </summary>
    public Button ItemLink;

    public Scrollbar Scroll;
    public int ActiveItemID = 0;
    public string ActiveItemName = "";
    public Text LabelText;
    public VerticalLayoutGroup Content;
    public Image MaskImage;
    public bool Opened=false;

    void Start()
    {
        //ShowList();

        if (Items.Length > 0)
        {
            //есть подготовленные елементы 
            InitList(Items);

        }

        HideList();
    }
    
    /// <summary>
    /// убрали мышку с меню
    /// </summary>
    public void OnExit()
    {
        HideList();
    }
    /// <summary>
    /// открываем или скрываем меню
    /// </summary>
    public void OnClick()
    {
        if (DropDownList.gameObject.activeInHierarchy)
            HideList();
        else
            ShowList();
    }
    private void HideList()
    {
        Opened = false;
        DropDownList.gameObject.SetActive(false);
    }
    private void ShowList()
    {
        Opened = true;
        DropDownList.gameObject.SetActive(true);
        Content.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    //void Update()
    //{
    //    if (Opened)
    //    {
    //        RaycastHit hitInfo = new RaycastHit();
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //        if (Physics.Raycast(ray, out hitInfo))
    //        {
    //            print("OnMouseEnter " + hitInfo.transform.name);
    //            //if (hover_state == HoverState.NONE)
    //            //{
    //            //    hitInfo.collider.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
    //            //    hoveredGO = hitInfo.collider.gameObject;
    //            //}
    //            //hover_state = HoverState.HOVER;
    //        }
    //        else
    //        {
    //           // print("OnMouseExit " + hitInfo.transform.name);
    //            //if (hover_state == HoverState.HOVER)
    //            //{
    //            //    hoveredGO.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
    //            //}
    //            //hover_state = HoverState.NONE;
    //        }

    //        //if (hover_state == HoverState.HOVER)
    //        //{
    //        //    hitInfo.collider.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver); //Mouse is hovering
    //        //    if (Input.GetMouseButtonDown(0))
    //        //    {
    //        //        hitInfo.collider.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver); //Mouse down
    //        //    }
    //        //    if (Input.GetMouseButtonUp(0))
    //        //    {
    //        //        hitInfo.collider.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver); //Mouse up
    //        //    }

    //        //}
    //    }
    //}
    /// <summary>
    /// инициализируем кнопки в меню
    /// </summary>
    /// <param name="items"></param>
    public void InitList(string[] items)
    {
        ShowList();
        Items = items;
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
            go = (GameObject)Instantiate(ItemLink.gameObject);
            go.transform.SetParent(Content.gameObject.transform);
            go.gameObject.SetActive(true);
            ddi = go.GetComponent<DropDownItem>();
            ddi.Id = i;
            ddi.LabelText.text = s;
            _buttonsList.Add(ddi);
            i++;
        }

        int col = 0;
        //Content.gameObject.transform.sizeDelta=
        if (_buttonsList.Count > CountItemsInMenu)
            col = CountItemsInMenu;
        else
            col = _buttonsList.Count;
        RectTransform rt = ItemLink.GetComponent<RectTransform>();
        MaskImage.rectTransform.sizeDelta = DropDownList.rectTransform.sizeDelta = new Vector2(DropDownList.rectTransform.sizeDelta.x, rt.rect.height *col + 5);
        if (Scroll != null)
        {
            Scroll.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10f, rt.rect.height*col + 5);
        }
        else
        {
           // print(this.name);
        }

        Content.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(DropDownList.rectTransform.sizeDelta.x, rt.rect.height * _buttonsList.Count + 5);
        SetActiveItem(0);
        HideList();
    }


    /// <summary>
    /// устанавливаем выделение
    /// </summary>
    /// <param name="id"></param>
    public void SetActiveItem(int id)
    {
        //print("SetActiveItem "+id);
        ActiveItemID = id;
        if (Items.Count() > id)
        {
            if (Items[id] != null)
            {
                LabelText.text = ActiveItemName = Items[id];
            }
        }
        HideList();
    }

    public void SetActiveItem(string str)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            // print(i);
            if (Items[i] == str)
            {
                LabelText.text = ActiveItemName = Items[i];
                ActiveItemID = i;
            }
        }

    }
}
