
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineUserInputZoom : CinemachineExtension
{
    // Input Manager�̓��͖�
    [SerializeField] private string _inputName = "Mouse ScrollWheel";

    // ���͒l�Ɋ|����l
    [SerializeField] private float _inputScale = 100;

    // FOV�̍ŏ��E�ő�
    [SerializeField, Range(1, 179)] private float _minFOV = 10;
    [SerializeField, Range(1, 179)] private float _maxFOV = 90;

    // �ω����邨���悻�̎���[s]
    [SerializeField] private float _smoothTime = 0.1f;
    // �ω��̍ő呬�x
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
        // Aim�̒��ゾ�����������{
        if (stage != CinemachineCore.Stage.Aim)
            return;

        var lens = state.Lens;

        if (!Mathf.Approximately(_scrollDelta, 0))
        {
            // FOV�̕␳�ʂ��v�Z
            _targetAdjustFOV = Mathf.Clamp(
                _targetAdjustFOV - _scrollDelta * _inputScale,
                _minFOV - lens.FieldOfView,
                _maxFOV - lens.FieldOfView
            );

            _scrollDelta = 0;
        }

        // FOV�l���v�Z
        _currentAdjustFOV = Mathf.SmoothDamp(
            _currentAdjustFOV,
            _targetAdjustFOV,
            ref _currentAdjustFOVVelocity,
            _smoothTime,
            _maxSpeed,
            deltaTime
        );

        // state�̓��e�͖��񃊃Z�b�g�����̂ŁA
        // ����␳����K�v������
        lens.FieldOfView += _currentAdjustFOV;

        state.Lens = lens;
    }
}