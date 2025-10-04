using UnityEngine;

public class UI_Maneger : MonoBehaviour
{
    /// <summary>
    /// UImaneger���Q�Ƃł���悤�ɃV���O���g��
    /// </summary>
    public static UI_Maneger Instance
    {
        get;
        private set;
    }

    [SerializeField] private RectTransform _treeUIPos;//�ړI��UI
    [SerializeField] private RectTransform _stoneUIPos;//�΂�UI
    [SerializeField] private RectTransform _wheatUIPos;//����UI

    private void Awake()
    {
        //������
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);//����Ȃ����
        }
    }

    /// <summary>
    /// UI�̃|�W�V������Ԃ��|�W�V����
    /// </summary>
    /// <returns>���\�[�X��UI�̃|�W�V�������A��</returns>
    public RectTransform GetTreePos()
    {
        return _treeUIPos;
    }
    public RectTransform GetStonePos()
    {
        return _stoneUIPos;
    }
    public RectTransform GetWheatPos()
    {
        return _wheatUIPos;
    }


}
