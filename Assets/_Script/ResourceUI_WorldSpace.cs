using UnityEngine;

public class ResourceUI_WorldSpace : MonoBehaviour
{
    [SerializeField] GameObject _imagePrefab; // 見た目だけのゲームオブジェクト
    [SerializeField] Canvas _canvas; // UIを載せるCanvas
    [SerializeField] Camera _camera; // ゲームのカメラ
    //[SerializeField] Transform _transform;

    /// <summary>
    /// 資源UIをスポーンするためのメソッド
    /// </summary>
    public void Spawn_Resource(Transform target)
    {
        //canvas下に生成
        GameObject resource = Instantiate(_imagePrefab, _canvas.transform);

        // ResourceUIElementコンポーネントを追加
        ResourceUIElement element = resource.AddComponent<ResourceUIElement>();
        //
        element.Initialize(target, _camera, _canvas);
    }
}

/// <summary>
/// 個別のResource UI要素を管理するクラス
/// </summary>
public class ResourceUIElement : MonoBehaviour
{
    private Transform _worldTarget;
    private Camera _camera;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private bool _isTracking = true;
    private float _trackingTime = 0.7f;//生成の0.3f 待機の0.5f

    //初期値設定
    public void Initialize(Transform worldTarget, Camera camera, Canvas canvas)
    {
        _worldTarget = worldTarget;
        _camera = camera;
        _canvas = canvas;
        _rectTransform = GetComponent<RectTransform>();
        //トラッキング解除
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
    /// Updateで位置修正
    /// </summary>
    private void UpdatePosition()
    {
        // ワールド座標をスクリーン座標に変換
        Vector3 screenPos = _camera.WorldToScreenPoint(_worldTarget.position);

        // スクリーンからUIに表示
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screenPos,
            null,
            out Vector2 localPos);

        _rectTransform.anchoredPosition = localPos;
    }
}