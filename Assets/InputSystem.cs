using R3;
using UnityEngine;
public class InputSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerBase _player;
    void Start()
    {
        var attack = Observable.EveryUpdate()
             .Where(_ => Input.GetMouseButton(0))
             .Scan(0, (count, _) => count + 1)
             .Subscribe(count =>
             {
                 if (count % 5 == 0)
                 {

                 }
                 else
                 {

                 }
             });
        
    }


}
