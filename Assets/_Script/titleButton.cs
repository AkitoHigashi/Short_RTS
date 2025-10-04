using UnityEngine;
using TMPro;
using DG.Tweening;

public class titleButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private float _fadeDuration = 1f;  // フェード時間

    private void Start()
    {
        LoopFade();
    }

    private void LoopFade()
    {
        // 0 = 完全透明, 1 = 不透明
        _titleText.alpha = 0f;

        // フェードイン → フェードアウトを無限ループ
        _titleText.DOFade(1f, _fadeDuration)       // フェードイン
                  .SetLoops(-1, LoopType.Yoyo);   // -1 = 無限ループ, Yoyo = 行き来
    }
}
