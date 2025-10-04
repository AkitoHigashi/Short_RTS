using UnityEngine;

public class Base_Archery : Base
{
    [SerializeField] private GameObject _soldierPrefab; // ソルジャーのPrefab
    [SerializeField] private Transform _spawnPoint; // 生成位置
    [SerializeField] private int _stoneCostForSpawn = 10; // 必要素材な素材
    [SerializeField] private int _plusCost = 0; // 増加コスト基本0
    private int _currentCost; // 今のコスト
    public override int ReqStone
    {
        get { return _currentCost; }
    }

    // 木と麦は必要ないのでオーバーライド
    public override int ReqTree
    {
        get { return 0; }
    }

    public override int ReqWheat
    {
        get { return 0; }
    }

    protected override void OnEnable()
    {
        _currentCost = _stoneCostForSpawn; // 初期設定
        base.OnEnable();
    }
    public override void Upgrade()
    {
        // 基底クラスのUpgrade()を呼ばずに、直接SpawnSoldier()を実行
        // これにより _currentLevel の増加処理を回避
        SpawnSoldier();
    }

    /// <summary>
    /// 資源あるかチェック
    /// </summary>
    /// <returns></returns>
    private bool CanSpawnSoldier()
    {
        return Inventory.Instance.CheckResource(0, _currentCost, 0);
    }

    /// <summary>
    /// ソルジャーを生成するメソッド
    /// </summary>
    public void SpawnSoldier()
    {
        if (CanSpawnSoldier() == false)
        {
            Debug.Log("石が足りません");
            return;
        }

        // 石消費
        Inventory.Instance.UseResourse(0, _currentCost, 0);

        // ソルジャーを生成
        GameObject newSoldier = Instantiate(_soldierPrefab, _spawnPoint.position, Quaternion.identity);
        Debug.Log("ソルジャー生成！！");

        // コスト増加
        _currentCost += _plusCost;

        // UI更新
        UIUpdate();
    }

    /// <summary>
    /// レベルアップしないため、ソルジャー生成可能かをチェック
    /// 最大レベル制限は無視する
    /// </summary>
    /// <returns></returns>
    public override bool CheckUpgrade()
    {
        // 基底クラスのレベルチェックを無視して、資源チェックのみ実行
        return CanSpawnSoldier();
    }

    /// <summary>
    /// レベル制限を無視するように
    /// </summary>
    public override void UIUpdate()
    {
        // 必要資材と数をUIに反映
        if (ReqTree > 0)
        {
            _treeImage.gameObject.SetActive(true);
            _reqText.text = ReqTree.ToString();
        }
        else if (ReqStone > 0)
        {
            _stoneImage.gameObject.SetActive(true);
            _reqText.text = ReqStone.ToString();
        }
        else if (ReqWheat > 0)
        {
            _wheatImage.gameObject.SetActive(true);
            _reqText.text = ReqWheat.ToString();
        }
    }
}
