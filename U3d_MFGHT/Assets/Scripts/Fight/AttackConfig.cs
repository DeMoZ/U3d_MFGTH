using UnityEngine;

[CreateAssetMenu]
public class AttackConfig : ScriptableObject
{
    [SerializeField] private AttackPointPositions _fromLocalPosition;
    [SerializeField] private AttackPointPositions _toLocalPosition;
    [SerializeField] private float _blendTime = 0.5f;
    [SerializeField] private float _amplitude = 0.4f;
    [SerializeField] private AnimationCurve _speedCurve;

    public AttackPointPositions GetFromLocalPosition() => _fromLocalPosition;
    public AttackPointPositions GetToLocalPosition() => _toLocalPosition;
    public AnimationCurve GetSpeedCurve() => _speedCurve;
    public float GetBlendTime() => _blendTime;
    public float GetAmplitude() => _amplitude;
}