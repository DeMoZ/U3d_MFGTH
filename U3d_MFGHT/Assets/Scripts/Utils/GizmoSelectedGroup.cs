using UnityEngine;

public class GizmoSelectedGroup : MonoBehaviour
{
    private GizmoSelected[] _gizmos;

    private void Start()
    {
        GetChildGizmos();
    }

    [ContextMenu(nameof(GetChildGizmos))]
    private void GetChildGizmos()
    {
        _gizmos = GetComponentsInChildren<GizmoSelected>();
    }

    private void OnDrawGizmosSelected()
    {
        if (_gizmos == null) return;
        
        foreach (var gizmo in _gizmos)
            gizmo.Draw();
    }
}