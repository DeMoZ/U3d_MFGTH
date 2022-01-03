using UnityEngine;

[CreateAssetMenu]
public class ResourceLoader : ScriptableObject
{
    public UIPrefabs UIPrefabs;
    public PlayerView PlayerView;

    public T Get<T>(T objectPrefab, Transform parent) where T : MonoBehaviour
    {
        return UnityEngine.GameObject.Instantiate(objectPrefab,parent);
    }

    public PlayerView GetPlayer(Transform parent)
    {
        return Instantiate(PlayerView, parent);
    }
}