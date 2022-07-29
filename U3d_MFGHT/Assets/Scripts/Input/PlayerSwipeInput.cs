using System;
using UniRx;
using UnityEngine;

public class PlayerSwipeInput : IDisposable
{
    public struct Context
    {
        public IReactiveCommand<Swipe> OnSwipe;
        // public IReadOnlyReactiveProperty<PlayerState> PlayerState;
        // public IReadOnlyReactiveProperty<SequenceState> PlayerState;
    }

    private Context _context;
    private CompositeDisposable _toDespose = new();

    public PlayerSwipeInput(Context context)
    {
        _context = context;

        _context.OnSwipe.Subscribe(OnSwipe).AddTo(_toDespose);
    }

    public void OnSwipe(Swipe swipe)
    {
        Debug.LogWarning($"swipe received {swipe.SwipeStates},{swipe.SwipeDirection}");
        
        // TODO: оповестить о сделаном свайпе, н должна обработаться логика
        //       - в данном состоянии возможно ли его вообще произвести
        
        // TODO: oпределить, если удар затяжной
        // TODO: oпределить, если удар в серии
        // TODO: oпределить, если удар в таймингах
        
        // hint: начало удара в таймингах серии, дальше можно удерживать, смена удара не возможна
        
        // 
    }

    public void Dispose()
    {
        _toDespose.Dispose();
    }
}