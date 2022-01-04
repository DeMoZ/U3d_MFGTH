using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public struct Ctx
    {
    }

    [SerializeField] private AttackMapView _attackMap;

    private Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
    }
}