using UnityEngine;
using System.Collections;

public class HistoryDate : MonoBehaviour
{

    public HistoryPanel HP;
    public int FieldNom = 1;
    public void OnChanged()
    {
        print("OnChanged");
        if (FieldNom==1)
            HP.OnDate1Changed();
        else
        {
            HP.OnDate2Changed();
        }
    }
}
