namespace Fight.NewConfig
{
    public enum MovePart
    {
        DefaultPoint,
        RightHand,
        LeftHand,
        RightFoot,
        LeftFoot,
        HeadAim,
        RightHandTip,
        LeftHandTip,
        RightLegTip,
        LeftLegTip
    }

    public enum MoveType
    {
        Position,
        Rotation,
        PositionAndRotation
    }

    public enum PointType
    {
        Start,
        End,
        Hold,
        EndShort,
        EndLong
    }
}