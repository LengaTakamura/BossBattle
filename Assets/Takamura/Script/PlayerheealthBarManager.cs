using UnityEngine;
using UnityEngine.UI;

public class PlayerheealthBarManager : MonoBehaviour
{
    Slider _slider;
    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void FillUpdate(float value) => _slider.value = value;
}
