using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

/// <summary>
/// プレイヤーの資源回収システム
/// リソースエリアに入ると自動で資源回収を開始し、エリアから出ると停止する
/// </summary>
public class Player_Resource : MonoBehaviour
{
    [Header("リソース設定")]
    [SerializeField] private ResourceUI_WorldSpace RUWS;
    [Header("プログレスUI設定")]
    [SerializeField] private Image _progressBar;
    [SerializeField] private Camera _camera;
    [Header("リソースタグ設定")]
    [SerializeField] private string _wheatTag = "Wheat";
    [SerializeField] private string _treeTag = "Tree";
    [SerializeField] private string _stoneTag = "Stone";

    [Header("回収時間設定")]
    [SerializeField, Tooltip("リソース1個を回収するのにかかる時間（秒）")]
    private float _duration = 6f;

    // 現在の処理状態を管理する変数
    private Coroutine _currentCoroutine = null;  // 現在実行中のコルーチン
    private Transform _currentTarget = null;     // 現在処理中のリソースエリア

    private void Start()
    {
        if (RUWS == null)
        {
            RUWS = FindAnyObjectByType<ResourceUI_WorldSpace>();
            if (RUWS == null)
            {
                Debug.LogError("ResourceUI_WorldSpaceがシーンに見つからない");
            }
        }
    }
    /// <summary>
    /// リソースエリアに入った時の処理
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // リソースエリアかどうかを判定
        if (IsResourceArea(other.tag))
        {
            StartNewResourceProcess(other.tag, other.transform);
        }
    }

    /// <summary>
    /// リソースエリアから出た時の処理
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        // 現在処理中のエリアから出た場合のみ処理を停止
        if (_currentTarget == other.transform)
        {
            StopCurrentResourceProcess();
            Debug.Log($"{other.tag}エリア({other.name})で資源回収終了！");
        }
    }

    /// <summary>
    /// 指定されたタグがリソースエリアかどうかを判定
    /// </summary>
    /// <param name="tag">判定するタグ</param>
    /// <returns>リソースエリアの場合true</returns>
    private bool IsResourceArea(string tag)
    {
        return tag == _wheatTag || tag == _treeTag || tag == _stoneTag;
    }

    /// <summary>
    /// 新しいリソース回収処理を開始
    /// </summary>
    /// <param name="resourceType">リソースの種類</param>
    /// <param name="target">処理対象のTransform</param>
    private void StartNewResourceProcess(string resourceType, Transform target)
    {
        // 既存の処理があれば停止
        StopCurrentResourceProcess();

        // 新しい処理を開始
        _currentTarget = target;
        _currentCoroutine = StartCoroutine(ResourceProcess(resourceType, target));

        Debug.Log($"{resourceType}エリア({target.name})で資源回収開始！");
    }

    /// <summary>
    /// 現在のリソース回収処理を停止
    /// </summary>
    private void StopCurrentResourceProcess()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            _currentTarget = null;
            Debug.Log("リソース回収処理を停止しました");
        }
    }

    /// <summary>
    /// リソース回収のメインループ
    /// 指定した間隔でリソースを生成し続ける
    /// </summary>
    /// <param name="resourceType">リソースの種類（デバッグ用）</param>
    /// <param name="target">リソース生成対象のTransform</param>
    /// <returns>コルーチン</returns>
    private IEnumerator ResourceProcess(string resourceType, Transform target)
    {
        while (true)
        {
            // 回収時間のカウントダウン
            float timer = 0f;
            while (timer < _duration)
            {
                timer += Time.deltaTime;

                // 進捗率を計算（0～1の範囲）
                float progress = Mathf.Clamp01(timer / _duration);
                UpdateProgressUI(progress);

                // 次のフレームまで待機
                yield return null;
            }

            // リソースを生成
            RUWS.Spawn_Resource(resourceType, target);
            Debug.Log($"{resourceType}リソースを回収しました！");
        }
    }
    private void UpdateProgressUI(float progress)
    { // プログレスバーを更新
        if (_progressBar != null)
        {
            _progressBar.DOFillAmount(progress, 0.4f);
        }
    }
    private void Update()
    {
        if (_progressBar != null)
        {
            _progressBar.transform.LookAt(_camera.transform.position);
        }

    }

    /// <summary>
    /// オブジェクト破棄時のクリーンアップ処理
    /// メモリリークを防ぐために必要
    /// </summary>
    private void OnDestroy()
    {
        StopCurrentResourceProcess();
    }
}