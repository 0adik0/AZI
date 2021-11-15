using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
/// <summary>
/// место для стопок фишек
/// </summary>
public class FishkiPos : MonoBehaviour
{
    //public Dictionary<int,List<Fishka>> Stopki = new Dictionary<int, List<Fishka>>();
    public List<FishkiStopka> Stopki;
    public RectTransform RectTransform;
    public GameObject StartFishka;
    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void Clear()
    {
        foreach (FishkiStopka stopka in Stopki)
        {
            Destroy(stopka.gameObject);
        }
        Stopki = new List<FishkiStopka>();
    }

    public void AddStartFishka()
    {
        foreach (FishkiStopka stopka in Stopki)
        {
            Destroy(stopka.gameObject);
        }
        Stopki = new List<FishkiStopka>();
        if (StartFishka != null)
            Destroy(StartFishka);
        if (StartFishka != null)
            Destroy(StartFishka);
        StartFishka = (GameObject)Instantiate(Resources.Load("GUI/Fishka0"));
        StartFishka.GetComponentInChildren<RectTransform>().SetParent(gameObject.transform);
        StartFishka.GetComponentInChildren<RectTransform>().localPosition = new Vector2(0, 0);

    }
    public void AddStopka(Fishka fishka, int col, float time = 0f)
    {

        GameObject go = (GameObject)Instantiate(Resources.Load("GUI/FishkiStopka"));
        FishkiStopka fs = go.GetComponent<FishkiStopka>();
        //    print(fs);
        fs.RectTransform.SetParent(gameObject.transform);
        float d = Stopki.Count / 3f;
        //print("! " + d + ":" + Stopki.Count);
        fs.RectTransform.localPosition = new Vector3( (float)Math.Floor(d) * -30f,0);
        fs.Add(fishka, col, time);
        Stopki.Add(fs);
    }

    public void AddFishka(Fishka fishka, float time = 0f)
    {
        print("AddFishka");
        FishkiStopka fs = Stopki.First();
        fs.RectTransform.SetParent(gameObject.transform);
        float d = Stopki.Count / 3f;
        //print("! " + d + ":" + Stopki.Count);
        fs.RectTransform.localPosition = new Vector3((float)Math.Floor(d) * -30f, (Stopki.Count - (float)Math.Floor(d) * 3) * 30f + d * 15f);
        fs.Add(fishka, 1,time);
    }

    public void RemoveStartFishka()
    {
        if (StartFishka != null)
            Destroy(StartFishka);
    }
}
