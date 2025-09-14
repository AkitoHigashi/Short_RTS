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


}
