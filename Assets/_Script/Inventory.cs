using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _treeCountText;
    [SerializeField] private TextMeshProUGUI _stoneCountText;
    [SerializeField] private TextMeshProUGUI _wheatCountText;

    private int _treeCount = 0;
    private int _stoneCount = 0;
    private int _wheatCount = 0;

    public static Inventory Instance {  get; private set; }

    private void Awake()
    {
        if(Instance == null)
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
    public int Plus_treeCount(int count)
    {
        _treeCount += count;
        Debug.Log("カウントプラス");
        UpdateTreeCount();
        return _treeCount;
    }
    private void UpdateTreeCount()
    {
        _treeCountText.text = _treeCount.ToString();
        _stoneCountText.text = _stoneCount.ToString();
        _wheatCountText.text = _wheatCount.ToString();
    }
    private void Update()
    {
    }
}
