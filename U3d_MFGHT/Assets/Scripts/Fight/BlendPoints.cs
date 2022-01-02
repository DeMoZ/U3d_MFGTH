using System.Collections;
using UnityEngine;

public class BlendPoints : MonoBehaviour
{
    [SerializeField] private Transform _playerTransfrom;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _shoulder;
    [SerializeField] private Transform _fromPoint;
    [SerializeField] private Transform _toPoint;
    [SerializeField] private float _blendTime = 0.5f;
    [SerializeField] private float _amplitude = 1f;
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
            var timeScaled = _speedCurve.Evaluate(timer / _blendTime);
            amplitude = Mathf.Sin(timeScaled * Mathf.PI) * _amplitude;
            straightPosition = Vector3.Lerp(_fromPoint.position, _toPoint.position, timeScaled);
            position = straightPosition + _playerTransfrom.forward * amplitude;

            _target.position = position;
            timer += Time.deltaTime;
        }

        _target.position = _toPoint.position;
        _blendRoutine = null;
    }
}