using System.Collections.Generic;
using UnityEngine;

public class AttackMapView : MonoBehaviour
{
    [SerializeField] private PointView _rightDefaultPoint;
    [SerializeField] private List<PointView> _rightHandStartPoints;
    [SerializeField] private List<PointView> _rightHandEndPoints;
}