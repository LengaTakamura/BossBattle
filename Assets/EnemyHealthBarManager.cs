using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarManager : MonoBehaviour
{
    Slider _slider;
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

   public void FillUpdate(float value) => _slider.value = value;
  
}
