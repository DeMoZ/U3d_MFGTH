using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class AttackConfigCreatorView : MonoBehaviour
{
    [Header("Class not in use. Required changes.")]
    [Space(20)]
    [SerializeField] private string _fileName = "AttackConfig";
    [Space] 
    [SerializeField] private Transform _playerTransfrom;
    [SerializeField] private Transform _target;
    [Space]
    [SerializeField] private Transform _defaultPoint;
    [SerializeField] private Transform _fromPoint;
    [SerializeField] private Transform _toPoint;
    [Space]
    [SerializeField] private float _blendTime = 0.5f;
    [SerializeField] private float _amplitude = 0.4f;
    [SerializeField] private AnimationCurve _speedCurve;
    [Space]
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

    /// <summary>
    /// The method not in use currently. Required to make changes in AbstractAttackConfig.
    /// Then need to figure out how to use override Vector3 value. b 
    /// </summary>
    [ContextMenu(nameof(CreateScriptable))]
    public void CreateScriptable()
    {
        var attackInstance = ScriptableObject.CreateInstance(typeof(AttackConfigOverride)) as AttackConfigOverride;
        attackInstance.SetDefaultLocalPosition = _defaultPoint.localPosition;
        attackInstance.SetFromLocalPosition = _fromPoint.localPosition;
        attackInstance.SetToLocalPosition = _toPoint.localPosition;
        attackInstance.SetAmplitude = _amplitude;
        attackInstance.SetBlendTime = _blendTime;
        attackInstance.SetSpeedCurve = _speedCurve;

        Type projectWindowUtilType = typeof(ProjectWindowUtil);
        MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
        object obj = getActiveFolderPath.Invoke(null, new object[0]);
        var pathToCurrentFolder = obj.ToString();
        var newFilePath = $"{pathToCurrentFolder}/{_fileName}.asset";

        Debug.Log($"Creating file {newFilePath}");

        if (File.Exists(newFilePath))
            Debug.LogError($"File already exists {newFilePath}");
        else
            AssetDatabase.CreateAsset(attackInstance, newFilePath);
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