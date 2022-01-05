using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public struct Ctx
    {
    }

    [SerializeField] private AttackScheme _attackScheme;
    [SerializeField] private BodyParts _bodyParts;
    [SerializeField] private AttackMapView _attackMap;

    private Ctx _ctx;

    public AttackScheme AttackScheme => _attackScheme;
    public BodyParts BodyParts => _bodyParts;
    public AttackMapView AttackMap => _attackMap;


    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
    }
}