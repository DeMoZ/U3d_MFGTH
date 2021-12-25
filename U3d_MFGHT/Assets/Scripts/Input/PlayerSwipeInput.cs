using UniRx;
using UnityEngine;

public class PlayerSwipeInput
{
    public struct Context
    {
        public IReactiveCommand<Swipe> OnSwipe;
    }

    private Context _context;
    public PlayerSwipeInput(Context context)
    {
        _context = context;

        _context.OnSwipe.Subscribe(OnSwipe);
    }

    public void OnSwipe(Swipe swipe)
    {
        Debug.LogWarning($"swipe received {swipe.SwipeStates},{swipe.SwipeDirection}");
    }
}