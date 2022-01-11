using UniRx;
using UnityEngine;
using System.Threading.Tasks;

public class RootEntity
{
    public struct Context
    {
        public Transform UIParent;
        public ResourceLoader ResourceLoader;
    }
    
    private Context _context;

    public RootEntity(Context context)
    {
        _context = context;

        ReactiveProperty<bool> isSwipeEnabled = new ReactiveProperty<bool>(false);
        ReactiveCommand<Swipe> onSwipe = new ReactiveCommand<Swipe>();
        ReactiveCommand<Swipe> onSwipeValidated = new ReactiveCommand<Swipe>();

        var swipeCatcher = _context.ResourceLoader.Get<SwipeCatcher>(_context.ResourceLoader.UIPrefabs.SwipeCatcher, _context.UIParent);

        SwipeCatcher.Context swipeCatcherCtx = new SwipeCatcher.Context
        {
            OnSwipe = onSwipe
        };
        
        swipeCatcher.SetContext(swipeCatcherCtx);

        PlayerSwipeInput psi = new PlayerSwipeInput(new PlayerSwipeInput.Context
        {
            isSwipeEnabled = isSwipeEnabled,
            OnSwipe = onSwipe,
            OnSwipeValidated = onSwipeValidated
        });

        var playerEntityCtx = new PlayerEntity.Ctx
        {
             ResourceLoader = _context.ResourceLoader,
             OnSwipe = onSwipeValidated
        };

        var playerEntity = new PlayerEntity(playerEntityCtx);

        ActivateInput(isSwipeEnabled);
    }
    
    public async void ActivateInput(ReactiveProperty<bool> isSwipeEnabled)
    {
        await Task.Delay(2000);
        Debug.Log("Enabling swipe");
        isSwipeEnabled.Value = true;
    }
}