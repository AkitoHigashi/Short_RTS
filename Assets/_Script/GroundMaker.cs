using UnityEngine;

public class GroundMaker : MonoBehaviour
{
    [SerializeField] private Grid _hexGrid;
    [SerializeField] private GameObject _hexLandPrefab;
    [SerializeField] private LayerMask _groundMask;

    [SerializeField] private int gridWidth = 10;//グリッドの幅
    [SerializeField] private int gridHeight = 10;//グリッドの高さ

    private void Start()
    {
        GanerateGrid();
    }
    void GanerateGrid()
    {
        for (int x = -50; x < gridWidth; x++)//横サイズx
        {
            for (int y = -50; y < gridHeight; y++)//縦サイズz
            {
                Vector3Int cellPosition = new Vector3Int(x,y,0);//xzyなのでこれで良い
                //ヘックスの中央の座標取得
                Vector3 worldPosition = _hexGrid.GetCellCenterWorld(cellPosition);
                //その場所に生成
                Instantiate(_hexLandPrefab,worldPosition,Quaternion.identity,transform);

            }
        }
    }
}
