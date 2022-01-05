using System;
using UniRx;
using UnityEngine;

public class PlayerEntity : IDisposable
{
    public struct Ctx
    {
        public ResourceLoader ResourceLoader;
        public IReactiveCommand<Swipe> OnSwipe;
    }

    private Ctx _ctx;

    public PlayerEntity(Ctx ctx)
    {
        _ctx = ctx;

        var playerView = _ctx.ResourceLoader.GetPlayer(null);
        playerView.transform.position = Vector3.one;

        var playerViewCtx = new PlayerView.Ctx
        {
        };
        playerView.SetCtx(playerViewCtx);

        var attackMap = playerView.AttackMap;
        var bodyParts = playerView.BodyParts;
        var attackScheme = playerView.AttackScheme;
        
        var playerFightCtx = new PlayerFightPm.Ctx
        {
            AttackScheme = attackScheme,
            BodyParts = bodyParts,
            AttackMap = attackMap,
            OnSwipe = _ctx.OnSwipe
        };
        var playerFightPm = new PlayerFightPm(playerFightCtx);
    }

    public void Dispose()
    {
    }
}