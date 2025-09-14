using UnityEngine;

public class GroundMaker : MonoBehaviour
{
    [SerializeField] private Grid _hexGrid;
    [SerializeField] private GameObject _hexLandPrefab;
    [SerializeField] private LayerMask _groundMask;

    [SerializeField] private int gridWidth = 10;//�O���b�h�̕�
    [SerializeField] private int gridHeight = 10;//�O���b�h�̍���

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
                Vector3Int cellPosition = new Vector3Int(x,y,0);//xzy�Ȃ̂ł���ŗǂ�
                //�w�b�N�X�̒����̍��W�擾
                Vector3 worldPosition = _hexGrid.GetCellCenterWorld(cellPosition);
                //���̏ꏊ�ɐ���
                Instantiate(_hexLandPrefab,worldPosition,Quaternion.identity,transform);

            }
        }
    }
}
