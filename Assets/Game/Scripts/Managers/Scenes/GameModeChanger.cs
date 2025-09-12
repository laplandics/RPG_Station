using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeChanger : MonoBehaviour
{
    public static GameModeChanger Instance  {get; private set;}
    [SerializeField] private string battleScene;
    [SerializeField] private string exploringScene;
    
    private Player _player;
    private bool _isDataOnCurrentSceneSaved;
    private bool _isNewSceneLoaded;
    private void Awake()
    {
        if (!Instance) {Instance = this; DontDestroyOnLoad(gameObject);}
        else Destroy(gameObject);
    }

    public void StartBattle()
    {
        StartCoroutine(LoadBattleScene());
    }

    public void StartExploring()
    {
        StartCoroutine(LoadExploringScene());
    }

    private IEnumerator LoadBattleScene()
    {
        _isDataOnCurrentSceneSaved = false;
        yield return new WaitForSeconds(0.1f);
        
        GameManager.Instance.OnAllPersistentDataSaved += OnSave;
        SaveloadSystem.SaveAll();
        yield return new WaitUntil(() => _isDataOnCurrentSceneSaved);
        GameManager.Instance.OnAllPersistentDataSaved -= OnSave;
        SceneManager.LoadScene(battleScene);
        _isDataOnCurrentSceneSaved = false;
    }

    private IEnumerator LoadExploringScene()
    {
        _isNewSceneLoaded = false;
        yield return new WaitForSeconds(0.1f);
        
        SceneManager.sceneLoaded += OnLoad;
        SceneManager.LoadScene(exploringScene);
        yield return new WaitUntil(() => _isNewSceneLoaded);
        SceneManager.sceneLoaded -= OnLoad;
        SaveloadSystem.LoadAll();
        _player = FindFirstObjectByType<Player>();
        _player.controller.SetLockState(false);
    }

    private void OnSave() => _isDataOnCurrentSceneSaved = true;
    private void OnLoad(Scene scene, LoadSceneMode mode) => _isNewSceneLoaded = true;
}
