using UnityEngine;
using System.Collections;

public class Fishka : MonoBehaviour
{
    public bool InMove = false;
    public  RectTransform RectTransform;
    private CanvasGroup _canvasG;
    public Vector3 TargetPosition { get; set; }

    void Awake()
    {
        _canvasG = GetComponent<CanvasGroup>();
        RectTransform = GetComponent<RectTransform>();
        
    }

    public void Start()
    {
        RectTransform.localScale = new Vector3(2f, 2f, 2f);
    }
    public void MoveTo(RectTransform pos, float time)
    {
       // print("[Fishka]MoveTo " + this.name + ":" + pos.gameObject.name + ":" + time);
        gameObject.SetActive(false);
        TargetPosition = pos.position;

        Invoke("Launch", time);

    }

    void Launch()
    {
        InMove = true;
        gameObject.SetActive(true);
    }
    void Update()
    {
        if (InMove)
        {
            //двигаем к цели
        //    print(RectTransform.position + ":" + TargetPosition + ":" + gameObject.activeSelf);
            //_canvasG.alpha -= 0.01f;
            RectTransform.position = Vector2.MoveTowards(RectTransform.position, TargetPosition, 500f * Time.deltaTime);
            if (Vector2.Distance(RectTransform.position, TargetPosition) < 1f)
            {
                Destroy(gameObject);
            }
        }
    }

}
