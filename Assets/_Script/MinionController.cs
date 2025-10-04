using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class MinionController : MonoBehaviour
{
    [SerializeField] private float _controllRudius = 5f;
    [SerializeField] private LayerMask _minionMask;
    [SerializeField] private GameObject _circlePrefab;//�~�̃G�t�F�N�g
    [SerializeField] private float _circleDuration = 0.5f;//�\������
    private Vector3 _lastWorldPos;//Ruturn���̃}�E�X�̃|�W�V�������L���i�E�N���b�N�j
    [SerializeField] private List<MinionBase> _followingMinions = new List<MinionBase>();//�t�H���[���̃~�j�I�����X�g
    /// <summary>
    /// �w�肵���|�W�V�����ɉ~���o�����͂̃~�j�I�����t�H���[���[�h�ɕς��郁�\�b�h
    /// </summary>
    /// <param name="worldPos"></param>
    public void Return(Vector3 worldPos)
    {
        ShowCircle(worldPos);
        _lastWorldPos = worldPos;
        Collider[] hits = Physics.OverlapSphere(worldPos, _controllRudius, _minionMask);
        foreach (Collider hit in hits)
        {
            Debug.Log(hit.name);
            //�q�b�g�����I�u�W�F�g�̃~�j�I���x�[�X���擾
            MinionBase minion = hit.GetComponent<MinionBase>();
            //�~�j�I���x�[�X�������Ă邩�X�e�[�g���t�H���[���ȊO�̏ꍇ���s
            if (minion != null && minion.baseState != MinionBase.BaseState.FollowMode)
            {
                Debug.Log("�t�H���[�J�n");
                minion.SetFollow();//�t�H���[������

                if (!_followingMinions.Contains(minion))//�t�H���[���X�g�̂Ȃ��ɂ��̃I�u�W�F�N�g���ۑ�����Ă��邩
                {
                    _followingMinions.Add(minion);//�t�H���[�����X�g�ɒǉ�
                }
            }
        }
    }
    /// <summary>
    /// �t�H���[���̃~�j�I������̂����w��|�W�V�����܂ňړ������郁�\�b�h�i���N���b�N�j
    /// </summary>
    /// <param name="worldPos"></param>
    public void GoTo(Vector3 worldPos)
    {
        _followingMinions.RemoveAll(NullCheck);//���X�g���Ƀk��������ƃ��X�g���l�߂�B
        foreach (var minion in _followingMinions)
        {
            if(minion != null && minion.baseState == MinionBase.BaseState.FollowMode)
            {
                //�ړ��̃��\�b�h
                minion.CommandMove(worldPos);
                _followingMinions.Remove(minion);//���̃~�j�I�������X�g����폜
                break; // ��̂����ړ������邽�߈��ŏI���
            }
        }

    }
    /// <summary>
    ///�@�k���Ȃ̂���check����
    /// </summary>
    /// <param name="minion">�t�H���[���̃R���C�_�[</param>
    /// <returns>�^�U�`�F�b�N</returns>
    private bool NullCheck(MinionBase minion)
    {
        return minion == null;
    }
    /// <summary>
    /// �}�E�X�̃N���b�N�����ӏ��ɉ~��\������
    /// </summary>
    /// <param name="pos">�E�N���b�N�����Ƃ��̃}�E�X�̃|�W�V����</param>
    private void ShowCircle(Vector3 pos)
    {
        if (_circlePrefab == null) return;
        //�|�C���^�̈ʒu�ɉ~�𐶐�
        GameObject circle = Instantiate(_circlePrefab, pos + Vector3.up * 0.05f, Quaternion.identity);
        circle.transform.localScale = Vector3.zero;//�ŏ��͌����Ȃ�
        Vector3 targetScale = new Vector3(_controllRudius * 2f, 0.001f, _controllRudius * 2f);
        //�傫������
        circle.transform.DOScale(targetScale, _circleDuration).SetEase(Ease.OutQuart).OnComplete(() => Destroy(circle));
    }
    // �f�o�b�O�p�ɉ~��\��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_lastWorldPos, _controllRudius);
    }
}