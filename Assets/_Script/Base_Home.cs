using UnityEngine;

public class Base_Home : Base
{
    [SerializeField] private GameObject _minionPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _wheatCostForSpawn = 5;//処理時の必要の消費量
    [SerializeField] private int _plusCost = 3;//増加コスト

    private int _currentCost;//今のコスト

    public override int ReqWheat
    {
        get { return _currentCost; }
    }
    // 木と石は必要ないのでオーバーライド
    public override int ReqTree
    {
        get { return 0; }
    }

    public override int ReqStone
    {
        get { return 0; }
    }
    protected override void OnEnable()
    {
        _currentCost = _wheatCostForSpawn; // 初期設定
        base.OnEnable();
    }
    public override void Upgrade()
    {
        // 基底クラスのUpgrade()を呼ばずに、直接SpawnMinion()を実行
        // これにより _currentLevel の増加処理を回避
        SpawnMinion();
    }
    /// <summary>
    /// 資源あるかチェック
    /// </summary>
    /// <returns></returns>
    private bool CanSpawnMinion()
    {
        return Inventory.Instance.CheckResource(0, 0, _currentCost);
    }
    /// <summary>
    /// ミニオンを生成するメソッド
    /// </summary>
    public void SpawnMinion()
    {
        if (CanSpawnMinion() == false)
        {
            return;
        }
        //麦消費
        Inventory.Instance.UseResourse(0, 0, _currentCost);
        //ミニオンを生成
        GameObject newMinion = Instantiate(_minionPrefab, _spawnPoint.position, Quaternion.identity);
        Debug.Log("ミニオン生成！！");

        //コスト増加
        _currentCost += _plusCost;
        //UI更新
        UIUpdate();
    }
    /// <summary>
    /// レベルアップしないため、ミニオン生成可能かをチェック
    /// 最大レベル制限は無視する
    /// </summary>
    /// <returns></returns>
    public override bool CheckUpgrade()
    {
        // 基底クラスのレベルチェックを無視して、資源チェックのみ実行a
        return CanSpawnMinion();
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