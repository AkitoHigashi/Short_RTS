using System.Collections.Generic;
using UnityEngine;
/// <summary>
///�@�n�ʂ𐶐�����N���X
/// </summary>
public class GroundMaker : MonoBehaviour
{
    [SerializeField] private Grid _hexGrid;//�z�u�̊�̃O���b�h
    [SerializeField] private GameObject _hexLandPrefab;//��������n��
    [SerializeField] private GameObject _TestOBJ;
    [SerializeField] private LayerMask _groundMask;//�g��񂩂�

    [SerializeField] private int gridWidth = 50;//�O���b�h�̕�
    [SerializeField] private int gridHeight = 50;//�O���b�h�̍���

    private List<TileData> _tiles = new List<TileData>();//�^�C�����̃��X�g���쐬
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





    private void Start()
    {
        GanerateGrid();
    }
    /// <summary>
    /// �n�ʂ̃^�C����͈͂��Đ�������֐�
    /// </summary>
    void GanerateGrid()
    {
        for (int x = -50; x < gridWidth; x++)//���T�C�Yx
        {
            for (int y = -50; y < gridHeight; y++)//�c�T�C�Yz
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);//xzy�Ȃ̂ł���ŗǂ��BGrid�̍��W
                //�w�b�N�X�̒����̃��[���h���W�擾
                Vector3 worldPosition = _hexGrid.GetCellCenterWorld(cellPosition);
                //���̏ꏊ�ɐ���
                Instantiate(_hexLandPrefab, worldPosition, Quaternion.identity, transform);
                //�^�C���̏������X�g�ɂ���Ă����B
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

}

