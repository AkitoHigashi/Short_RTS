using UnityEngine;

public class UI_Maneger : MonoBehaviour
{
    /// <summary>
    /// UImanegerを参照できるようにシングルトン
    /// </summary>
    public static UI_Maneger Instance
    {
        get;
        private set;
    }

    [SerializeField] private RectTransform _treeUIPos;//目的のUI
    [SerializeField] private RectTransform _stoneUIPos;//石のUI
    [SerializeField] private RectTransform _wheatUIPos;//麦のUI

    private void Awake()
    {
        //初期化
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);//あるなら消す
        }
    }

    /// <summary>
    /// UIのポジションを返すポジション
    /// </summary>
    /// <returns>リソースのUIのポジションを帰す</returns>
    public RectTransform GetTreePos()
    {
        return _treeUIPos;
    }
    public RectTransform GetStonePos()
    {
        return _stoneUIPos;
    }
    public RectTransform GetWheatPos()
    {
        return _wheatUIPos;
    }


}
