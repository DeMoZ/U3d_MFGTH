using System;

public class EndAttackShortState : AbstractAttackState
{
    public EndAttackShortState(Ctx ctx) : base(ctx)
    {
    }

    protected override void OnSwipe(Swipe swipe)
    {
        throw new NotImplementedException();
    }
}