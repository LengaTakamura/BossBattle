using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class PlayerHuman : PlayerBase
{
    private void Start()
    {
        var clickF = Observable.EveryUpdate()
             .Where(_ => Input.GetKeyDown(KeyCode.F))
             .Scan(MotionIndex.Idol, (currentState, input) =>
             {
                 if (currentState == MotionIndex.Skating)
                 {
                     if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                     {
                         return MotionIndex.Run;
                     }
                     else
                     {
                         return MotionIndex.Idol;
                     }
                 }
                 else
                 {
                     return MotionIndex.Skating;

                 }

             })
             .Subscribe(newState =>
             {
                 StateChange(newState);

             }).AddTo(this);

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {

        base.FixedUpdate();

    }
}
