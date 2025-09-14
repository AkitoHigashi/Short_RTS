using UnityEngine;

public class ResourceUI_WorldSpace : MonoBehaviour
{
    [SerializeField] GameObject _imagePrefab; // �����ڂ����̃Q�[���I�u�W�F�N�g
    [SerializeField] Canvas _canvas; // UI���ڂ���Canvas
    [SerializeField] Camera _camera; // �Q�[���̃J����
    //[SerializeField] Transform _transform;

    /// <summary>
    /// ����UI���X�|�[�����邽�߂̃��\�b�h
    /// </summary>
    public void Spawn_Resource(Transform target)
    {
        //canvas���ɐ���
        GameObject resource = Instantiate(_imagePrefab, _canvas.transform);

        // ResourceUIElement�R���|�[�l���g��ǉ�
        ResourceUIElement element = resource.AddComponent<ResourceUIElement>();
        //
        element.Initialize(target, _camera, _canvas);
    }
}

/// <summary>
/// �ʂ�Resource UI�v�f���Ǘ�����N���X
/// </summary>
public class ResourceUIElement : MonoBehaviour
{
    private Transform _worldTarget;
    private Camera _camera;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private bool _isTracking = true;
    private float _trackingTime = 0.7f;//������0.3f �ҋ@��0.5f

    //�����l�ݒ�
    public void Initialize(Transform worldTarget, Camera camera, Canvas canvas)
    {
        _worldTarget = worldTarget;
        _camera = camera;
        _canvas = canvas;
        _rectTransform = GetComponent<RectTransform>();
        //�g���b�L���O����
        Invoke(nameof(StopTracking),_trackingTime);
    }

    void Update()
    {
        if (_isTracking && _worldTarget != null && _camera != null && _canvas != null)
        {
            UpdatePosition();
        }
    }

    private void StopTracking()
    {
        _isTracking = false;
    }
    /// <summary>
    /// Update�ňʒu�C��
    /// </summary>
    private void UpdatePosition()
    {
        // ���[���h���W���X�N���[�����W�ɕϊ�
        Vector3 screenPos = _camera.WorldToScreenPoint(_worldTarget.position);

        // �X�N���[������UI�ɕ\��
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screenPos,
            null,
            out Vector2 localPos);

        _rectTransform.anchoredPosition = localPos;
    }
}