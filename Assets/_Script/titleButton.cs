using UnityEngine;
using TMPro;
using DG.Tweening;

public class titleButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private float _fadeDuration = 1f;  // �t�F�[�h����

    private void Start()
    {
        LoopFade();
    }

    private void LoopFade()
    {
        // 0 = ���S����, 1 = �s����
        _titleText.alpha = 0f;

        // �t�F�[�h�C�� �� �t�F�[�h�A�E�g�𖳌����[�v
        _titleText.DOFade(1f, _fadeDuration)       // �t�F�[�h�C��
                  .SetLoops(-1, LoopType.Yoyo);   // -1 = �������[�v, Yoyo = �s����
    }
}
