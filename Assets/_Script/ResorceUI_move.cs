using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ResorceUI_move : MonoBehaviour
{
    private RectTransform _rectpos;//���̃A�^�b�`����Ă�����POS�G
    [SerializeField] private float _moveDuration = 0.6f;

    private void Start()
    {
        _rectpos = GetComponent<RectTransform>();

        RectTransform target = UI_Maneger.Instance.GetTreePos();
        //�V�[�N�G���X�̍쐬
        Sequence seq = DOTween.Sequence();
        //�傫��POP����悤�ɂ���B
        seq.Append(_rectpos.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        //�ړ�����
        //�ړ����Ȃ���傫���Ȃ�
        seq.Join(_rectpos.DOAnchorPos(target.anchoredPosition, _moveDuration).SetDelay(0.5f).SetEase(Ease.InBack));
        seq.Join(_rectpos.DOScale(1.3f, _moveDuration * 0.5f).SetEase(Ease.InOutQuad));
        //�r�����珬�����Ȃ�
        seq.Append(_rectpos.DOScale(0f, _moveDuration * 0.5f).SetEase(Ease.InExpo));
        seq.AppendCallback(() =>
        {
            Inventory.Instance.Plus_treeCount(1);
            Destroy(gameObject);
        });
        //�Ō�͏������t�F�[�h�A�E�g(�Ȃ��Ă��������j
       // seq.Append(_rectpos.DOScale(0f, 0.3f).SetEase(Ease.Linear));
    }
}
