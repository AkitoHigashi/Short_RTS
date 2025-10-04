using UnityEngine;
using DG.Tweening;
public class ResorceUI_move : MonoBehaviour
{
    private RectTransform _rectpos;//このアタッチされているやつのPOS；
    [SerializeField] private float _moveDuration = 0.6f;
    [SerializeField] private int _amount = 2;

    [Header("どの資源か選択")]
    [SerializeField] private ResourceType _resourceType; // ← インスペクタで Tree / Stone / Wheat を選択

    private void Start()
    {
        _rectpos = GetComponent<RectTransform>();

        // 対象のUI位置を取得
        RectTransform target = null;
        switch (_resourceType)
        {
            case ResourceType.Tree:
                target = UI_Maneger.Instance.GetTreePos();
                break;
            case ResourceType.Stone:
                target = UI_Maneger.Instance.GetStonePos();
                break;
            case ResourceType.Wheat:
                target = UI_Maneger.Instance.GetWheatPos();
                break;
        }
        if (target == null)
        {
            Debug.LogError($"ResourceUI_move: {_resourceType} のターゲットUIが見つかりません");
            return;
        }
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
            Inventory.Instance.AddResource(_resourceType, _amount);
            Destroy(gameObject);
        });
        //最後は小さくフェードアウト(なくていいかも）
       // seq.Append(_rectpos.DOScale(0f, 0.3f).SetEase(Ease.Linear));
    }
}
