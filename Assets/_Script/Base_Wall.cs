using UnityEngine;

public class Base_Wall : Base
{
    [Header("����I�u�W�F�g")]
    [SerializeField] private GameObject[] _objectLevel1; // �\������ǃI�u�W�F�N�g�z��
    [SerializeField] private GameObject[] _objectLevel2; // �\������ǃI�u�W�F�N�g�z��
    [Header("�Q��")]
    [SerializeField] private PlayerBase _playerBase;

    private void Start()
    {
        SetActiveObjcts(_objectLevel1, false);
        SetActiveObjcts(_objectLevel2, false);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    /// <summary>
    /// �A�b�v�O���[�h�̏����̃��\�b�h
    /// </summary>
    public override void Upgrade()
    {
        base.Upgrade();//�x�[�X�����Ă�
        if (Level == 1)
        {
            SetActiveObjcts(_objectLevel1, true);
            Signal(Level);
        }
        if (Level == 2)
        {
            SetActiveObjcts(_objectLevel1, false);//����
            SetActiveObjcts(_objectLevel2, true);
            Signal(Level);
        }
    }
    /// <summary>
    /// ��������playerbase�ɑ���M��
    /// </summary>
    /// <param name="wallLevel"></param>
    private void Signal(int wallLevel)
    {
        if (_playerBase != null)
        {
            _playerBase.WallUpgraded(wallLevel);
            Debug.Log($"�ǃ��x��{wallLevel}�A�b�v�O���[�h�M���𑗐M");
        }
        else
        {
            Debug.LogError("PlayerBase���ݒ肳��Ă��܂���");
        }
    }

}
