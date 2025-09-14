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


}
