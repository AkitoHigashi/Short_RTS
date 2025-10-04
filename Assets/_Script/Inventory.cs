using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _treeCountText;
    [SerializeField] private TextMeshProUGUI _stoneCountText;
    [SerializeField] private TextMeshProUGUI _wheatCountText;

    private int _treeCount = 0;
    private int _stoneCount = 0;
    private int _wheatCount = 0;

    public static Inventory Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _treeCountText.text = _treeCount.ToString();
        _stoneCountText.text = _stoneCount.ToString();
        _wheatCountText.text = _wheatCount.ToString();
    }
    /// <summary>
    /// 資源を消費するメソッド
    /// </summary>
    /// <param name="tree"></param>
    /// <param name="stone"></param>
    /// <param name="wheat"></param>
    /// <returns></returns>
    public bool UseResourse(int tree, int stone, int wheat)
    {
        if (_treeCount >= tree && _stoneCount >= stone && _wheatCount >= wheat)//資材を必要数持ってるか
        {
            _treeCount -= tree;
            _stoneCount -= stone;
            _wheatCount -= wheat;
            UpdateTextCount();
            return true;
        }
        return false;
    }
    /// <summary>
    /// 必要資源を持っているか確認するメソッド
    /// </summary>
    /// <param name="tree"></param>
    /// <param name="stone"></param>
    /// <param name="wheat"></param>
    /// <returns></returns>
    public bool CheckResource(int tree,int stone ,int wheat)
    {
        return _treeCount >= tree && _stoneCount >= stone && _wheatCount >= wheat;
    }
    public int Plus_treeCount(int count)
    {
        _treeCount += count;
        Debug.Log("カウントプラス");
        UpdateTextCount();
        return _treeCount;
    }
    /// <summary>
    /// 資源加算
    /// </summary>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    public void AddResource(ResourceType type , int amount)
    {
        switch (type)
        {
            case ResourceType.Tree: _treeCount += amount; break;
            case ResourceType.Stone: _stoneCount += amount; break;
            case ResourceType.Wheat: _wheatCount += amount; break;
        }
        UpdateTextCount();
        Debug.Log($"{type} +{amount}");
    }
    /// <summary>
    /// UI更新
    /// </summary>
    private void UpdateTextCount()
    {
        _treeCountText.text = _treeCount.ToString();
        _stoneCountText.text = _stoneCount.ToString();
        _wheatCountText.text = _wheatCount.ToString();
    }
}
public enum ResourceType
{
    Tree,
    Stone,
    Wheat,
}
