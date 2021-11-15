using UnityEngine;
using System.Collections;

public class ExitPanel : Menu
{

   
    public void Show()
    {
        IsOpen = true;
    }
    public void OnStay()
    {
        //gameObject.SetActive(false);
        IsOpen = false;
       // GUIController.Instance.OnBackBtn();
    }

    public void OnExit()
    {
        //gameObject.SetActive(false);
        //_menu.IsOpen = false;
       // TableController.Instance.EndGame();
      //  Net.SendExit();
        Application.Quit();
    }
    public void OnLobbi()
    {
        //gameObject.SetActive(false);
        
        // 
      //  Net.SendExit();
        TableController.Instance.EndGame();
        IsOpen = false;
    }
}
