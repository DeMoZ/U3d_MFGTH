using System.Collections.Generic;
using UnityEngine;

public class AttackMapView : MonoBehaviour
{
    [SerializeField] private PointView _rightDefaultPoint;
    [SerializeField] private List<PointView> _rightHandStartPoints;
    [SerializeField] private List<PointView> _rightHandEndPoints;

    public PointView RHDefaultPoint => _rightDefaultPoint;
    public List<PointView> RHStartPoints => _rightHandStartPoints;
    public List<PointView> RHEndPoints => _rightHandEndPoints;
}