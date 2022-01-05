using UnityEngine;

public class PointView : MonoBehaviour
{
    [SerializeField] private AttackPointPositions _attackPointPosition;
    public AttackPointPositions AttackPointPosition => _attackPointPosition;
}