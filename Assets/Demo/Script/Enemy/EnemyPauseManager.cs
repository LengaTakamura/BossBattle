using UnityEngine;

public class EnemyPauseManager : MonoBehaviour,IPause
{
    EnemyManager _enemy;

    float _speed;
    private void Start()
    {
        _enemy = transform.root.GetComponent<EnemyManager>();
    }
    void IPause.Pause()
    {
       _speed =  _enemy.OnPause();
    }

    void IPause.Resume()
    {
        _enemy.OnResume(_speed);
    }
}
