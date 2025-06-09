
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineUserInputZoom : CinemachineExtension
{
    // Input Managerの入力名
    [SerializeField] private string _inputName = "Mouse ScrollWheel";

    // 入力値に掛ける値
    [SerializeField] private float _inputScale = 100;

    // FOVの最小・最大
    [SerializeField, Range(1, 179)] private float _minFOV = 10;
    [SerializeField, Range(1, 179)] private float _maxFOV = 90;

    // 変化するおおよその時間[s]
    [SerializeField] private float _smoothTime = 0.1f;
    // 変化の最大速度
    [SerializeField] private float _maxSpeed = Mathf.Infinity;

    private float _scrollDelta;
    private float _targetAdjustFOV;
    private float _currentAdjustFOV;
    private float _currentAdjustFOVVelocity;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {
        _scrollDelta += Input.GetAxis(_inputName);
        // Aimの直後だけ処理を実施
        if (stage != CinemachineCore.Stage.Aim)
            return;

        var lens = state.Lens;

        if (!Mathf.Approximately(_scrollDelta, 0))
        {
            // FOVの補正量を計算
            _targetAdjustFOV = Mathf.Clamp(
                _targetAdjustFOV - _scrollDelta * _inputScale,
                _minFOV - lens.FieldOfView,
                _maxFOV - lens.FieldOfView
            );

            _scrollDelta = 0;
        }

        // FOV値を計算
        _currentAdjustFOV = Mathf.SmoothDamp(
            _currentAdjustFOV,
            _targetAdjustFOV,
            ref _currentAdjustFOVVelocity,
            _smoothTime,
            _maxSpeed,
            deltaTime
        );

        // stateの内容は毎回リセットされるので、
        // 毎回補正する必要がある
        lens.FieldOfView += _currentAdjustFOV;

        state.Lens = lens;
    }
}