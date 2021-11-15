using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginPanel : Menu
{
    public InputField LoginField;
    public InputField PassField;
    public Text ErrorText;
    
    
    public void LoadPrefs()
    {
        if (PlayerPrefs.GetString("Login") != "")
            LoginField.text = PlayerPrefs.GetString("Login");
        //print(PlayerPrefs.GetString("Login"));
    }
    public void OnLogin()
    {
        if (LoginField.text.Length == 0 || PassField.text.Length == 0)
        {
            ErrorText.text = GameController.LangManager.GetTextValue("Login.NeedEnterError");
        }
        else
        {
            ErrorText.text = "";
            PlayerPrefs.SetString("Login", LoginField.text);
            GameController.Pass = PassField.text;

            Net.Auth(LoginField.text, PassField.text);

            PassField.text = "";
            if (_canvasGroup != null)
                _canvasGroup.interactable = false;
        }
    }

    void Update()
    {
        if (PassField.text != "" && LoginField.text != "" && Input.GetKeyUp(KeyCode.Return) && GUIController.Instance.CurrentMenu==GUIController.Instance.LoginPanel)
        {
            OnLogin();
        }
    }
    public void Start()
    {
        LoadPrefs();
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void WrongLogin()
    {
        if (_canvasGroup != null)
            _canvasGroup.interactable = true;
        ErrorText.text = GameController.LangManager.GetTextValue("Login.Error");
    }
}
