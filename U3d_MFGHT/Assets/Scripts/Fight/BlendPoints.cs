using System.Collections;
using UnityEngine;

public class BlendPoints : MonoBehaviour
{
    [SerializeField] private Transform _playerTransfrom;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _fromPoint;
    [SerializeField] private Transform _toPoint;
    [SerializeField] private float _blendTime = 0.5f;
    [SerializeField] private float _amplitude = 0.4f;
    [SerializeField] private AnimationCurve _speedCurve;

    [SerializeField] private bool repeat = false;
    private Coroutine _blendRoutine;

    [ContextMenu(nameof(Blend))]
    public void Blend()
    {
        if (_blendRoutine != null)
        {
            StopCoroutine(_blendRoutine);
            _blendRoutine = null;
        }

        _blendRoutine = StartCoroutine(IEBlend());
    }

    private void Update()
    {
        if (repeat && _blendRoutine == null)
            _blendRoutine = StartCoroutine(IEBlend());
    }

    private IEnumerator IEBlend()
    {
        var timer = 0f;
        float timerScaled;
        float amplitude;
        Vector3 straightPosition;
        Vector3 position;
        while (timer < _blendTime)
        {
            yield return null;
            /*             
                Vector3 shoulderToStraight = straightPosition - _shoulder.position;
                Vector3 fromTo = _toPoint.position - _fromPoint.position;                
                Vector3 cross = Vector3.Cross(shoulderToStraight, fromTo);
                Vector3 rotatedVector = Quaternion.AngleAxis(-90, cross) * fromTo;                
                Vector3 position = straightPosition + rotatedVector.normalized * amplitude;              
            */
            timerScaled = _speedCurve.Evaluate(timer / _blendTime);
            amplitude = Mathf.Sin(timerScaled * Mathf.PI) * _amplitude;
            straightPosition = Vector3.Lerp(_fromPoint.position, _toPoint.position, timerScaled);
            position = straightPosition + _playerTransfrom.forward * amplitude;

            _target.position = position;
            timer += Time.deltaTime;
        }

        _target.position = _toPoint.position;
        _blendRoutine = null;
    }
}

/*public class TheHit : ScriptableObject
{
    [SerializeField] private Transform _fromPoint;
    [SerializeField] private Transform _toPoint;
    [SerializeField] private float _blendTime = 0.5f;
    [SerializeField] private float _amplitude = 0.4f;
    [SerializeField] private AnimationCurve _speedCurve;
}*/

/*public class BlendAttackEntity
{
    public struct Context
    {
        private Transform _playerTransfrom;
        private Transform _target;
        private Transform _fromPoint;
        private Transform _toPoint;
        private float _blendTime;
        private float _amplitude;
        private AnimationCurve _speedCurve;
    }
}*/

/*public class BlendAttackSchemeView
{
    public List<Ancor> _ancors;

    public struct Context
    {
        private Transform _playerTransfrom;
        private Transform _target;
        private Transform _fromPoint;
        private Transform _toPoint;
        private float _blendTime;
        private float _amplitude;
        private AnimationCurve _speedCurve;
    }
}*/

/*public class Ancor : MonoBehaviour
{
    [field: SerializeField] public AnchorType AnchorType { get; private set; }
}

public enum AnchorType
{
    Default,
    R_FromUp,
    R_FromDown,
    R_FromLeft,
    R_FromRight,
    R_ToUp,
    R_ToDown,
    R_ToLeft,
    R_ToRight,
}*/

/*public class BlendAttackPm
{
    public struct Context
    {
        private Transform _playerTransfrom;
        private Transform _target;
        private Transform _fromPoint;
        private Transform _toPoint;
        private float _blendTime;
        private float _amplitude;
        private AnimationCurve _speedCurve;
    }

    private IEnumerator IEBlend()
    {
        var timer = 0f;
        float timerScaled;
        float amplitude;
        Vector3 straightPosition;
        Vector3 position;
        while (timer < _blendTime)
        {
            yield return null;
            /*             
                Vector3 shoulderToStraight = straightPosition - _shoulder.position;
                Vector3 fromTo = _toPoint.position - _fromPoint.position;                
                Vector3 cross = Vector3.Cross(shoulderToStraight, fromTo);
                Vector3 rotatedVector = Quaternion.AngleAxis(-90, cross) * fromTo;                
                Vector3 position = straightPosition + rotatedVector.normalized * amplitude;              
            #1#
            timerScaled = _speedCurve.Evaluate(timer / _blendTime);
            amplitude = Mathf.Sin(timerScaled * Mathf.PI) * _amplitude;
            straightPosition = Vector3.Lerp(_fromPoint.position, _toPoint.position, timerScaled);
            position = straightPosition + _playerTransfrom.forward * amplitude;

            _target.position = position;
            timer += Time.deltaTime;
        }

        _target.position = _toPoint.position;
        _blendRoutine = null;
    }
}*/