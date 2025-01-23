using R3;
using UnityEngine;
using R3.Triggers;
using System;
public class InputSystem : MonoBehaviour
{
    private Subject<PlayerBase.MotionIndex> _getFSubject;
    public Observable<PlayerBase.MotionIndex> GetFObserbable => _getFSubject;

    [SerializeField]
    private PlayerBase _player;
    void Start()
    {
        var clickF = Observable.EveryUpdate()
            .Where(_ => _player.CanSkating)
            .Where(_ => Input.GetKeyDown(KeyCode.F))
            .Scan(0, (count, _) => count + 1)
            .Where(count => count % 2 != 0)
            .Subscribe(count => {
                Debug.Log($"skate‚ÉˆÚs‰Â”\:{count}‰ñ–Ú");
                _player.StateChange(PlayerBase.MotionIndex.Skate);
            }).AddTo(this);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
