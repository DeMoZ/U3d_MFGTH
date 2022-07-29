using UniRx;
using UnityEngine;

public class RootEntity
{
    public struct Context
    {
        /*public Transform UIParent;
        public ResourceLoader ResourceLoader;*/
    }

    private Context _context;
    
    public RootEntity(Context context)
    {
        _context = context;

        SwipeCatcher swipeCatcher = default;//_context.ResourceLoader.Get<SwipeCatcher>(_context.ResourceLoader.UIPrefabs.SwipeCatcher, _context.UIParent); 
        
        
        IReactiveCommand<Swipe> onSwipe = new ReactiveCommand<Swipe>();

        SwipeCatcher.Context swipeCatcherCtx = new SwipeCatcher.Context
        {
            OnSwipe = onSwipe
        };
        
        swipeCatcher.SetContext(swipeCatcherCtx);

        PlayerSwipeInput psi = new PlayerSwipeInput(new PlayerSwipeInput.Context
        {
            OnSwipe = onSwipe
        });
    }
}