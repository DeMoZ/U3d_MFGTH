using UnityEngine;

[CreateAssetMenu]
public class AttackConfigOverride : AbstractAttackConfig
{
    public Vector3 SetDefaultLocalPosition;
    public Vector3 SetFromLocalPosition;
    public Vector3 SetToLocalPosition;
    public float SetBlendTime = 0.5f;
    public float SetAmplitude = 0.4f;
    public AnimationCurve SetSpeedCurve;

    public override Vector3 GetDefaultLocalPosition() => SetDefaultLocalPosition;
    public override Vector3 GetFromLocalPosition() => SetFromLocalPosition;
    public override Vector3 GetToLocalPosition() => SetToLocalPosition;
    public override float GetBlendTime() => SetBlendTime;
    public override float GetAmplitude() => SetAmplitude;
    public override AnimationCurve GetSpeedCurve() => SetSpeedCurve;
}