using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using System.Linq;
/// <summary>
///　地面を生成するクラス
/// </summary>
public class GroundMaker : MonoBehaviour
{
    [SerializeField] private Grid _hexGrid;//配置の基準のグリッド
    [SerializeField] private Transform _player; // プレイヤーのTransform
    [SerializeField] private Transform _groundParent;//地面の親オブジェ
    [SerializeField] private Transform _WallParent;//壁の親オブジェ
    [SerializeField] private Transform _ResourceAreaParent;//資源エリアの親オブジェ
    [Header("生成する系")]
    [SerializeField] private GameObject _hexLandPrefab;//生成する地面
    [SerializeField] private GameObject _outsideWall;//外を囲う岩（壁）
    [SerializeField] private GameObject _playerBasePrefab;//プレイヤーの拠点
    [SerializeField] private GameObject _enemyBasePrefab;//プレイヤーの拠点
    [SerializeField] private GameObject _enemyBasePrefab2;//プレイヤーの拠点
    [SerializeField] private GameObject _TestOBJ;
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [Header("資源エリア生成")]
    [SerializeField] private GameObject[] _resourcePrefabs;
    [SerializeField, Header("エリアの出現の数")] private int _resourceCount = 50;
    [Header("タイル系")]
    [SerializeField] private int _gridWidth = 50;//グリッドの幅
    [SerializeField] private int _gridHeight = 50;//グリッドの高さ
    [Header("エリア検出設定")]
    [SerializeField] private LayerMask _groundMask;//地面のレイヤー
    [SerializeField] private float _enemyAreaRadius = 5f;//敵のエリアの範囲
    [SerializeField] private float _playerAreaRadius = 5f;//自分のエリアの範囲

    private List<TileData> _tiles = new List<TileData>();//タイル情報のリストを作成
    private List<TileData> _outSideTile = new List<TileData>();//外周タイルのリスト
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

