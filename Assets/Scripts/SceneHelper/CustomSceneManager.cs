using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
public class CustomSceneManager : MonoBehaviour
{

    public static CustomSceneManager Instance; // Реализация синглтона для удобства доступа
    private AsyncOperationHandle<SceneInstance> _sceneHandle;
    private List<AsyncOperationHandle<GameObject>> _objects;
    private void Awake(){
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(SO_SceneConfig config){
        // Выгружаем предыдущий уровень
        UnloadCurrentLevel();
        _objects = new List<AsyncOperationHandle<GameObject>>();

        foreach (AssetReferenceGameObject item in config.Environment){
            Addressables.LoadAssetAsync<GameObject>(config.Environment[0]).Completed += HandleEnvironmentLoad;
        }

        Addressables.LoadSceneAsync(config.SceneID).Completed += HandleScenarioLoad;
    }

    private void HandleScenarioLoad(AsyncOperationHandle<SceneInstance> obj){
        _sceneHandle = obj;
    }
    private void HandleEnvironmentLoad(AsyncOperationHandle<GameObject> obj){
        if (obj.Status == AsyncOperationStatus.Succeeded){
            _objects.Add(obj);
            Instantiate(obj.Result);
        }
        else{
            Debug.LogError("Failed to load environment.");
        }
    }

    private void UnloadCurrentLevel(){
        if (_sceneHandle.IsValid()){
            Addressables.UnloadSceneAsync(_sceneHandle, true);
        }
        foreach (AsyncOperationHandle<GameObject> obj in _objects){
            Addressables.Release(obj);
        }
    }

}