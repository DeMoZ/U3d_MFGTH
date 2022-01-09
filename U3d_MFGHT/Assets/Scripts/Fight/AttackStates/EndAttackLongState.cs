using System;

public class EndAttackLongState : AbstractAttackState
{
    public EndAttackLongState(Ctx ctx) : base(ctx)
    {
    }

    protected override void OnSwipe(Swipe swipe)
    {
        throw new NotImplementedException();
    }
}