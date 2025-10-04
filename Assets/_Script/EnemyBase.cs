using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{
    [Header("見た目の設定")]
    [SerializeField] private GameObject[] _levelPrefabs;
    [SerializeField] private float _upgradeInterval = 50f;
    [Header("HP設定")]
    [SerializeField] private int _maxHP = 300; // 初期HP
    [Header("UI設定")]
    [SerializeField] private Image _hpFillImage; // HPゲージの塗りつぶし部分）

    [Header("敵ユニット生成設定")]
    [SerializeField] private GameObject _enemyPrefab; // 生成する敵プレハブ
    [SerializeField] private Transform[] _spawnPoints; // 生成位置
    [SerializeField] private float[] _spawnIntervals = { 15f, 10f, 5f, 1f }; // レベル毎の生成間隔
    [SerializeField] private bool _spawnEnabled = true; // 生成のON/OFF
    private int _currentHP;

    private int _currentLevel = 0;
    private float _timer = 0f;
    private float _spawnTimer = 0f;
    private void Start()
    {
        _currentHP = _maxHP; // HP初期化
        UpdateBaseVisual();
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _upgradeInterval)
        {
            UpgradeBase();
            _timer = 0f;
        }
        // 敵ユニット生成のタイマー
        if (_spawnEnabled && _currentLevel < _spawnIntervals.Length)
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnIntervals[_currentLevel])
            {
                SpawnEnemyUnit();
                _spawnTimer = 0f;
            }
        }
    }
    /// <summary>
    /// アップグレードの見た目
    /// </summary>
    private void UpdateBaseVisual()
    {
        // 全部非表示
        foreach (GameObject obj in _levelPrefabs)
        {
            obj.SetActive(false);
        }
        // 現在レベルだけ表示
        _levelPrefabs[_currentLevel].SetActive(true);
    }
    private void UpgradeBase()
    {
        if (_currentLevel < _levelPrefabs.Length - 1)
        {
            _currentLevel++;
            UpdateBaseVisual();
            Debug.Log("拠点がレベルアップ: " + _currentLevel);
        }
    }
    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _currentHP = Mathf.Max(0, _currentHP); // HP下限を0に設定

        Debug.Log($"敵拠点がダメージを受けた！ 残りHP: {_currentHP}");
        UpdateHPUI();

        if (_currentHP <= 0)
        {
            OnDestroyed();
        }
    }
    // 破壊された時の処理
    private void OnDestroyed()
    {
        Debug.Log("敵拠点が破壊された！");
        gameObject.SetActive(false); // 非表示に
                                     // ここでゲームオーバーやエフェクト
        SceneLoader.Instance.LoadScene("GameClear");
    }
    /// <summary>
    /// 敵ユニットを生成
    /// </summary>
    private void SpawnEnemyUnit()
    {
        // 敵プレハブがあるかチェック
        if (_enemyPrefab == null)
        {
            Debug.LogWarning("敵プレハブが設定されていません！");
            return;
        }

        // 生成位置があるかチェック
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            Debug.LogWarning("生成位置が設定されていません！");
            return;
        }
        // ランダムな生成位置を選択
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        // 敵ユニット生成
        GameObject spawnedEnemy = Instantiate(_enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    /// <summary>
    /// 強制的に敵ユニット生成
    /// </summary>
    public void ForceSpawnUnit()
    {
        SpawnEnemyUnit();
    }

    // HPを外部から取得できるように
    public int GetCurrentHP()
    {
        return _currentHP;
    }
    public int GetMaxHP()
    {
        return _maxHP;
    }
    // HPゲージとUIを更新
    private void UpdateHPUI()
    {
        if (_hpFillImage != null)
        {
            float fillAmount = (float)_currentHP / (float)_maxHP;
            _hpFillImage.DOFillAmount(fillAmount,0.2f);
        }
    }
}
