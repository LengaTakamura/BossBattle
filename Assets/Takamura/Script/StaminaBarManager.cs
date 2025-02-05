using UnityEngine;
using UnityEngine.UI;

public class StaminaBarManager : MonoBehaviour
{

    Slider _slider;
    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void SliderUpdate(float value) => _slider.value = value;

}
