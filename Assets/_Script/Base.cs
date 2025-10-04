using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Base : MonoBehaviour
{
    [Header("共通のステータス")]
    [SerializeField] private protected int _currentLevel = 0;
    [SerializeField] private int _maxLevel;
    [SerializeField] private GameObject _trigger;

    [Header("進化の必要資源コスト")]
    [SerializeField] private int[] _treeCost;
    [SerializeField] private int[] _stoneCost;
    [SerializeField] private int[] _wheatCost;

    [Header("UI")]
    [SerializeField] private protected Canvas _canvas;
    [SerializeField] private protected　TextMeshProUGUI _reqText;
    [SerializeField] private protected　Image _treeImage;
    [SerializeField] private protected Image _stoneImage;
    [SerializeField] private protected Image _wheatImage;

    protected virtual void OnEnable()
    {
        UIUpdate();
    }
    protected int Level { get { return _currentLevel; } }
    /// <summary>
    /// 強化に必要な資材:木
    /// </summary>
    public virtual int ReqTree
    {
        get { return _treeCost[_currentLevel]; }

    }
    /// <summary>
    /// 強化に必要な資材:石
    /// </summary>
    public virtual int ReqStone
    {
        get { return _stoneCost[_currentLevel]; }
    }
    /// <summary>
    /// 強化に必要な資材:麦
    /// </summary>
    public virtual int ReqWheat
    {
        get { return _wheatCost[_currentLevel]; }
    }
    /// <summary>
    /// アップグレードできるかチェック
    /// </summary>
    public virtual bool CheckUpgrade()
    {
        if (_currentLevel >= _maxLevel)
        {
            Debug.Log("これ以上進化できません");
            return false;
        }
        return Inventory.Instance.CheckResource(ReqTree, ReqStone, ReqWheat);
    }
    /// <summary>
    /// アップグレードを実行
    /// </summary>
    public virtual void Upgrade()
    {
        if (CheckUpgrade() == false)
        {
            return;
        }
        Inventory.Instance.UseResourse(ReqTree, ReqStone, ReqWheat);
        _currentLevel = _currentLevel + 1;
        UIUpdate();
    }
    /// <summary>
    /// 受け取ったオブジェのセットアクティブを変更するメソッド
    /// </summary>
    /// <param name="objects"></param>
    /// <param name="active"></param>
    public virtual void SetActiveObjcts(GameObject[] objects, bool active)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(active);
        }
    }
   public virtual void UIUpdate()
    {
        // レベルが最大なら Canvas とトリガーを非アクティブ化
        if (_currentLevel >= _maxLevel)
        {
            if (_canvas != null && _trigger != null)
            {
                _canvas.gameObject.SetActive(false);
                _trigger.gameObject.SetActive(false);

            }
            return;
        }
        //一度非アクティブ化
        _treeImage.gameObject.SetActive(false);
        _stoneImage.gameObject.SetActive(false);
        _wheatImage.gameObject.SetActive(false);

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
