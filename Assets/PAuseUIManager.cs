using DG.Tweening;
using UnityEngine;

public class PAuseUIManager : MonoBehaviour,IPause
{
    CanvasGroup _canvasGroup;
    int count = 0;
    private void Awake()
    {
        _canvasGroup =GameObject.Find("PausePanel").GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    _canvasGroup.gameObject.SetActive(true);
        //}
    }

    void IPause.Pause()
    {
        count++;
        if(count > 1)
        _canvasGroup.DOFade(1, 0.5f);
    }

    void IPause.Resume()
    {
        _canvasGroup.DOFade(0, 0.5f);
    }
}
