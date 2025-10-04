using UnityEngine;

public class PlayerBase : Base
{
    [Header("���_�̃X�e�[�^�X")]
    [SerializeField] private int _currentHP = 100;
    [SerializeField] private int[] _hpPlusLevel = { 0, 100, 150, 250 }; // ���x�����Ƃ�HP
    [Header("����I�u�W�F�g")]
    [SerializeField] private GameObject[] _objectLevel1;
    [SerializeField] private GameObject[] _objectLevel2;
    [SerializeField] private GameObject[] _objectLevel3;
    [Header("�ǂ��������̑�����HP")]
    [SerializeField] private int _increaseHP1 = 100;
    [SerializeField] private int _increaseHP2 = 200;
    [Header("�Q��")]
    [SerializeField] private PlayerBaseHPUI _hpUI;

    public int MaxHP
    {
        get { return 100 + _hpPlusLevel[1] + _hpPlusLevel[2] + _hpPlusLevel[3] + _increaseHP1 + _increaseHP2; }
    }

    /// <summary>
    /// ���݂̋��_��HP
    /// </summary>
    public int CureentHP
    {
        get { return _currentHP; }
    }

    private void Start()
    {
        SetActiveObjcts(_objectLevel1, false);
        SetActiveObjcts(_objectLevel2, false);
        SetActiveObjcts(_objectLevel3, false);

        _hpUI = FindAnyObjectByType<PlayerBaseHPUI>();
    }
    /// <summary>
    /// �A�b�v�O���[�h�̏����̃��\�b�h
    /// </summary>
    public override void Upgrade()
    {
        base.Upgrade();//�x�[�X�����Ă�
        _currentHP += _hpPlusLevel[Level];//HP����
        _hpUI.UpdateHPUI();

        if (Level == 1)
        {
            SetActiveObjcts(_objectLevel1, true);
        }
        if (Level == 2)
        {
            SetActiveObjcts(_objectLevel2, true);
        }
        if (Level == 3)
        {
            SetActiveObjcts(_objectLevel3, true);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    public void WallUpgraded(int wallLevel)
    {
        if (wallLevel == 1)
        {
            _currentHP += _increaseHP1;
            _hpUI.UpdateHPUI();
        }
        if (wallLevel == 2)
        {
            _currentHP += _increaseHP2;
            _hpUI.UpdateHPUI();
        }
    }
    // �_���[�W���󂯂鏈��
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _currentHP = Mathf.Max(0, _currentHP); // HP������0�ɐݒ�
        _hpUI.UpdateHPUI();

        Debug.Log($"���_���_���[�W���󂯂��I �c��HP: {_currentHP}");

        if (_currentHP <= 0)
        {
            OnDestroyed();
        }
    }
    private void OnDestroyed()
    {
        Debug.Log("�G���_���j�󂳂ꂽ�I");
        gameObject.SetActive(false); // �Ƃ肠������\����
        SceneLoader.Instance.LoadScene("GameOver");
        // �����ŃQ�[���I�[�o�[��G�t�F�N�g���o���Ă�OK
    }

}
