using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FloatTweener : MonoBehaviour
{
	static FloatTweener instance;

	List<int> tweenIds = new List<int>();
	List<int> tweensToStop = new List<int>();

	void Start ()
	{
		 
	}
	 
	//проверяет есть ГО твинера на сцене, и если нет - создает
    static GameObject CheckInit()
    {
        GameObject go=null;
		if (instance == null)
		{
			go = new GameObject("Tweener");
			instance = go.AddComponent<FloatTweener>();
		}
        return go;
	}

	/// <summary>
	/// Tweens value from to with specified speed.
	/// </summary>
	/// <param name="from">From</param>
	/// <param name="to">To</param>
	/// <param name="speed">Speed</param>
	/// <param name="OnChange">Called on change</param>
	/// <param name="id">Tween identifier. Used for override same tweens with new values</param>
	public static GameObject TweenTo(float from, float to, float speed, Action<float> OnChange, int id = 0)
	{
        GameObject go=CheckInit();

		//если такой твин уже существует, добавляем в список на остановку, иначе просто регистрируем твин
		if (instance.tweenIds.Contains(id))
			if (!instance.tweensToStop.Contains(id))
				instance.tweensToStop.Add(id);
		else
			instance.tweenIds.Add(id);

		instance.StartCoroutine(instance.Tween(from, to, speed, OnChange, id));
	    return go;
	}

    public static void StopTween(int id)
    {
        print("StopTween");
      //убираем иды из списка на остановку и из списка твинов (поскольку твин завершен)
        instance.tweensToStop.Add(id);
    }
	//твинит значение переменной
	IEnumerator Tween(float from, float to, float speed, Action<float> OnChange, int id)
	{
		float curValue = from;

		//лерпим каждый фрейм пока значение больше трешхолда
		while (Mathf.Abs(curValue - to) > 0.01f)
		{
			curValue = Mathf.Lerp(curValue, to, Time.deltaTime * speed);
			OnChange(curValue);
			yield return new WaitForEndOfFrame();

			//если ид текущего твина есть в списке на останову - прерываем цикл
			if (tweensToStop.Contains(id))
				break;
		}

		OnChange(to);

		//убираем иды из списка на остановку и из списка твинов (поскольку твин завершен)
		tweensToStop.Remove(id);
		tweenIds.Remove(id);
	}
}
