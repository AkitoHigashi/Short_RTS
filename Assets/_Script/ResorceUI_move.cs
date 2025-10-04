using UnityEngine;
using DG.Tweening;
public class ResorceUI_move : MonoBehaviour
{
    private RectTransform _rectpos;//���̃A�^�b�`����Ă�����POS�G
    [SerializeField] private float _moveDuration = 0.6f;
    [SerializeField] private int _amount = 2;

    [Header("�ǂ̎������I��")]
    [SerializeField] private ResourceType _resourceType; // �� �C���X�y�N�^�� Tree / Stone / Wheat ��I��

    private void Start()
    {
        _rectpos = GetComponent<RectTransform>();

        // �Ώۂ�UI�ʒu���擾
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
            Debug.LogError($"ResourceUI_move: {_resourceType} �̃^�[�Q�b�gUI��������܂���");
            return;
        }
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
            Inventory.Instance.AddResource(_resourceType, _amount);
            Destroy(gameObject);
        });
        //�Ō�͏������t�F�[�h�A�E�g(�Ȃ��Ă��������j
       // seq.Append(_rectpos.DOScale(0f, 0.3f).SetEase(Ease.Linear));
    }
}
