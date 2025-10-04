using UnityEngine;

public class Base_Tower : Base
{
    [Header("解放オブジェト")]
    [SerializeField] private GameObject[] _objectLevel1; // 表示する壁オブジェクト配列

    private void Start()
    {
        SetActiveObjcts(_objectLevel1, false);
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
        }
    }

}
