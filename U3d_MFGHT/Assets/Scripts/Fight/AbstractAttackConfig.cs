using UnityEngine;

//[CreateAssetMenu]
public abstract class AbstractAttackConfig : ScriptableObject
{
    public abstract Vector3 GetDefaultLocalPosition();
    public abstract Vector3 GetFromLocalPosition ();
    public abstract Vector3 GetToLocalPosition ();
    public abstract float GetBlendTime ();
    public abstract float GetAmplitude ();
    public abstract AnimationCurve GetSpeedCurve ();
}

