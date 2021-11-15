using System;

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottomMenu : MonoBehaviour
{

    public Button PlayButton;
    public Button NewsButton;
    public Button RulesButton;
    public RectTransform MainScreenPanel;
    public RectTransform GamePanel;
    public Text MoneyText;
    public Text BalanceText;
    public Text BottomBalanceText;
    public Slider BetSlider;
    public Button UravButton;
    public Button PassButton;
    public Button IncreaseButton;
    public Button ExitButton;
    public static BottomMenu Instance;
    public GameObject IncreasePanel;
   // public Button LoginBtn;
    public Text GameBalanceText;
    public CanvasGroup CanvasG;
    public static bool HideRaise = false;
    public static bool HideCall = false;

    public void ShowBetSlider()
    {
        IncreasePanel.gameObject.SetActive(true);
        if (TableController.Instance.MaxBet > GameController.Money)
        {
            InitSlider(TableController.Instance.MinBet, TableController.Instance.MinBet,
                (Single)GameController.Money);
        }
        else
        {
            InitSlider(TableController.Instance.MinBet, TableController.Instance.MinBet,
               TableController.Instance.MaxBet);
        }
    }
    public void HideBetSlider()
    {
        IncreasePanel.gameObject.SetActive(false);
    }
    public void Awake()
    {
        CanvasG = GetComponent<CanvasGroup>();
        Instance = this;
    }

    public static void ShowLoginBtn(string str)
    {
        print("ShowLoginBtn");
      //  Instance.LoginBtn.gameObject.SetActive(true);
        //Instance.LoginText.text = str;
    }
    public void OnExit()
    {
        print("OnExit");
        TableController.Instance.ShowExitPanel();
    }
    public static void LockInteract()
    {
       // Instance.BetSlider.gameObject.SetActive(false);
        Instance.IncreasePanel.gameObject.SetActive(false);
        Instance.IncreaseButton.gameObject.SetActive(false);
        Instance.PassButton.gameObject.SetActive(false);
        Instance.UravButton.gameObject.SetActive(false);
        Instance.CanvasG.interactable = false;
    }
    public static void UnLockInteract()
    {
        print("TableController.CurrentRound " + TableController.CurrentRound);
       // Instance.GameBalanceText.text = GameController.Money.ToString()+"$";
        if (TableController.CurrentRound == 1)
        {
            TableController.Instance.StartAuction();
            Instance.CanvasG.interactable = true;
            Instance.BetSlider.gameObject.SetActive(true);
            if (!HideRaise)
                Instance.IncreaseButton.gameObject.SetActive(true);
            else
                Instance.IncreaseButton.gameObject.SetActive(false);
            if (!HideCall)
                Instance.UravButton.gameObject.SetActive(true);
            else
                Instance.UravButton.gameObject.SetActive(false);
            Instance.PassButton.gameObject.SetActive(true);

        }
    }

   

    private void UnlockPass()
    {
        print("UnlockPass");
        Instance.PassButton.gameObject.SetActive(true);
    }
    public void ShowMainScreen()
    {
        MainScreenPanel.gameObject.SetActive(true);
        GamePanel.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        BottomBalanceText.gameObject.SetActive(false);
        /*PlayButton.gameObject.SetActive(true);
        NewsButton.gameObject.SetActive(true);
        RulesButton.gameObject.SetActive(true);*/
    }

    public void HideMainScreen()
    {
        MainScreenPanel.gameObject.SetActive(false);
    }
    /// <summary>
    /// Показываем игровую панель
    /// </summary>
    public void ShowGame()
    {
        BottomBalanceText.gameObject.SetActive(true);
        MainScreenPanel.gameObject.SetActive(false);
        GamePanel.gameObject.SetActive(true);
        ExitButton.gameObject.SetActive(true);
        HideBetSlider();
        if (GameController.Money < TableController.Instance.MinBet)
        {
            //у игрока не хватает денег на минимальную ставку
            UravButton.gameObject.SetActive(false);
            IncreaseButton.gameObject.SetActive(false);
         //   BetSlider.gameObject.SetActive(false);
        }
       
        /*PlayButton.gameObject.SetActive(false);
        NewsButton.gameObject.SetActive(false);
        RulesButton.gameObject.SetActive(false);*/
    }

    public void HideGame()
    {
        GamePanel.gameObject.SetActive(false);
        Instance.CanvasG.interactable = true;
    }
    public void InitSlider(Single cur, Single min, Single max)
    {
        print("InitSlider " + cur + ":" + min + ":" + max);
        BetSlider.minValue = min;
        BetSlider.maxValue = max;
        BetSlider.value = cur;
        MoneyText.text = cur.ToString();
    }
    public void OnBetChange(Single s)
    {
        MoneyText.text = Math.Round(s, 1).ToString() + "$";
    }

    public static void EndGame()
    {
        Instance.HideGame();
    }

    public static void UnlockPassWithDelay()
    {
        print("UnlockPassWithDelay");
        Instance.PassButton.gameObject.SetActive(false);
        Instance.Invoke("UnlockPass", 3f);
    }

    
}
