using UnityEngine;

public class RemovePrefix : MonoBehaviour
{
    [SerializeField] private Transform _mixamoRigObject = default;
    [SerializeField] private string _prefix = "mixamorig:";

    [ContextMenu("Remove Prefix")]
    public void RemovePrefixFromNames()
    {
        if (!_mixamoRigObject)
            _mixamoRigObject = transform;

        foreach (Transform child in _mixamoRigObject)
            FixPrefix(child);
    }

    private void FixPrefix(Transform go)
    {
        if (go.name.Contains(_prefix))
            go.name = go.name.Substring(_prefix.Length);

        foreach (Transform child in go)
            FixPrefix(child);
    }
}