using System;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private SwipeCatcher _swipeCatcher = default;

    private void Awake()
    {
        var rootEntity = new RootEntity(new RootEntity.Context
        {
            SwipeCatcher = _swipeCatcher
        });
    }
}