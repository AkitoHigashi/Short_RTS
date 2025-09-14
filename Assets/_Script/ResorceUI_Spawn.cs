using UnityEngine;
/// <summary>
/// �g��Ȃ��X�N���v�g
/// </summary>
public class ResorceUI_Spawn : MonoBehaviour
{
    [SerializeField] GameObject _imagePrefeb;//�����ڂ����̃Q�[���I�u�W�F�N�g
    [SerializeField] Canvas _canvas;//UI���ڂ���canvas
    [SerializeField] Camera _camera;//�Q�[���̃J����
    [SerializeField] Transform _transform;
    /// <summary>
    /// ����UI���X�|�[�����邽�߂̃��\�b�h
    /// </summary>
    public void Spawn_Resorce()
    {
        Vector3 screenPos = _camera.WorldToScreenPoint(_transform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (_canvas.transform as RectTransform, screenPos,null, out Vector2 localPos);

        GameObject resorce = Instantiate(_imagePrefeb, _canvas.transform);
        RectTransform rect = resorce.GetComponent<RectTransform>();
        rect.anchoredPosition = localPos;
    }
}
