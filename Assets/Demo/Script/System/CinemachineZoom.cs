using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CinemachineZoom : MonoBehaviour
{
   　private CinemachineCamera _camera;
    [SerializeField] private float _scrollSpeed = 10f;
    [SerializeField] private float _minFOV = 10f;
    [SerializeField] private float _maxFOV = 60f;
    private void Start()
    {
        _camera = GetComponent<CinemachineCamera>();
    }

    void Update()
    {
        ZooomOut();
    }

    void ZooomOut()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            _camera.Lens.FieldOfView -= scroll * _scrollSpeed;
            _camera.Lens.FieldOfView = Mathf.Clamp(_camera.Lens.FieldOfView, _minFOV, _maxFOV);
        }
    }
}
