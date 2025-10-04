using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBaseHPUI : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField] private Image _hpFillImage; // HPゲージの塗りつぶし部分
    [SerializeField] private Image _hpBackgroundImage; // HPゲージの背景

    [Header("プレイヤー拠点の参照")]
    [SerializeField] private PlayerBase _playerBase; // プレイヤー拠点への参照

    private int _currentHP;
    private int _maxHP;

    private void Start()
    {
        // PlayerBaseを自動検索（設定されていない場合）
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
