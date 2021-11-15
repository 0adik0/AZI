using Ucss;
using UnityEngine;
using System.Collections;

public class RestartPanel : Menu {

    public void OnExit()
    {
        print("OnExit");
        WWWForm data = new WWWForm();
        data.AddField("event", "PLAYER_AWAY");
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + GameController.Token, data, new EventHandlerHTTPString(OnLockInteract),
            new EventHandlerServiceError(Net.Instance.OnHTTPError),
            new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
        //GUIController.Instance.ShowLobbiPanel();
        TableController.Instance.EndGame();
        IsOpen = false;
    }

    private void OnLockInteract(string text, string transactionid)
    {
        print(text);
    }
}
