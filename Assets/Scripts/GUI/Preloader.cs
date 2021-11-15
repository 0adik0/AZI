using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Preloader : MonoBehaviour
{
    public Image Bar;
    private AsyncOperation Loader;
    void Start()
    {
//        print("Start");

        StartCoroutine(LoadCoroutine());
        
    }

    private IEnumerator LoadCoroutine()
    {
       // Debug.Log("Loading starts");
        Loader = Application.LoadLevelAsync("MainMobile");
        Loader.allowSceneActivation = false;
        Bar.fillAmount = 0;
        while (!Loader.isDone)
        {
            
           // Debug.Log("Loading Progress : " + Loader.progress);
            Bar.fillAmount = Loader.progress;
            if (Loader.progress >= 0.9f)
            {
                Bar.fillAmount = 1f;
                //Debug.Log("Loading done - ACTIVATE SCENE");
                yield return new WaitForSeconds(0.2f);
                Loader.allowSceneActivation = true;
            }
            yield return 0;
        }
    }
}
