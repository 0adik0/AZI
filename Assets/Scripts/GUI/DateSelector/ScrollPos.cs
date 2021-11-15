using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// класс отвечает за позиционирование скролла при прокручивании на определенных позициях
/// </summary>
public class ScrollPos : UIBehaviour, IEndDragHandler, IBeginDragHandler
{
    public ScrollRect scrollRect; // the scroll rect to scroll
    public SnapDirection direction; // the direction we are scrolling
    public int itemCount; // how many items we have in our scroll rect
    public int ItemsVisible = 3;//how many items showed
    public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f); // a curve for transitioning in order to give it a little bit of extra polish
    public float speed; // the speed in which we snap ( normalized position per second? )
    public float MinVelocity=1000f;
    public DateSelector DT;
    private bool _endMove = false;
    public ETip Tip;

    public GameObject Content;

  
    public void InitList(int count)
    {
        itemCount = count;
    }
    //protected  void Reset()
    //{
    //    base.Reset();

    //    if (scrollRect == null) // if we are resetting or attaching our script, try and find a scroll rect for convenience 
    //        scrollRect = GetComponent<ScrollRect>();
    //}

    public void OnBeginDrag(PointerEventData eventData)
    {
    //    StopCoroutine(SnapRect()); // if we are snapping, stop for the next input
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _endMove = false;
        StartCoroutine(SnapRect()); // simply start our coroutine ( better than using update )
    }
    /// <summary>
    /// устанавливаем активный элемент
    /// </summary>
    /// <param name="id"></param>
    public void SetActiveItem(int id)
    {
        
    }
    private IEnumerator SnapRect()
    {
        while (!_endMove)
        {
            if (scrollRect == null)
                throw new System.Exception("Scroll Rect can not be null");
            if (itemCount == 0)
                throw new System.Exception("Item count can not be zero");
            if ((Math.Abs(scrollRect.velocity.y) < MinVelocity && direction == SnapDirection.Vertical) || (Math.Abs(scrollRect.velocity.x) < MinVelocity && direction == SnapDirection.Horizontal))
            {
                float startNormal = direction == SnapDirection.Horizontal
                    ? scrollRect.horizontalNormalizedPosition
                    : scrollRect.verticalNormalizedPosition; // find our start position
                float delta = 1f/(float) (itemCount - ItemsVisible); // percentage each item takes

                int target = Mathf.RoundToInt(startNormal/delta);
                // this finds us the closest target based on our starting point
                float endNormal = delta*target; // this finds the normalized value of our target
                float duration = Mathf.Abs((endNormal - startNormal)/speed);
                // this calculates the time it takes based on our speed to get to our target
                //print(delta + ":" + endNormal + ":" + duration+":"+speed);
                int u = itemCount - target - 2;
                if (u < 1) u = 1;
                if (u > (itemCount - 2)) u = itemCount - 2;
                
                DT.SelectItem(Tip,u);
                float timer = 0f; // timer value of course
                while (timer < 1f) // loop until we are done
                {
                    timer = Mathf.Min(1f, timer + Time.deltaTime/duration); // calculate our timer based on our speed
                    float value = Mathf.Lerp(startNormal, endNormal, curve.Evaluate(timer));
                    // our value based on our animation curve, cause linear is lame

                    if (direction == SnapDirection.Horizontal)
                        // depending on direction we set our horizontal or vertical position
                        scrollRect.horizontalNormalizedPosition = value;
                    else
                        scrollRect.verticalNormalizedPosition = value;

                    yield return new WaitForEndOfFrame(); // wait until next frame
                }
                _endMove = true;
            }
            else
            {
                yield return new WaitForEndOfFrame(); // wait until next frame
            }
        }
    }
}

// The direction we are snapping in
public enum SnapDirection
{
    Horizontal,
    Vertical,
}
public enum ETip
{
    Date,
    Month,
    Year,
    Country,
    Item
}