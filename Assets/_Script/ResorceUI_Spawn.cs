using UnityEngine;
/// <summary>
/// 使わないスクリプト
/// </summary>
public class ResorceUI_Spawn : MonoBehaviour
{
    [SerializeField] GameObject _imagePrefeb;//見た目だけのゲームオブジェクト
    [SerializeField] Canvas _canvas;//UIを載せるcanvas
    [SerializeField] Camera _camera;//ゲームのカメラ
    [SerializeField] Transform _transform;
    /// <summary>
    /// 資源UIをスポーンするためのメソッド
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
