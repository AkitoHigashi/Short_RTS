using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using System.Linq;
/// <summary>
///�@�n�ʂ𐶐�����N���X
/// </summary>
public class GroundMaker : MonoBehaviour
{
    [SerializeField] private Grid _hexGrid;//�z�u�̊�̃O���b�h
    [SerializeField] private Transform _player; // �v���C���[��Transform
    [SerializeField] private Transform _groundParent;//�n�ʂ̐e�I�u�W�F
    [SerializeField] private Transform _WallParent;//�ǂ̐e�I�u�W�F
    [SerializeField] private Transform _ResourceAreaParent;//�����G���A�̐e�I�u�W�F
    [Header("��������n")]
    [SerializeField] private GameObject _hexLandPrefab;//��������n��
    [SerializeField] private GameObject _outsideWall;//�O���͂���i�ǁj
    [SerializeField] private GameObject _playerBasePrefab;//�v���C���[�̋��_
    [SerializeField] private GameObject _enemyBasePrefab;//�v���C���[�̋��_
    [SerializeField] private GameObject _enemyBasePrefab2;//�v���C���[�̋��_
    [SerializeField] private GameObject _TestOBJ;
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [Header("�����G���A����")]
    [SerializeField] private GameObject[] _resourcePrefabs;
    [SerializeField, Header("�G���A�̏o���̐�")] private int _resourceCount = 50;
    [Header("�^�C���n")]
    [SerializeField] private int _gridWidth = 50;//�O���b�h�̕�
    [SerializeField] private int _gridHeight = 50;//�O���b�h�̍���
    [Header("�G���A���o�ݒ�")]
    [SerializeField] private LayerMask _groundMask;//�n�ʂ̃��C���[
    [SerializeField] private float _enemyAreaRadius = 5f;//�G�̃G���A�͈̔�
    [SerializeField] private float _playerAreaRadius = 5f;//�����̃G���A�͈̔�

