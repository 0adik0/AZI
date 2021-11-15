using UnityEngine;
using System.Collections;

public class CardInHand : MonoBehaviour {
    private Animator _animator;
    private CanvasGroup _canvasGroup;
    public int Id=0;
    //  private RectTransform _rect;
    private bool _isOpen = false;
   

    public void Take(float delay=0)
    {
       
        Invoke("Open",delay);
    }

    private void Open()
    {
        gameObject.SetActive(true);
        _animator.SetBool("IsOpen", true);
    }
    public void Drop()
    {
        _animator.SetBool("IsOpen", false);
        Invoke("HideCard", 0.5f);
    }

    public void HideCard()
    {
        gameObject.SetActive(false);
        CancelInvoke("Open");
    }
    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetActive(bool b)
    {
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = b;
    }
}
