using System;
using UniRx;
using UnityEngine;

public class PlayerFightPm : IDisposable
{
    public struct Ctx
    {
        public IReactiveCommand<Swipe> OnSwipe;
    }

    private Ctx _ctx;
    private CompositeDisposable _toDispose;

    public PlayerFightPm(Ctx ctx)
    {
        _ctx = ctx;
        _toDispose = new CompositeDisposable();
        
        _ctx.OnSwipe.Subscribe(OnSwipe).AddTo(_toDispose);
    }

    private void OnSwipe(Swipe swipe)
    {
        Debug.LogWarning($"PlayerFightPm Received on swipe  {swipe.SwipeStates},{swipe.SwipeDirection}");
    }

    public void Dispose()
    {
        _toDispose.Dispose();
    }
}