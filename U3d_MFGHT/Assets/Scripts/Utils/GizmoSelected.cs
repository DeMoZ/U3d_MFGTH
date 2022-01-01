using System;
using UnityEngine;

public class GizmoSelected : MonoBehaviour
{
    [SerializeField] private SelectedGizmoType _gizmo;
    [SerializeField] private float _radius = 0.1f;
    [SerializeField] private Color _color = Color.yellow;

    private void OnDrawGizmosSelected()
    {
        Draw();
    }

    public void Draw()
    {
        Gizmos.color = _color;
        switch (_gizmo)
        {
            case SelectedGizmoType.Sphere:
                Gizmos.DrawSphere(transform.position, _radius);
                break;
            case SelectedGizmoType.Cube:
                Gizmos.DrawCube(transform.position, Vector3.one * _radius);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}