using UnityEngine;

public class Base_Tower : Base
{
    [Header("����I�u�W�F�g")]
    [SerializeField] private GameObject[] _objectLevel1; // �\������ǃI�u�W�F�N�g�z��

    private void Start()
    {
        SetActiveObjcts(_objectLevel1, false);
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
        }
    }

}
