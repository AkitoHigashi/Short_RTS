using UnityEngine;

public class Base_Wall : Base
{
    [Header("解放オブジェト")]
    [SerializeField] private GameObject[] _objectLevel1; // 表示する壁オブジェクト配列
    [SerializeField] private GameObject[] _objectLevel2; // 表示する壁オブジェクト配列
    [Header("参照")]
    [SerializeField] private PlayerBase _playerBase;

    private void Start()
    {
        SetActiveObjcts(_objectLevel1, false);
        SetActiveObjcts(_objectLevel2, false);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    /// <summary>
    /// アップグレードの処理のメソッド
    /// </summary>
    public override void Upgrade()
    {
        base.Upgrade();//ベースを一回呼ぶ
        if (Level == 1)
        {
            SetActiveObjcts(_objectLevel1, true);
            Signal(Level);
        }
        if (Level == 2)
        {
            SetActiveObjcts(_objectLevel1, false);//消す
            SetActiveObjcts(_objectLevel2, true);
            Signal(Level);
        }
    }
    /// <summary>
    /// 強化時にplayerbaseに送る信号
    /// </summary>
    /// <param name="wallLevel"></param>
    private void Signal(int wallLevel)
    {
        if (_playerBase != null)
        {
            _playerBase.WallUpgraded(wallLevel);
            Debug.Log($"壁レベル{wallLevel}アップグレード信号を送信");
        }
        else
        {
            Debug.LogError("PlayerBaseが設定されていません");
        }
    }

}
