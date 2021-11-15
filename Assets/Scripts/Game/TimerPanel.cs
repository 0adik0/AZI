using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class TimerPanel : MonoBehaviour
{
    public GameObject Fon;
    public bool StopTimer = false;
    public Text TimeText;
    public int TimeLeft = 0;
    public Image Anim;
    public int TimeDelayed = 0;
    /// <summary>
    /// показываем тайм с обратным отсчетом
    /// </summary>
    /// <param name="time"></param>

    public void ShowTime(int time)
    {
        print("ShowTime " + time + ":");
        gameObject.SetActive(true);
        if (Fon) Fon.SetActive(true);
        if (time > 0)
        {
            if (Anim != null)
            {
                TimeLeft = 20;
                TimeText.text = TimeLeft.ToString();
            }
            else
            {
                if (TimeLeft == 0)
                {
                    TimeLeft = 20;
                    TimeText.text = TimeLeft.ToString();
                }
            }

            CancelInvoke("Tic");
            InvokeRepeating("Tic", 1f, 1f);
        }
        else
        {
            CancelInvoke("Tic");
            TimeText.text = "";
        }

        UpdateFon();
    }

    public void ShowTimeWithDelay(int time, int delay)
    {
        TimeDelayed = time;
        Invoke("ShowTimeDelayed", delay);
    }

    private void ShowTimeDelayed()
    {
        int time = TimeDelayed;
        print("ShowTime " + time + ":");
        gameObject.SetActive(true);
        if (Fon) Fon.SetActive(true);
        if (time > 0)
        {
            if (Anim != null)
            {

                TimeLeft = time;
                TimeText.text = TimeLeft.ToString();
            }
            else
            {
                if (TimeLeft == 0)
                {
                    TimeLeft = time;
                    TimeText.text = TimeLeft.ToString();
                }
            }

            CancelInvoke("Tic");
            InvokeRepeating("Tic", 1f, 1f);
            UpdateFon();
        }
        else
        {
            CancelInvoke("Tic");
            TimeText.text = "";
        }

    }

    private void UpdateFon()
    {
        if (Anim != null)
            Anim.fillAmount = TimeLeft / (float)TableController.Instance.MaxTurnTime;
    }

    public void Hide()
    {
        CancelInvoke("Tic");
        print("hide timer");
        TimeLeft = 0;
        TimeText.text = "";
        if (Fon) Fon.SetActive(false);
        gameObject.SetActive(false);
    }
    public void Tic()
    {
        if (StopTimer)
        {
            if (Fon) Fon.SetActive(false);
            CancelInvoke("Tic");
            gameObject.SetActive(false);
            return;
        }
        TimeLeft--;
        UpdateFon();
        if (TimeLeft > 0)
        {
            TimeText.text = TimeLeft.ToString();
        }
        else
        {
            print("end time timer");
            CancelInvoke("Tic");
            if (Fon) Fon.SetActive(false);
            gameObject.SetActive(false);
        }

    }
}
