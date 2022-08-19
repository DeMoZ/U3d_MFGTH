using System;
using System.Collections.Generic;
using System.Globalization;
using UI;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootEntity : IDisposable
{
    public struct Ctx
    {
    }

    private Ctx _ctx;
    private const string MENU_SCENE = "MenuScene";
    private const string SWITCH_SCENE = "2_SwitchScene";
    private const string LEVEL_SCENE_1 = "LevelScene_1";
    private const string LEVEL_SCENE_2 = "LevelScene_2";

    private IGameScene _currentScene;
    private ReactiveCommand _onPlayClick;
    private ReactiveCommand<bool> _onBoo;

    private List<IDisposable> _diposables;

    public RootEntity(Ctx ctx)
    {
        Debug.Log($"[RootEntity][time] Loading scene start.. {Time.realtimeSinceStartup}");
        _ctx = ctx;
        _diposables = new List<IDisposable>();
        _onPlayClick = new();

        //_diposables.Add(_onPlayClick.Subscribe(x => OnPlayScene()));
        _onPlayClick.Subscribe(x => OnPlayScene());
       
        SwitchScenes(GameStates.Menu);
    }

    private void SwitchScenes(GameStates scene)
    {
        // load switch scene Additive (with UI over all)
        _diposables.Add(SceneManager.LoadSceneAsync(SWITCH_SCENE) // async load scene
            .AsAsyncOperationObservable() // as Observable thread
            .Do(x =>
            {
                // call during the process
                Debug.Log($"[RootEntity][SwitchScenes] Async load scene {SWITCH_SCENE} progress: " + x.progress); // show progress
            }).Subscribe(_ =>
            {
                Debug.Log($"[RootEntity][SwitchScenes] Async load scene {SWITCH_SCENE} done");
                OnSwitchSceneLoaded(scene);
            }));
    }

    private string GetSceneName(GameStates state)
    {
        return state switch
        {
            GameStates.Menu => MENU_SCENE,
            GameStates.Level1 => LEVEL_SCENE_1,
            GameStates.Level2 => LEVEL_SCENE_2,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }

    private void OnSwitchSceneLoaded(GameStates scene)
    {
        _currentScene?.Exit();
        var onLoadingProcess = new ReactiveProperty<string>();
        var switchSceneEntity = new SwitchSceneEntity(new SwitchSceneEntity.Ctx{
            onLoadingProcess = onLoadingProcess,
        });

        Debug.Log($"[RootEntity][OnSwitchSceneLoaded] Start load scene {scene}");

        _currentScene = SceneEntity(scene);
        _diposables.Add(SceneManager.LoadSceneAsync(GetSceneName(scene)) // async load scene
            .AsAsyncOperationObservable() // as Observable thread
            .Do(x =>
            {
                // call during the process
                Debug.Log($"[RootEntity][OnSwitchSceneLoaded] Async load scene {scene} progress: " + x.progress); // show progress
                onLoadingProcess.Value = x.progress.ToString(CultureInfo.InvariantCulture);
            }).Subscribe(_ =>
            {
                switchSceneEntity.Exit();
                switchSceneEntity.Dispose();
                
                _currentScene.Enter();
            }));
    }

    private IGameScene SceneEntity(GameStates scene)
    {
        IGameScene newScene = scene switch
        {
            GameStates.Menu => LoadMenu(),
            GameStates.Level1 => LoadLevel1(),
            GameStates.Level2 => LoadLevel2(),
            _ => LoadMenu()
        };

        return newScene;
    }

    private IGameScene LoadMenu()
    {
        return new MenuSceneEntity(new MenuSceneEntity.Ctx
        {
            state = GameStates.Menu,
            onPlaySelected = _onPlayClick,
        });
    }

    private IGameScene LoadLevel1()
    {
        return new LevelSceneEntity(new LevelSceneEntity.Ctx
        {
            state = GameStates.Level1,
        });
    }

    private IGameScene LoadLevel2()
    {
        return new MenuSceneEntity(new MenuSceneEntity.Ctx
        {
            state = GameStates.Menu,
            onPlaySelected = _onPlayClick,
        });
    }

    private void OnPlayScene()
    {
    }

    public void Dispose()
    {
        foreach (var disposable in _diposables)
            disposable.Dispose();

        _currentScene.Dispose();
    }
}

public interface IGameScene : IDisposable
{
    public void Enter();
    public void Exit();
}

public class SwitchSceneEntity : IGameScene
{
    public struct Ctx
    {
        public ReactiveProperty<string> onLoadingProcess;
    }

    private Ctx _ctx;

    public SwitchSceneEntity(Ctx ctx)
    {
        _ctx = ctx;

        var ui = UnityEngine.GameObject.FindObjectOfType<UiSwitchScene>();
        ui.SetCtx(new UiSwitchScene.Ctx
        {
            onLoadingProcess = _ctx.onLoadingProcess,
        });
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Dispose()
    {
    }
}

public class LevelSceneEntity : IGameScene
{
    public struct Ctx
    {
        public GameStates state;
    }

    private Ctx _ctx;

    public LevelSceneEntity(Ctx ctx)
    {
        _ctx = ctx;

        // SwipeCatcher swipeCatcher = default;//_context.ResourceLoader.Get<SwipeCatcher>(_context.ResourceLoader.UIPrefabs.SwipeCatcher, _context.UIParent); 
        //
        //
        // IReactiveCommand<Swipe> onSwipe = new ReactiveCommand<Swipe>();
        //
        // SwipeCatcher.Context swipeCatcherCtx = new SwipeCatcher.Context
        // {
        //     OnSwipe = onSwipe
        // };
        //
        // swipeCatcher.SetContext(swipeCatcherCtx);
        //
        // PlayerSwipeInput psi = new PlayerSwipeInput(new PlayerSwipeInput.Context
        // {
        //     OnSwipe = onSwipe
        // });
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Dispose()
    {
    }
}