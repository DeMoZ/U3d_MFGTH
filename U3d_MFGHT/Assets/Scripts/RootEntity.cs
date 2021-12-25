using UniRx;
using UnityEngine;

public class RootEntity
{
    public struct Context
    {
        public SwipeCatcher SwipeCatcher;
    }

    private Context _context;
    
    public RootEntity(Context context)
    {
        _context = context;

        IReactiveCommand<Swipe> onSwipe = new ReactiveCommand<Swipe>();

        SwipeCatcher.Context swipeCatcherCtx = new SwipeCatcher.Context
        {
            OnSwipe = onSwipe
        };
        
        _context.SwipeCatcher.SetContext(swipeCatcherCtx);

        PlayerSwipeInput psi = new PlayerSwipeInput(new PlayerSwipeInput.Context
        {
            OnSwipe = onSwipe
        });
    }
}

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