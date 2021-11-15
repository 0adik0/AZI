using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class FishkiStopka : MonoBehaviour
{
    private List<GameObject> _fishki2Show;
    public List<Fishka> Fishki;
    public RectTransform RectTransform;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        Fishki = new List<Fishka>();
        _fishki2Show = new List<GameObject>();
    }
  
    /// <summary>
    /// добавляем фишки
    /// </summary>
    /// <param name="fishka"></param>
    /// <param name="col"></param>
    public void Add(Fishka fishka, int col,float time=0f)
    {
        RectTransform.localScale = new Vector3(1f, 1f);
        GameObject go;
        for (int i = 0; i < col; i++)
        {
         //   print("fishka.gameObject " + fishka.gameObject);
            go = (GameObject)Instantiate(fishka.gameObject);
            _fishki2Show.Add(go);
            Fishki.Add(go.GetComponent<Fishka>());
            go.GetComponentInChildren<RectTransform>().SetParent(gameObject.transform);
            go.GetComponentInChildren<RectTransform>().localPosition = new Vector2(Random.Range(-5f, 5f), Fishki.Count * 15f + Random.Range(-5f, 5f));
            go.gameObject.SetActive(false);
            
        }
        InvokeRepeating("ShowFishki", time, 0.2f);
    }

    void ShowFishki()
    {
        RectTransform.localScale = new Vector3(1f, 1f);
        GameObject go;
        if (_fishki2Show.Count > 0)
        {
            go=_fishki2Show.First();
            _fishki2Show.Remove(go);
            if (go!=null)
                go.SetActive(true);
        }
        else
        {
            CancelInvoke("ShowFishki");
        }
    }

    public void RemoveFishki(float delay)
    {

     //   print("RemoveFishki "+this.name);
        int i2 = 0;
        for (int i = Fishki.Count-1;  i>=0; i--)
        {
       //     print("! " + Fishki[i] + ":" + i2 * 0.2f + ":" + i);
            if (Fishki[i]!=null)
                Destroy(Fishki[i].gameObject, i2*0.2f+delay);
            i2++;
        }
        //InvokeRepeating("Rem", 0.2f, 0.2f);
    }

    void Rem()
    {
       // print("Rem " + Fishki.Count);
        GameObject go;
        if (Fishki.Count > 0)
        {
            Fishka f = Fishki.First();
            Fishki.Remove(f);
            Destroy(f);
            //Debug.Break();
        }
        else
        {
         //   print("end del");
            CancelInvoke("Rem");
            Destroy(gameObject);
        }
    }
}
