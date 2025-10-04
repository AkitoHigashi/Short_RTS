using UnityEngine;

public class Base_Archery : Base
{
    [SerializeField] private GameObject _soldierPrefab; // �\���W���[��Prefab
    [SerializeField] private Transform _spawnPoint; // �����ʒu
    [SerializeField] private int _stoneCostForSpawn = 10; // �K�v�f�ނȑf��
    [SerializeField] private int _plusCost = 0; // �����R�X�g��{0
    private int _currentCost; // ���̃R�X�g
    public override int ReqStone
    {
        get { return _currentCost; }
    }

    // �؂Ɣ��͕K�v�Ȃ��̂ŃI�[�o�[���C�h
    public override int ReqTree
    {
        get { return 0; }
    }

    public override int ReqWheat
    {
        get { return 0; }
    }

    protected override void OnEnable()
    {
        _currentCost = _stoneCostForSpawn; // �����ݒ�
        base.OnEnable();
    }
    public override void Upgrade()
    {
        // ���N���X��Upgrade()���Ă΂��ɁA����SpawnSoldier()�����s
        // ����ɂ�� _currentLevel �̑������������
        SpawnSoldier();
    }

    /// <summary>
    /// �������邩�`�F�b�N
    /// </summary>
    /// <returns></returns>
    private bool CanSpawnSoldier()
    {
        return Inventory.Instance.CheckResource(0, _currentCost, 0);
    }

    /// <summary>
    /// �\���W���[�𐶐����郁�\�b�h
    /// </summary>
    public void SpawnSoldier()
    {
        if (CanSpawnSoldier() == false)
        {
            Debug.Log("�΂�����܂���");
            return;
        }

        // �Ώ���
        Inventory.Instance.UseResourse(0, _currentCost, 0);

        // �\���W���[�𐶐�
        GameObject newSoldier = Instantiate(_soldierPrefab, _spawnPoint.position, Quaternion.identity);
        Debug.Log("�\���W���[�����I�I");

        // �R�X�g����
        _currentCost += _plusCost;

        // UI�X�V
        UIUpdate();
    }

    /// <summary>
    /// ���x���A�b�v���Ȃ����߁A�\���W���[�����\�����`�F�b�N
    /// �ő僌�x�������͖�������
    /// </summary>
    /// <returns></returns>
    public override bool CheckUpgrade()
    {
        // ���N���X�̃��x���`�F�b�N�𖳎����āA�����`�F�b�N�̂ݎ��s
        return CanSpawnSoldier();
    }

    /// <summary>
    /// ���x�������𖳎�����悤��
    /// </summary>
    public override void UIUpdate()
    {
        // �K�v���ނƐ���UI�ɔ��f
        if (ReqTree > 0)
        {
            _treeImage.gameObject.SetActive(true);
            _reqText.text = ReqTree.ToString();
        }
        else if (ReqStone > 0)
        {
            _stoneImage.gameObject.SetActive(true);
            _reqText.text = ReqStone.ToString();
        }
        else if (ReqWheat > 0)
        {
            _wheatImage.gameObject.SetActive(true);
            _reqText.text = ReqWheat.ToString();
        }
    }
}