    private void Awake()
    {
        GanerateGrid();
        SpawnPlayerBase();
        SpawnEnenyBase();
        GetResourceAreaTile();
        BakeNavMesh();
    }
    /// <summary>
    /// 地面のタイルを範囲して生成する関数
    /// </summary>
    void GanerateGrid()
    {
        for (int x = -50; x < _gridWidth; x++)//横サイズx
        {
            for (int y = -50; y < _gridHeight; y++)//縦サイズz
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);//xzyなのでこれで良い。Gridの座標
                //ヘックスの中央のワールド座標取得
                Vector3 worldPosition = _hexGrid.GetCellCenterWorld(cellPosition);
                //その場所に生成
                Instantiate(_hexLandPrefab, worldPosition, Quaternion.identity, _groundParent);
                //タイルの情報をリストにいれていく。
                _tiles.Add(new TileData
                {
                    GridPosition = cellPosition,
                    Position = worldPosition,
                    IsResourceArea = false,
                    IsPlayerBaseArea = false,
                    IsEnemyBaseArea = false
                });
                //外側に壁生成
                if (x == -50 || x == _gridWidth - 1 || y == -50 || y == _gridHeight - 1)
                {
                    Instantiate(_outsideWall, worldPosition, Quaternion.identity, _WallParent);
                    _tiles[_tiles.Count - 1].IsOutsideWall = true;//タイルデータ更新
                    _outSideTile.Add(_tiles[_tiles.Count - 1]);//リストに入れる
                }
            }
        }
    }
    /// <summary>
    /// 地面をbakeするメソッド
    /// </summary>
    private void BakeNavMesh()
    {
        if (_navMeshSurface != null)
        {
            _navMeshSurface.BuildNavMesh();
            Debug.Log("Bakeした！");
        }
        else
        {
            Debug.Log("_navMeshSurfaceにアタッチしてねです");
        }
    }
    /// <summary>
    /// 資源エリアになりうるタイルを取得
    /// </summary>
    private void GetResourceAreaTile()
    {
        for (int i = 0; i < _resourceCount; i++)
        {
            int index = Random.Range(0, _tiles.Count);
            var tile = _tiles[index];

            if (tile.IsPlayerBaseArea || tile.IsEnemyBaseArea || tile.IsOutsideWall || tile.IsResourceArea)
            {
                continue;//次の要素へ
            }

            int prefabIndex = Random.Range(0, _resourcePrefabs.Length);//プレハブをランダムで選ぶ
            var prefab = _resourcePrefabs[prefabIndex];
            Instantiate(prefab, tile.Position, Quaternion.identity, _ResourceAreaParent);//プレハブを生成
            tile.IsResourceArea = true;

            Vector3Int centerTile = tile.GridPosition;
            List<Vector3Int> neighbors = GetTileNeighbors(centerTile);
            // 隣接タイルにオブジェクトを配置
            foreach (var neighbor in neighbors)
            {
                var neighborTile = _tiles.FirstOrDefault(t => t.GridPosition == neighbor);//条件式と合うものが見つかったらその要素を返す
                if (neighborTile != null && !neighborTile.IsPlayerBaseArea && !neighborTile.IsEnemyBaseArea && !neighborTile.IsOutsideWall && !neighborTile.IsResourceArea)
                {
                    Vector3 worldPos = _hexGrid.GetCellCenterWorld(neighbor);
                    Instantiate(prefab, worldPos, Quaternion.identity, _ResourceAreaParent);
                    neighborTile.IsResourceArea = true; // ← ここ大事
                }
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
    /// プレイヤーbaseをつくる
    /// </summary>
    public void SpawnPlayerBase()
    {
        Vector3Int cellPos = new Vector3Int(0, 0, 0);
        //ワールド座標に変換
        Vector3 worldPos = _hexGrid.GetCellCenterWorld(cellPos);

        Instantiate(_playerBasePrefab, worldPos, Quaternion.identity);
        Debug.Log($"拠点作成:{worldPos}");
        GetPlayerArea(cellPos);

    }
    /// <summary>
    /// 指定座標の中からランダムに2か所だけ敵拠点を配置する
    /// </summary>
    private void SpawnEnenyBase()
    {
        Vector3Int[] spawnCellpos = new Vector3Int[]
        {
          new Vector3Int(43,37, 0),//右上0
          new Vector3Int(43, -45, 0),//右下1
          new Vector3Int(-40, 44, 0),//左上2
          new Vector3Int(-45, -39, 0),//左下3
         };
        int firstIndex = Random.Range(0, 2);//右だけ判定
        int secondIndex;
        if (firstIndex == 0)//右上の時
        {
            secondIndex = 3;//左下
        }
        else//右下の時
        {
            secondIndex = 2;//左上
        }
        Vector3 worldPosition = _hexGrid.GetCellCenterWorld(spawnCellpos[firstIndex]);
        Vector3 worldPosition2 = _hexGrid.GetCellCenterWorld(spawnCellpos[secondIndex]);
        Instantiate(_enemyBasePrefab, worldPosition, Quaternion.identity);
        Instantiate(_enemyBasePrefab2, worldPosition2, Quaternion.identity);
        GetEnemyArea(spawnCellpos[firstIndex]);
        GetEnemyArea(spawnCellpos[secondIndex]);
    }

    /// <summary>
    /// エネミーエリアのタイルを取得する
    /// </summary>
    /// グリッド座標を受け取る
    private void GetEnemyArea(Vector3Int gridCenterPos)
    {
        var worldPos = _hexGrid.GetCellCenterWorld(gridCenterPos);//グリッド座標を変換
        var tiles = Physics.OverlapSphere(worldPos, _enemyAreaRadius, _groundMask);
        foreach (var tile in tiles)
        {
            Vector3Int gridPos = _hexGrid.WorldToCell(tile.transform.position);//戻す
            var tileData = _tiles.FirstOrDefault(t => t.GridPosition == gridPos);//リストから最初に見つかった要素を返すメソッド　タイルデータのグリッドポジションに代入
            if (tileData != null)
            {
                tileData.IsEnemyBaseArea = true;
            }
        }
    }
    /// <summary>
    /// プレイヤーのエリアのタイルを取得する
    /// </summary>
    /// グリッド座標を受け取る
    private void GetPlayerArea(Vector3Int gridCenterPos)
    {
        var worldPos = _hexGrid.GetCellCenterWorld(gridCenterPos);//グリッド座標を変換
        var tiles = Physics.OverlapSphere(worldPos, _playerAreaRadius, _groundMask);
        foreach (var tile in tiles)
        {
            Vector3Int gridPos = _hexGrid.WorldToCell(tile.transform.position);//戻す
            var tileData = _tiles.FirstOrDefault(t => t.GridPosition == gridPos);//リストから最初に見つかった要素を返すメソッド　タイルデータのグリッドポジションに代入
            if (tileData != null)
            {
                tileData.IsPlayerBaseArea = true;
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
    /// <summary>
    /// 現在のGrid座標を求める
    /// </summary>
    public void CurrentGridPos()
    {
        Debug.Log($"{_hexGrid.WorldToCell(_player.position)}");
        Debug.Log($"{_hexGrid.GetCellCenterWorld(_hexGrid.WorldToCell(_player.position))}");
    }
    private void OnDrawGizmosSelected()
    {
        if (_hexGrid == null) return;

        // プレイヤーエリア半径
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_hexGrid.GetCellCenterWorld(new Vector3Int(0, 0, 0)), _playerAreaRadius);

        // 敵エリア半径
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hexGrid.GetCellCenterWorld(new Vector3Int(43, 37, 0)), _enemyAreaRadius);
        // 敵エリア半径
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hexGrid.GetCellCenterWorld(new Vector3Int(43, -45, 0)), _enemyAreaRadius);
        // 敵エリア半径
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hexGrid.GetCellCenterWorld(new Vector3Int(-40, 44, 0)), _enemyAreaRadius);
        // 敵エリア半径
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hexGrid.GetCellCenterWorld(new Vector3Int(-45, -39, 0)), _enemyAreaRadius);
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
    public bool IsOutsideWall;
}

