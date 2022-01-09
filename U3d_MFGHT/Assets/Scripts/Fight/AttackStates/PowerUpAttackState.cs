using System;

public class PowerUpAttackState : AbstractAttackState
{
    public PowerUpAttackState(Ctx ctx) : base(ctx)
    {
    }

    protected override void OnSwipe(Swipe swipe)
    {
        throw new NotImplementedException();
    }
}