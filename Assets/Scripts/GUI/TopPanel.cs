using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    public Slider LangButton;
    public Button HelpButton;
    public Button CloseButton;
    public Button LogoutButton;
    public Text LoginText;
    public Button LoginButton;
    public Text VersionText;
    public static TopPanel Instance;

    void Start()
    {
        Instance = this;
        if (VersionText != null)
            VersionText.text = Application.version;
    }
    /// <summary>
    /// показываем главное меню
    /// </summary>
    public void ShowMainMenu()
    {

        HelpButton.gameObject.SetActive(true);
        
        CloseButton.gameObject.SetActive(false);
        if (GameController.Id > 0)
        {
            LogoutButton.gameObject.SetActive(true);
        }
        else
        {
            LangButton.gameObject.SetActive(true);
        }
        if (GameController.Login != "")
        {
            LoginButton.gameObject.SetActive(true);
            LoginText.text = GameController.Login;
        }
    }
    /// <summary>
    /// показываем вид меню
    /// </summary>
    public void HideMainMenu()
    {
        LoginButton.gameObject.SetActive(false);
        LangButton.gameObject.SetActive(false);
        HelpButton.gameObject.SetActive(false);
        CloseButton.gameObject.SetActive(true);
        LogoutButton.gameObject.SetActive(false);
    }

   

    public void HideLobbi()
    {
        print("Не ОБработан HideMainScreen");
    }


}
