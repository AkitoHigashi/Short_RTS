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

    [SerializeField] private int gridWidth = 50;//グリッドの幅
    [SerializeField] private int gridHeight = 50;//グリッドの高さ

    private List<TileData> _tiles = new List<TileData>();//タイル情報のリストを作成
    /// <summary>
    /// (偶数行）offset座標で周辺タイルを見つける為の方向の配列 Gridの座標と対応　 Odd-r 
    /// </summary>
    private Vector3Int[] _evenRowDirection = new Vector3Int[]
    {
        new Vector3Int(1,0,0),//右
        new Vector3Int(0,+1,0),//右上
        new Vector3Int(0,-1,0),//右下
        new Vector3Int(-1,0,0),//左
        new Vector3Int(-1,+1,0),//左上
        new Vector3Int(-1,-1,0),//左下
    };
    /// <summary>
    /// (奇数行)offset座標で周辺タイルを見つける為の方向の配列 Gridの座標と対応 Odd-r 
    /// </summary>
    private Vector3Int[] _oddRowDirection = new Vector3Int[]
    {
        new Vector3Int(1,0,0),//右
        new Vector3Int(1,+1,0),//右上
        new Vector3Int(1,-1,0),//右下
        new Vector3Int(-1,0,0),//左
        new Vector3Int(0,-1,0),//左上
        new Vector3Int(0,+1,0),//左下
    };





    private void Start()
    {
        GanerateGrid();
    }
    /// <summary>
    /// 地面のタイルを範囲して生成する関数
    /// </summary>
    void GanerateGrid()
    {
        for (int x = -50; x < gridWidth; x++)//横サイズx
        {
            for (int y = -50; y < gridHeight; y++)//縦サイズz
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);//xzyなのでこれで良い。Gridの座標
                //ヘックスの中央のワールド座標取得
                Vector3 worldPosition = _hexGrid.GetCellCenterWorld(cellPosition);
                //その場所に生成
                Instantiate(_hexLandPrefab, worldPosition, Quaternion.identity, transform);
                //タイルの情報をリストにいれていく。
                _tiles.Add(new TileData
                {
                    GridPosition = cellPosition,
                    Position = worldPosition,
                    IsResourceArea = false,
                    IsPlayerBaseArea = false,
                    IsEnemyBaseArea = false
                });

            }
        }
    }
    /// <summary>
    /// １つタイルの周辺タイル6タイルを取得するメソッド
    /// </summary>
    List<Vector3Int> GetTileNeighbors(Vector3Int center)//中央となるタイルGrid座標を取得
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();//隣接マスを入れるリストを作成
        Vector3Int[] useDirections;//偶数か奇数かの使用する配列を入れる
        if (center.y % 2 == 0)
        {
            useDirections = _evenRowDirection;
        }
        else
        {
            useDirections = _oddRowDirection;
        }

        foreach (var around in useDirections)
        {
            Vector3Int neighborPos = center + around;//中央マスと隣接までの差分を足す

            // _tiles の中に存在するか確認
            bool found = false;
            for (int i = 0; i < _tiles.Count; i++)
            {
                if (_tiles[i].GridPosition == neighborPos)
                {
                    found = true;
                    break; // 見つかったらループ終了
                }
            }
            if (found)
            {
                neighbors.Add(neighborPos); // 存在するタイルだけリストに追加
            }
        }
        return neighbors;
    }
    /// <summary>
    /// 指定したposにテストオブジェクトを配置する
    /// </summary>
    /// <param name="i"></param>
    public void TestBtn(int i)
    {
        Instantiate(_TestOBJ, _tiles[i].Position, Quaternion.identity, transform);
        Debug.Log($"{_tiles[i].Position}:{_tiles[i].GridPosition}");

        Vector3Int centerTile = _tiles[i].GridPosition;
        List<Vector3Int> neighbors = GetTileNeighbors(centerTile);

        Debug.Log($"タイル{centerTile}の隣接タイル数: {neighbors.Count}");

        // 隣接タイルにテストオブジェクトを配置
        foreach (var neighbor in neighbors)
        {
            Vector3 worldPos = _hexGrid.GetCellCenterWorld(neighbor);
            Instantiate(_TestOBJ, worldPos, Quaternion.identity, transform);
        }
    }

}    /// <summary>
     /// 生成したタイルのデータを保存するクラス
     /// </summary>
public class TileData
{
    public Vector3Int GridPosition;
    public Vector3 Position;
    public bool IsResourceArea;
    public bool IsPlayerBaseArea;
    public bool IsEnemyBaseArea;

}

