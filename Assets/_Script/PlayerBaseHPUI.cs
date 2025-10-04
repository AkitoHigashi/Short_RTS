using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBaseHPUI : MonoBehaviour
{
    [Header("UI�v�f")]
    [SerializeField] private Image _hpFillImage; // HP�Q�[�W�̓h��Ԃ�����
    [SerializeField] private Image _hpBackgroundImage; // HP�Q�[�W�̔w�i

    [Header("�v���C���[���_�̎Q��")]
    [SerializeField] private PlayerBase _playerBase; // �v���C���[���_�ւ̎Q��

    private int _currentHP;
    private int _maxHP;

    private void Start()
    {
        // PlayerBase�����������i�ݒ肳��Ă��Ȃ��ꍇ�j
        if (_playerBase == null)
        {
            _playerBase = FindAnyObjectByType<PlayerBase>();
        }
        UpdateHPUI();
    }

    public void UpdateHPUI()
    {
       _currentHP = _playerBase.CureentHP;
        _maxHP = _playerBase.MaxHP;
        if (_hpFillImage != null)
        {
            float fillAmount = (float)_currentHP / (float)_maxHP;
            _hpFillImage.DOFillAmount(fillAmount, 0.2f);
        }
    }
}
