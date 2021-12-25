using System;
using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;

public class SwipeCatcher : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerClickHandler /*, IPointerUpHandler*/
{
    [SerializeField] private float _minSwipeLength = 0.1f;

    public struct Context
    {
        public IReactiveCommand<Swipe> OnSwipe;
    }

    private Context _ctx;
    private Vector2 _touchPos;
    private SwipeDirections _swipeDirection;

    public void SetContext(Context ctx)
    {
        _ctx = ctx;
    }

    /// <summary>
    /// Test Awake
    /// </summary>
    //public void Awake() => _ctx = new Context { OnSwipe = new ReactiveCommand<Swipe>() };
    public void OnPointerDown(PointerEventData eventData)
    {
        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeStates = SwipeStates.None,
            SwipeDirection = SwipeDirections.None
        });

        //Debug.Log("OnPointerDown");

        _touchPos = eventData.position;
        _swipeDirection = SwipeDirections.Thrust;

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeStates = SwipeStates.Start,
            SwipeDirection = _swipeDirection
        });
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var nextPos = eventData.position;

        if (TryCalculateSwipeDirection(nextPos, out var direction))
            _swipeDirection = direction;
        else
            return;

        //Debug.Log("OnBeginDrag");
        _touchPos = eventData.position;
        DebugShowArrowInInspector();

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeStates = SwipeStates.Change,
            SwipeDirection = _swipeDirection
        });
    }

    public void OnDrag(PointerEventData eventData)
    {
        var nextPos = eventData.position;

        if (TryCalculateSwipeDirection(nextPos, out var direction) && direction != _swipeDirection)
            _swipeDirection = direction;
        else
            return;

        //Debug.Log("OnDrag - direction changed");
        DebugShowArrowInInspector();

        _touchPos = eventData.position;

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeStates = SwipeStates.Change,
            SwipeDirection = _swipeDirection
        });
    }

// The order is OnPointerUP->OnPointerClick->OnEndDrag. 
// So I dont need OnPointerUp in calculations
    /*public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");

        _ctx.OnInputDirection.Execute(new Swipe
        {
            SwipeStates = SwipeStates.None,
            SwipeDirection = SwipeDirections.None
        });    
    }*/

    public void OnPointerClick(PointerEventData eventData)
    {
        var nextPos = eventData.position;

        if (Vector2.Distance(nextPos, _touchPos) >= _minSwipeLength)
            return;

        //Debug.Log("OnPointerClick");

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeStates = SwipeStates.End,
            SwipeDirection = SwipeDirections.Thrust
        });

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeStates = SwipeStates.None,
            SwipeDirection = SwipeDirections.None
        });
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeStates = SwipeStates.End,
            SwipeDirection = _swipeDirection
        });

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeStates = SwipeStates.None,
            SwipeDirection = SwipeDirections.None
        });
    }

    private bool TryCalculateSwipeDirection(Vector2 nextPos, out SwipeDirections direction)
    {
        direction = SwipeDirections.None;

        if (Vector2.Distance(nextPos, _touchPos) < _minSwipeLength)
            return false;

        var angle = Vector2.SignedAngle((nextPos - _touchPos).normalized, Vector2.up);
        var sign = false;

        if (angle < 0)
        {
            angle = Mathf.Abs(angle);
            sign = true;
        }

        if (0f <= angle && angle < 45.0f) direction = SwipeDirections.ToUp;
        else if (45.0f <= angle && angle < 135.0f) direction = sign ? SwipeDirections.ToLeft: SwipeDirections.ToRight;
        else if (135.0f <= angle && angle <= 180.0f) direction = SwipeDirections.ToDown;
        
        return true;
    }

    [SerializeField] private string _graph = "o";
    private void DebugShowArrowInInspector()
    {
        _graph = _swipeDirection switch
        {
            SwipeDirections.None => "o",
            SwipeDirections.ToRight => ">",
            SwipeDirections.ToLeft => "<",
            SwipeDirections.ToUp => "^",
            SwipeDirections.ToDown => "v",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}