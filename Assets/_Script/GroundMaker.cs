using System.Collections.Generic;
using UnityEngine;
/// <summary>
///　地面を生成するクラス
/// </summary>
public class GroundMaker : MonoBehaviour
{
    [SerializeField] private Grid _hexGrid;//配置の基準のグリッド
    [SerializeField] private GameObject _hexLandPrefab;//生成する地面
    [SerializeField] private GameObject _TestOBJ;
    [SerializeField] private LayerMask _groundMask;//使わんかも

    [SerializeField] private int gridWidth = 10;//グリッドの幅
    [SerializeField] private int gridHeight = 10;//グリッドの高さ

    private List<TileData> _tiles = new List<TileData>();//タイル情報のリストを作成

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
                Vector3Int cellPosition = new Vector3Int(x, y, 0);//xzyなのでこれで良い
                //ヘックスの中央の座標取得
                Vector3 worldPosition = _hexGrid.GetCellCenterWorld(cellPosition);
                //その場所に生成
                Instantiate(_hexLandPrefab, worldPosition, Quaternion.identity, transform);
                //タイルの情報をリストにいれていく。
                _tiles.Add(new TileData
                {
                    Position = worldPosition,
                    IsResourceArea = false,
                    IsPlayerBaseArea = false,
                    IsEnemyBaseArea = false
                });

            }
        }
    }
    /// <summary>
    /// 指定したposにテストオブジェクトを配置する
    /// </summary>
    /// <param name="i"></param>
    public void TestBtn(int i)
    {
        Instantiate(_TestOBJ, _tiles[i].Position, Quaternion.identity, transform);
        Debug.Log(_tiles[i].Position);
    }
}    /// <summary>
     /// 生成したタイルのデータを保存するクラス
     /// </summary>
public class TileData
{
    public Vector3 Position;
    public bool IsResourceArea;
    public bool IsPlayerBaseArea;
    public bool IsEnemyBaseArea;

}

