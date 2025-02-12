using DG.Tweening;
using R3;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
public class InputSystem : MonoBehaviour
{
    [SerializeField]
    CanvasGroup _startUI;

    private Subject<PlayerBase.MotionIndex> _getFSubject;
    public Observable<PlayerBase.MotionIndex> GetFObserbable => _getFSubject;

    [SerializeField]
    private PlayerBase _player;
    [SerializeField]
    EnemyManager _enemyManager;
    [SerializeField]
    Canvas _canvas;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        var clickF = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.F))
            .Where(_ => _player.gameObject.activeSelf)
            .Scan(0, (count, _) => count + 1)
            .Subscribe(count => {

              
                if (count % 2 != 0)
                {
                    _player.StateChange(PlayerBase.MotionIndex.Skating);
                    _canvas.gameObject.SetActive(false);
                   _enemyManager.IsSkating = true;
                }
                else
                {
          
                    _player.StateChange(PlayerBase.MotionIndex.NonAvoid);
                    _canvas.gameObject.SetActive(true);
                    _enemyManager.IsSkating = false;
                }
               

            }).AddTo(this);

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
