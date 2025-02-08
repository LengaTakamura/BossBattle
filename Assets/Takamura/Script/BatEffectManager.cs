using UnityEngine;

public class BatEffectManager : MonoBehaviour
{
    [SerializeField]
    float _speed = 3f;

    GameObject _bat;

    private Vector3 _currentTargetPosition;
    public void Initialized(BatManager bat)
    {
        bat.GetTargetPos += UpdateTartgetPos;
    }

    private void Update()
    {
        MoveToTarget();
        Debug.Log(_currentTargetPosition);
    }

    private void UpdateTartgetPos(Vector3 newPos)
    {
        _currentTargetPosition = newPos;
    }
    public void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTargetPosition + new Vector3(0,3,0), Time.deltaTime * _speed);
    }
}
