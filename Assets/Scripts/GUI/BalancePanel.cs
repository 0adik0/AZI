using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class BalancePanel : Menu
{

    public Text BalanceText;
    public static BalancePanel Instance;

    void Start()
    {
        Instance = this;
    }
    public void Show()
    {
        if (GameController.Id>0)
            IsOpen = true;
        UpdateBalance();
    }
    public void Hide()
    {
        IsOpen = false;
    }
    /// <summary>
    /// обновляем баланс
    /// </summary>
    public static void UpdateBalance()
    {
        if (Instance!=null)
        Instance.BalanceText.text = GameController.Money + "$";
    }
   
}