    private List<TileData> _tiles = new List<TileData>();//�^�C�����̃��X�g���쐬
    private List<TileData> _outSideTile = new List<TileData>();//�O���^�C���̃��X�g
    /// <summary>
    /// (�����s�joffset���W�Ŏ��Ӄ^�C����������ׂ̕����̔z�� Grid�̍��W�ƑΉ��@ Odd-r 
    /// </summary>
    private Vector3Int[] _evenRowDirection = new Vector3Int[]
    {
        new Vector3Int(1,0,0),//�E
        new Vector3Int(0,+1,0),//�E��
        new Vector3Int(0,-1,0),//�E��
        new Vector3Int(-1,0,0),//��
        new Vector3Int(-1,+1,0),//����
        new Vector3Int(-1,-1,0),//����
    };
    /// <summary>
    /// (��s)offset���W�Ŏ��Ӄ^�C����������ׂ̕����̔z�� Grid�̍��W�ƑΉ� Odd-r 
    /// </summary>
    private Vector3Int[] _oddRowDirection = new Vector3Int[]
    {
        new Vector3Int(1,0,0),//�E
        new Vector3Int(1,+1,0),//�E��
        new Vector3Int(1,-1,0),//�E��
        new Vector3Int(-1,0,0),//��
        new Vector3Int(0,-1,0),//����
        new Vector3Int(0,+1,0),//����
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
    /// �n�ʂ̃^�C����͈͂��Đ�������֐�
    /// </summary>
    void GanerateGrid()
    {
        for (int x = -50; x < _gridWidth; x++)//���T�C�Yx
        {
            for (int y = -50; y < _gridHeight; y++)//�c�T�C�Yz
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);//xzy�Ȃ̂ł���ŗǂ��BGrid�̍��W
                //�w�b�N�X�̒����̃��[���h���W�擾
                Vector3 worldPosition = _hexGrid.GetCellCenterWorld(cellPosition);
                //���̏ꏊ�ɐ���
                Instantiate(_hexLandPrefab, worldPosition, Quaternion.identity, _groundParent);
                //�^�C���̏������X�g�ɂ���Ă����B
                _tiles.Add(new TileData
                {
                    GridPosition = cellPosition,
                    Position = worldPosition,
                    IsResourceArea = false,
                    IsPlayerBaseArea = false,
                    IsEnemyBaseArea = false
                });
                //�O���ɕǐ���
                if (x == -50 || x == _gridWidth - 1 || y == -50 || y == _gridHeight - 1)
                {
                    Instantiate(_outsideWall, worldPosition, Quaternion.identity, _WallParent);
                    _tiles[_tiles.Count - 1].IsOutsideWall = true;//�^�C���f�[�^�X�V
                    _outSideTile.Add(_tiles[_tiles.Count - 1]);//���X�g�ɓ����
                }
            }
        }
    }
    /// <summary>
    /// �n�ʂ�bake���郁�\�b�h
    /// </summary>
    private void BakeNavMesh()
    {
        if (_navMeshSurface != null)
        {
            _navMeshSurface.BuildNavMesh();
            Debug.Log("Bake�����I");
        }
        else
        {
            Debug.Log("_navMeshSurface�ɃA�^�b�`���Ă˂ł�");
        }
    }
    /// <summary>
    /// �����G���A�ɂȂ肤��^�C�����擾
    /// </summary>
    private void GetResourceAreaTile()
    {
        for (int i = 0; i < _resourceCount; i++)
        {
            int index = Random.Range(0, _tiles.Count);
            var tile = _tiles[index];

            if (tile.IsPlayerBaseArea || tile.IsEnemyBaseArea || tile.IsOutsideWall || tile.IsResourceArea)
            {
                continue;//���̗v�f��
            }

            int prefabIndex = Random.Range(0, _resourcePrefabs.Length);//�v���n�u�������_���őI��
            var prefab = _resourcePrefabs[prefabIndex];
            Instantiate(prefab, tile.Position, Quaternion.identity, _ResourceAreaParent);//�v���n�u�𐶐�
            tile.IsResourceArea = true;

            Vector3Int centerTile = tile.GridPosition;
            List<Vector3Int> neighbors = GetTileNeighbors(centerTile);
            // �אڃ^�C���ɃI�u�W�F�N�g��z�u
            foreach (var neighbor in neighbors)
            {
                var neighborTile = _tiles.FirstOrDefault(t => t.GridPosition == neighbor);//�������ƍ������̂����������炻�̗v�f��Ԃ�
                if (neighborTile != null && !neighborTile.IsPlayerBaseArea && !neighborTile.IsEnemyBaseArea && !neighborTile.IsOutsideWall && !neighborTile.IsResourceArea)
                {
                    Vector3 worldPos = _hexGrid.GetCellCenterWorld(neighbor);
                    Instantiate(prefab, worldPos, Quaternion.identity, _ResourceAreaParent);
                    neighborTile.IsResourceArea = true; // �� �����厖
                }
            }

        }
    }
    /// <summary>
    /// �P�^�C���̎��Ӄ^�C��6�^�C�����擾���郁�\�b�h
    /// </summary>
    List<Vector3Int> GetTileNeighbors(Vector3Int center)//�����ƂȂ�^�C��Grid���W���擾
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();//�אڃ}�X�����郊�X�g���쐬
        Vector3Int[] useDirections;//����������̎g�p����z�������
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
            Vector3Int neighborPos = center + around;//�����}�X�Ɨאڂ܂ł̍����𑫂�

            // _tiles �̒��ɑ��݂��邩�m�F
            bool found = false;
            for (int i = 0; i < _tiles.Count; i++)
            {
                if (_tiles[i].GridPosition == neighborPos)
                {
                    found = true;
                    break; // ���������烋�[�v�I��
                }
            }
            if (found)
            {
                neighbors.Add(neighborPos); // ���݂���^�C���������X�g�ɒǉ�
            }
        }
        return neighbors;
    }
    /// <summary>
    /// �v���C���[base������
    /// </summary>
    public void SpawnPlayerBase()
    {
        Vector3Int cellPos = new Vector3Int(0, 0, 0);
        //���[���h���W�ɕϊ�
        Vector3 worldPos = _hexGrid.GetCellCenterWorld(cellPos);

        Instantiate(_playerBasePrefab, worldPos, Quaternion.identity);
        Debug.Log($"���_�쐬:{worldPos}");
        GetPlayerArea(cellPos);

    }
    /// <summary>
    /// �w����W�̒����烉���_����2���������G���_��z�u����
    /// </summary>
    private void SpawnEnenyBase()
    {
        Vector3Int[] spawnCellpos = new Vector3Int[]
        {
          new Vector3Int(43,37, 0),//�E��0
          new Vector3Int(43, -45, 0),//�E��1
          new Vector3Int(-40, 44, 0),//����2
          new Vector3Int(-45, -39, 0),//����3
         };
        int firstIndex = Random.Range(0, 2);//�E��������
        int secondIndex;
        if (firstIndex == 0)//�E��̎�
        {
            secondIndex = 3;//����
        }
        else//�E���̎�
        {
            secondIndex = 2;//����
        }
        Vector3 worldPosition = _hexGrid.GetCellCenterWorld(spawnCellpos[firstIndex]);
        Vector3 worldPosition2 = _hexGrid.GetCellCenterWorld(spawnCellpos[secondIndex]);
        Instantiate(_enemyBasePrefab, worldPosition, Quaternion.identity);
        Instantiate(_enemyBasePrefab2, worldPosition2, Quaternion.identity);
        GetEnemyArea(spawnCellpos[firstIndex]);
        GetEnemyArea(spawnCellpos[secondIndex]);
    }

    /// <summary>
    /// �G�l�~�[�G���A�̃^�C�����擾����
    /// </summary>
    /// �O���b�h���W���󂯎��
    private void GetEnemyArea(Vector3Int gridCenterPos)
    {
        var worldPos = _hexGrid.GetCellCenterWorld(gridCenterPos);//�O���b�h���W��ϊ�
        var tiles = Physics.OverlapSphere(worldPos, _enemyAreaRadius, _groundMask);
        foreach (var tile in tiles)
        {
            Vector3Int gridPos = _hexGrid.WorldToCell(tile.transform.position);//�߂�
            var tileData = _tiles.FirstOrDefault(t => t.GridPosition == gridPos);//���X�g����ŏ��Ɍ��������v�f��Ԃ����\�b�h�@�^�C���f�[�^�̃O���b�h�|�W�V�����ɑ��
            if (tileData != null)
            {
                tileData.IsEnemyBaseArea = true;
            }
        }
    }
    /// <summary>
    /// �v���C���[�̃G���A�̃^�C�����擾����
    /// </summary>
    /// �O���b�h���W���󂯎��
    private void GetPlayerArea(Vector3Int gridCenterPos)
    {
        var worldPos = _hexGrid.GetCellCenterWorld(gridCenterPos);//�O���b�h���W��ϊ�
        var tiles = Physics.OverlapSphere(worldPos, _playerAreaRadius, _groundMask);
        foreach (var tile in tiles)
        {
            Vector3Int gridPos = _hexGrid.WorldToCell(tile.transform.position);//�߂�
            var tileData = _tiles.FirstOrDefault(t => t.GridPosition == gridPos);//���X�g����ŏ��Ɍ��������v�f��Ԃ����\�b�h�@�^�C���f�[�^�̃O���b�h�|�W�V�����ɑ��
            if (tileData != null)
            {
                tileData.IsPlayerBaseArea = true;
            }
        }
    }
    /// <summary>
    /// �w�肵��pos�Ƀe�X�g�I�u�W�F�N�g��z�u����
    /// </summary>
    /// <param name="i"></param>
    public void TestBtn(int i)
    {
        Instantiate(_TestOBJ, _tiles[i].Position, Quaternion.identity, transform);
        Debug.Log($"{_tiles[i].Position}:{_tiles[i].GridPosition}");

        Vector3Int centerTile = _tiles[i].GridPosition;
        List<Vector3Int> neighbors = GetTileNeighbors(centerTile);

        Debug.Log($"�^�C��{centerTile}�̗אڃ^�C����: {neighbors.Count}");

        // �אڃ^�C���Ƀe�X�g�I�u�W�F�N�g��z�u
        foreach (var neighbor in neighbors)
        {
            Vector3 worldPos = _hexGrid.GetCellCenterWorld(neighbor);
            Instantiate(_TestOBJ, worldPos, Quaternion.identity, transform);
        }
    }
    /// <summary>
    /// ���݂�Grid���W�����߂�
    /// </summary>
    public void CurrentGridPos()
    {
        Debug.Log($"{_hexGrid.WorldToCell(_player.position)}");
        Debug.Log($"{_hexGrid.GetCellCenterWorld(_hexGrid.WorldToCell(_player.position))}");
    }
    private void OnDrawGizmosSelected()
    {
        if (_hexGrid == null) return;

        // �v���C���[�G���A���a
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_hexGrid.GetCellCenterWorld(new Vector3Int(0, 0, 0)), _playerAreaRadius);

        // �G�G���A���a
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hexGrid.GetCellCenterWorld(new Vector3Int(43, 37, 0)), _enemyAreaRadius);
        // �G�G���A���a
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hexGrid.GetCellCenterWorld(new Vector3Int(43, -45, 0)), _enemyAreaRadius);
        // �G�G���A���a
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hexGrid.GetCellCenterWorld(new Vector3Int(-40, 44, 0)), _enemyAreaRadius);
        // �G�G���A���a
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hexGrid.GetCellCenterWorld(new Vector3Int(-45, -39, 0)), _enemyAreaRadius);
    }
}    /// <summary>
     /// ���������^�C���̃f�[�^��ۑ�����N���X
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

