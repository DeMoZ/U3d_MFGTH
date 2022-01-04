using UnityEngine;

[CreateAssetMenu]
public class AttackConfig : ScriptableObject
{
    [SerializeField] private AttackPoints _defaultLocalPosition;
    [SerializeField] private AttackPoints _fromLocalPosition;
    [SerializeField] private AttackPoints _toLocalPosition;
    [SerializeField] private float _blendTime = 0.5f;
    [SerializeField] private float _amplitude = 0.4f;
    [SerializeField] private AnimationCurve _speedCurve;

    public AttackPoints GetDefaultLocalPosition() => _defaultLocalPosition;
    public AttackPoints GetFromLocalPosition() => _fromLocalPosition;
    public AttackPoints GetToLocalPosition() => _toLocalPosition;
    public AnimationCurve GetSpeedCurve() => _speedCurve;
    public float GetBlendTime() => _blendTime;
    public float GetAmplitude() => _amplitude;
}