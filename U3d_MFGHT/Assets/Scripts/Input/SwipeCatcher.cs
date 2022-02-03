using System;
using System.Collections;
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

    private Coroutine _thrustDelayRoutine;
    private WaitForSeconds _delaySeconds = new WaitForSeconds(0.05f);

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
            SwipeState = SwipeStates.None,
            SwipeDirection = SwipeDirections.None
        });

        _touchPos = eventData.position;
        //Debug.Log("OnPointerDown");

        if (_thrustDelayRoutine != null)
            StopCoroutine(_thrustDelayRoutine);

        _thrustDelayRoutine = StartCoroutine(IEDelayThrustRoutine(eventData));
    }

    private IEnumerator IEDelayThrustRoutine(PointerEventData eventData)
    {
        yield return _delaySeconds;

        _swipeDirection = SwipeDirections.Thrust;

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeState = SwipeStates.Start,
            SwipeDirection = _swipeDirection
        });

        //_thrustDelayRoutine = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_thrustDelayRoutine != null)
        {
            StopCoroutine(_thrustDelayRoutine);
            _thrustDelayRoutine = null;
        }

        var nextPos = eventData.position;

        if (TryCalculateSwipeDirection(nextPos, out var direction))
            _swipeDirection = direction;
        else
            return;

        //Debug.Log("OnBeginDrag");
        //DebugShowArrowInInspector();
        _touchPos = eventData.position;

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeState = _thrustDelayRoutine == null ? SwipeStates.Start : SwipeStates.Change,
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
        //DebugShowArrowInInspector();

        _touchPos = eventData.position;

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeState = SwipeStates.Change,
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
            SwipeState = SwipeStates.End,
            SwipeDirection = SwipeDirections.Thrust
        });

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeState = SwipeStates.None,
            SwipeDirection = SwipeDirections.None
        });
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeState = SwipeStates.End,
            SwipeDirection = _swipeDirection
        });

        _ctx.OnSwipe.Execute(new Swipe
        {
            SwipeState = SwipeStates.None,
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
        else if (45.0f <= angle && angle < 135.0f) direction = sign ? SwipeDirections.ToLeft : SwipeDirections.ToRight;
        else if (135.0f <= angle && angle <= 180.0f) direction = SwipeDirections.ToDown;

        return true;
    }

    private string _debugShowValue = "o";

    private void DebugShowArrowInInspector()
    {
        _debugShowValue = _swipeDirection switch
        {
            SwipeDirections.None => "o",
            SwipeDirections.ToRight => ">",
            SwipeDirections.ToLeft => "<",
            SwipeDirections.ToUp => "^",
            SwipeDirections.ToDown => "v",
            SwipeDirections.Thrust => "x",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}