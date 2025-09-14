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

    [SerializeField] private int gridWidth = 10;//�O���b�h�̕�
    [SerializeField] private int gridHeight = 10;//�O���b�h�̍���

    private List<TileData> _tiles = new List<TileData>();//�^�C�����̃��X�g���쐬

    private void Start()
    {
        GanerateGrid();
    }
    void GanerateGrid()
    {
        for (int x = -50; x < gridWidth; x++)//���T�C�Yx
        {
            for (int y = -50; y < gridHeight; y++)//�c�T�C�Yz
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);//xzy�Ȃ̂ł���ŗǂ�
                //�w�b�N�X�̒����̍��W�擾
                Vector3 worldPosition = _hexGrid.GetCellCenterWorld(cellPosition);
                //���̏ꏊ�ɐ���
                Instantiate(_hexLandPrefab, worldPosition, Quaternion.identity, transform);
                //�^�C���̏������X�g�ɂ���Ă����B
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
    /// �w�肵��pos�Ƀe�X�g�I�u�W�F�N�g��z�u����
    /// </summary>
    /// <param name="i"></param>
    public void TestBtn(int i)
    {
        Instantiate(_TestOBJ, _tiles[i].Position, Quaternion.identity, transform);
        Debug.Log(_tiles[i].Position);
    }
}    /// <summary>
     /// ���������^�C���̃f�[�^��ۑ�����N���X
     /// </summary>
public class TileData
{
    public Vector3 Position;
    public bool IsResourceArea;
    public bool IsPlayerBaseArea;
    public bool IsEnemyBaseArea;

}

