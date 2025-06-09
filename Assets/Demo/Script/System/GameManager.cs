using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Button _backButton;
    public Action Ended;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _backButton.gameObject.SetActive(false);
    }

    public void GameOvered()
    {
        Ended?.Invoke();
       _backButton.gameObject.SetActive(true);
       Cursor.visible = true;
       Cursor.lockState = CursorLockMode.None;
    }

    public void GameCleared()
    {
        Ended?.Invoke();
        _backButton.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    

    public void RestartGame( )
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        AppQuit();
    }

    void AppQuit()
    {
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
             Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}
