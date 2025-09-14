using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ResorceUI_move : MonoBehaviour
{
    private RectTransform _rectpos;//このアタッチされているやつのPOS；
    [SerializeField] private float _moveDuration = 0.6f;

    private void Start()
    {
        _rectpos = GetComponent<RectTransform>();

        RectTransform target = UI_Maneger.Instance.GetTreePos();
        //シークエンスの作成
        Sequence seq = DOTween.Sequence();
        //大きくPOPするようにする。
        seq.Append(_rectpos.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        //移動する
        //移動しながら大きくなり
        seq.Join(_rectpos.DOAnchorPos(target.anchoredPosition, _moveDuration).SetDelay(0.5f).SetEase(Ease.InBack));
        seq.Join(_rectpos.DOScale(1.3f, _moveDuration * 0.5f).SetEase(Ease.InOutQuad));
        //途中から小さくなる
        seq.Append(_rectpos.DOScale(0f, _moveDuration * 0.5f).SetEase(Ease.InExpo));
        seq.AppendCallback(() =>
        {
            Inventory.Instance.Plus_treeCount(1);
            Destroy(gameObject);
        });
        //最後は小さくフェードアウト(なくていいかも）
       // seq.Append(_rectpos.DOScale(0f, 0.3f).SetEase(Ease.Linear));
    }
}
