using UnityEngine;

/// <summary>
/// contains bones and targets of the body
/// </summary>
public class BodyParts : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _rHandTarget;

    public Transform Body => _body;
    public Transform RHTarget => _rHandTarget;
}