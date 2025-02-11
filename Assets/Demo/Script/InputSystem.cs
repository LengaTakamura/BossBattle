using DG.Tweening;
using UnityEngine;
public class InputSystem : MonoBehaviour
{
    [SerializeField]
    CanvasGroup _startUI;
    public void OnClicked()
    {
        _startUI.DOFade(0, 1.0f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
}
