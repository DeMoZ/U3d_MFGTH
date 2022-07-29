using UnityEngine;

public class EntryRoot : MonoBehaviour
{
    //[SerializeField] private ResourceLoader _resourceLoader = default;
    [SerializeField] private Transform UIInputParent = default;
    private void Awake()
    {
        var rootEntity = new RootEntity(new RootEntity.Context
        {
            //ResourceLoader = _resourceLoader,
            //UIParent = UIInputParent
        });
    }
}