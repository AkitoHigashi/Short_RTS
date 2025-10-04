using UnityEngine;

public class PlayerBase : Base
{
    [Header("拠点のステータス")]
    [SerializeField] private int _currentHP = 100;
    [SerializeField] private int[] _hpPlusLevel = { 0, 100, 150, 250 }; // レベルごとのHP
    [Header("解放オブジェト")]
    [SerializeField] private GameObject[] _objectLevel1;
    [SerializeField] private GameObject[] _objectLevel2;
    [SerializeField] private GameObject[] _objectLevel3;
    [Header("壁を強化時の増えるHP")]
    [SerializeField] private int _increaseHP1 = 100;
    [SerializeField] private int _increaseHP2 = 200;
    [Header("参照")]
    [SerializeField] private PlayerBaseHPUI _hpUI;

    public int MaxHP
    {
        get { return 100 + _hpPlusLevel[1] + _hpPlusLevel[2] + _hpPlusLevel[3] + _increaseHP1 + _increaseHP2; }
    }

    /// <summary>
    /// 現在の拠点のHP
    /// </summary>
    public int CureentHP
    {
        get { return _currentHP; }
    }

    private void Start()
    {
        SetActiveObjcts(_objectLevel1, false);
        SetActiveObjcts(_objectLevel2, false);
        SetActiveObjcts(_objectLevel3, false);

        _hpUI = FindAnyObjectByType<PlayerBaseHPUI>();
    }
    /// <summary>
    /// アップグレードの処理のメソッド
    /// </summary>
    public override void Upgrade()
    {
        base.Upgrade();//ベースを一回呼ぶ
        _currentHP += _hpPlusLevel[Level];//HP足す
        _hpUI.UpdateHPUI();

        if (Level == 1)
        {
            SetActiveObjcts(_objectLevel1, true);
        }
        if (Level == 2)
        {
            SetActiveObjcts(_objectLevel2, true);
        }
        if (Level == 3)
        {
            SetActiveObjcts(_objectLevel3, true);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    public void WallUpgraded(int wallLevel)
    {
        if (wallLevel == 1)
        {
            _currentHP += _increaseHP1;
            _hpUI.UpdateHPUI();
        }
        if (wallLevel == 2)
        {
            _currentHP += _increaseHP2;
            _hpUI.UpdateHPUI();
        }
    }
    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _currentHP = Mathf.Max(0, _currentHP); // HP下限を0に設定
        _hpUI.UpdateHPUI();

        Debug.Log($"拠点がダメージを受けた！ 残りHP: {_currentHP}");

        if (_currentHP <= 0)
        {
            OnDestroyed();
        }
    }
    private void OnDestroyed()
    {
        Debug.Log("敵拠点が破壊された！");
        gameObject.SetActive(false); // とりあえず非表示に
        SceneLoader.Instance.LoadScene("GameOver");
        // ここでゲームオーバーやエフェクトを出してもOK
    }

}
