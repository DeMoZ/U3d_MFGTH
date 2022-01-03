using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public struct Ctx
    {
        
    }

    private Ctx _ctx;

    public void  SetCtx(Ctx ctx)
    {
        _ctx = ctx;
    }
}