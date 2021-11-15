using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    private Animator _animator;
    internal CanvasGroup _canvasGroup;
  //  private RectTransform _rect;
    private bool _isOpen=false;
    public bool IsOpen {
        get
        {
            if (_animator != null)
            {
                return _animator.GetBool("IsOpen");
            }
            else
            {
                return _isOpen;
            }
        }
        set
        {
            if (_animator != null)
            {
//                print("!"+value);
                _animator.SetBool("IsOpen",value);
               if (_canvasGroup != null) _canvasGroup.blocksRaycasts =  value;
            }
            else
            {
                _isOpen = value;
                if (_canvasGroup != null) _canvasGroup.blocksRaycasts = value;
            }
        }
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
        //_rect = GetComponent<RectTransform>();
      //  _rect.offsetMax = _rect.offsetMin = new Vector2(0, 0);
    }

}
