using UnityEngine;

public class Base_Home : Base
{
    [SerializeField] private GameObject _minionPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _wheatCostForSpawn = 5;//�������̕K�v�̏����
    [SerializeField] private int _plusCost = 3;//�����R�X�g

    private int _currentCost;//���̃R�X�g

    public override int ReqWheat
    {
        get { return _currentCost; }
    }
    // �؂Ɛ΂͕K�v�Ȃ��̂ŃI�[�o�[���C�h
    public override int ReqTree
    {
        get { return 0; }
    }

    public override int ReqStone
    {
        get { return 0; }
    }
    protected override void OnEnable()
    {
        _currentCost = _wheatCostForSpawn; // �����ݒ�
        base.OnEnable();
    }
    public override void Upgrade()
    {
        // ���N���X��Upgrade()���Ă΂��ɁA����SpawnMinion()�����s
        // ����ɂ�� _currentLevel �̑������������
        SpawnMinion();
    }
    /// <summary>
    /// �������邩�`�F�b�N
    /// </summary>
    /// <returns></returns>
    private bool CanSpawnMinion()
    {
        return Inventory.Instance.CheckResource(0, 0, _currentCost);
    }
    /// <summary>
    /// �~�j�I���𐶐����郁�\�b�h
    /// </summary>
    public void SpawnMinion()
    {
        if (CanSpawnMinion() == false)
        {
            return;
        }
        //������
        Inventory.Instance.UseResourse(0, 0, _currentCost);
        //�~�j�I���𐶐�
        GameObject newMinion = Instantiate(_minionPrefab, _spawnPoint.position, Quaternion.identity);
        Debug.Log("�~�j�I�������I�I");

        //�R�X�g����
        _currentCost += _plusCost;
        //UI�X�V
        UIUpdate();
    }
    /// <summary>
    /// ���x���A�b�v���Ȃ����߁A�~�j�I�������\�����`�F�b�N
    /// �ő僌�x�������͖�������
    /// </summary>
    /// <returns></returns>
    public override bool CheckUpgrade()
    {
        // ���N���X�̃��x���`�F�b�N�𖳎����āA�����`�F�b�N�̂ݎ��sa
        return CanSpawnMinion();
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