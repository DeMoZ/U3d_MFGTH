using System;
using UniRx;
using UnityEngine;

public class PlayerSwipeInput : IDisposable
{
    public struct Context
    {
        public ReactiveProperty<bool> isSwipeEnabled;
        public IReactiveCommand<Swipe> OnSwipe;
        public IReactiveCommand<Swipe> OnSwipeValidated;
        // public IReadOnlyReactiveProperty<PlayerState> PlayerState;
        // public IReadOnlyReactiveProperty<SequenceState> PlayerState;
    }

    private Context _context;
    private CompositeDisposable _toDispose;

    public PlayerSwipeInput(Context context)
    {
        _context = context;
        _toDispose = new CompositeDisposable();

        _context.OnSwipe.Subscribe(OnSwipe).AddTo(_toDispose);
    }

    public void OnSwipe(Swipe swipe)
    {
        //Debug.Log($"<color=magenta>{this} received swipe and validate; state = {swipe.SwipeState}, direction = {swipe.SwipeDirection}</color>");

        if (_context.isSwipeEnabled.Value)  
            _context.OnSwipeValidated?.Execute(swipe);
    }

    public void Dispose()
    {
        _toDispose.Dispose();
    }
}