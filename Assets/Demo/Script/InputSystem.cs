using DG.Tweening;
using UnityEngine;
public class InputSystem : MonoBehaviour
{
    [SerializeField]
    CanvasGroup _startUI;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        GameStart();
    }
    public void GameStart()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        _startUI.DOFade(0, 2.0f);
        
    }

    
}
