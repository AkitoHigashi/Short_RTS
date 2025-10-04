using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{
    [Header("�����ڂ̐ݒ�")]
    [SerializeField] private GameObject[] _levelPrefabs;
    [SerializeField] private float _upgradeInterval = 50f;
    [Header("HP�ݒ�")]
    [SerializeField] private int _maxHP = 300; // ����HP
    [Header("UI�ݒ�")]
    [SerializeField] private Image _hpFillImage; // HP�Q�[�W�̓h��Ԃ������j

    [Header("�G���j�b�g�����ݒ�")]
    [SerializeField] private GameObject _enemyPrefab; // ��������G�v���n�u
    [SerializeField] private Transform[] _spawnPoints; // �����ʒu
    [SerializeField] private float[] _spawnIntervals = { 15f, 10f, 5f, 1f }; // ���x�����̐����Ԋu
    [SerializeField] private bool _spawnEnabled = true; // ������ON/OFF
    private int _currentHP;

    private int _currentLevel = 0;
    private float _timer = 0f;
    private float _spawnTimer = 0f;
    private void Start()
    {
        _currentHP = _maxHP; // HP������
        UpdateBaseVisual();
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _upgradeInterval)
        {
            UpgradeBase();
            _timer = 0f;
        }
        // �G���j�b�g�����̃^�C�}�[
        if (_spawnEnabled && _currentLevel < _spawnIntervals.Length)
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnIntervals[_currentLevel])
            {
                SpawnEnemyUnit();
                _spawnTimer = 0f;
            }
        }
    }
    /// <summary>
    /// �A�b�v�O���[�h�̌�����
    /// </summary>
    private void UpdateBaseVisual()
    {
        // �S����\��
        foreach (GameObject obj in _levelPrefabs)
        {
            obj.SetActive(false);
        }
        // ���݃��x�������\��
        _levelPrefabs[_currentLevel].SetActive(true);
    }
    private void UpgradeBase()
    {
        if (_currentLevel < _levelPrefabs.Length - 1)
        {
            _currentLevel++;
            UpdateBaseVisual();
            Debug.Log("���_�����x���A�b�v: " + _currentLevel);
        }
    }
    // �_���[�W���󂯂鏈��
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _currentHP = Mathf.Max(0, _currentHP); // HP������0�ɐݒ�

        Debug.Log($"�G���_���_���[�W���󂯂��I �c��HP: {_currentHP}");
        UpdateHPUI();

        if (_currentHP <= 0)
        {
            OnDestroyed();
        }
    }
    // �j�󂳂ꂽ���̏���
    private void OnDestroyed()
    {
        Debug.Log("�G���_���j�󂳂ꂽ�I");
        gameObject.SetActive(false); // ��\����
                                     // �����ŃQ�[���I�[�o�[��G�t�F�N�g
        SceneLoader.Instance.LoadScene("GameClear");
    }
    /// <summary>
    /// �G���j�b�g�𐶐�
    /// </summary>
    private void SpawnEnemyUnit()
    {
        // �G�v���n�u�����邩�`�F�b�N
        if (_enemyPrefab == null)
        {
            Debug.LogWarning("�G�v���n�u���ݒ肳��Ă��܂���I");
            return;
        }

        // �����ʒu�����邩�`�F�b�N
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            Debug.LogWarning("�����ʒu���ݒ肳��Ă��܂���I");
            return;
        }
        // �����_���Ȑ����ʒu��I��
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        // �G���j�b�g����
        GameObject spawnedEnemy = Instantiate(_enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    /// <summary>
    /// �����I�ɓG���j�b�g����
    /// </summary>
    public void ForceSpawnUnit()
    {
        SpawnEnemyUnit();
    }

    // HP���O������擾�ł���悤��
    public int GetCurrentHP()
    {
        return _currentHP;
    }
    public int GetMaxHP()
    {
        return _maxHP;
    }
    // HP�Q�[�W��UI���X�V
    private void UpdateHPUI()
    {
        if (_hpFillImage != null)
        {
            float fillAmount = (float)_currentHP / (float)_maxHP;
            _hpFillImage.DOFillAmount(fillAmount,0.2f);
        }
    }
}
