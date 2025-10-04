using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Base : MonoBehaviour
{
    [Header("���ʂ̃X�e�[�^�X")]
    [SerializeField] private protected int _currentLevel = 0;
    [SerializeField] private int _maxLevel;
    [SerializeField] private GameObject _trigger;

    [Header("�i���̕K�v�����R�X�g")]
    [SerializeField] private int[] _treeCost;
    [SerializeField] private int[] _stoneCost;
    [SerializeField] private int[] _wheatCost;

    [Header("UI")]
    [SerializeField] private protected Canvas _canvas;
    [SerializeField] private protected�@TextMeshProUGUI _reqText;
    [SerializeField] private protected�@Image _treeImage;
    [SerializeField] private protected Image _stoneImage;
    [SerializeField] private protected Image _wheatImage;

    protected virtual void OnEnable()
    {
        UIUpdate();
    }
    protected int Level { get { return _currentLevel; } }
    /// <summary>
    /// �����ɕK�v�Ȏ���:��
    /// </summary>
    public virtual int ReqTree
    {
        get { return _treeCost[_currentLevel]; }

    }
    /// <summary>
    /// �����ɕK�v�Ȏ���:��
    /// </summary>
    public virtual int ReqStone
    {
        get { return _stoneCost[_currentLevel]; }
    }
    /// <summary>
    /// �����ɕK�v�Ȏ���:��
    /// </summary>
    public virtual int ReqWheat
    {
        get { return _wheatCost[_currentLevel]; }
    }
    /// <summary>
    /// �A�b�v�O���[�h�ł��邩�`�F�b�N
    /// </summary>
    public virtual bool CheckUpgrade()
    {
        if (_currentLevel >= _maxLevel)
        {
            Debug.Log("����ȏ�i���ł��܂���");
            return false;
        }
        return Inventory.Instance.CheckResource(ReqTree, ReqStone, ReqWheat);
    }
    /// <summary>
    /// �A�b�v�O���[�h�����s
    /// </summary>
    public virtual void Upgrade()
    {
        if (CheckUpgrade() == false)
        {
            return;
        }
        Inventory.Instance.UseResourse(ReqTree, ReqStone, ReqWheat);
        _currentLevel = _currentLevel + 1;
        UIUpdate();
    }
    /// <summary>
    /// �󂯎�����I�u�W�F�̃Z�b�g�A�N�e�B�u��ύX���郁�\�b�h
    /// </summary>
    /// <param name="objects"></param>
    /// <param name="active"></param>
    public virtual void SetActiveObjcts(GameObject[] objects, bool active)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(active);
        }
    }
   public virtual void UIUpdate()
    {
        // ���x�����ő�Ȃ� Canvas �ƃg���K�[���A�N�e�B�u��
        if (_currentLevel >= _maxLevel)
        {
            if (_canvas != null && _trigger != null)
            {
                _canvas.gameObject.SetActive(false);
                _trigger.gameObject.SetActive(false);

            }
            return;
        }
        //��x��A�N�e�B�u��
        _treeImage.gameObject.SetActive(false);
        _stoneImage.gameObject.SetActive(false);
        _wheatImage.gameObject.SetActive(false);

        // �K�v���ނƐ���UI�ɔ��f
        if (ReqTree > 0)
        {
            _treeImage.gameObject.SetActive(true);
            _reqText.text = ReqTree.ToString();
        }
        else if (ReqStone > 0)
        {
            _stoneImage.gameObject.SetActive(true);
            _reqText.text = ReqStone.ToString();
        }
        else if (ReqWheat > 0)
        {
            _wheatImage.gameObject.SetActive(true);
            _reqText.text = ReqWheat.ToString();
        }
    }
}
