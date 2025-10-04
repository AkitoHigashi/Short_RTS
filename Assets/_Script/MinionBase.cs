using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// �~�j�I���ɕt������N���X
/// </summary>
public abstract class MinionBase : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected Animator _animator;
    protected Transform _playerPos;

    [SerializeField] protected float _followDis = 2f;
    [SerializeField] Transform _player;
    protected Vector3 _targetPos;

    public enum BaseState
    {
        Idle,
        FollowMode,
        MoveToPoint
    }

    public BaseState baseState = BaseState.Idle;
    protected virtual void Start()
    {
        // �v���C���[�������蓖�Ă̏ꍇ�A�����Ō���
        if (_player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
                Debug.Log($"{name}: �v���C���[�������Ō��o���܂���");
            }
            else
            {
                Debug.LogError($"{name}: �v���C���[��������Ȃ��I");
                return;
            }
        }
        initialize(_player);
    }

    /// <summary>
    /// ���������~�j�I���ɕt����R���|�[�l���g
    /// </summary>
    public virtual void initialize(Transform playerPos)
    {
        _playerPos = playerPos;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    protected virtual void Update()
    {
        switch (baseState)
        {
            case BaseState.Idle:
                Idle();
                break;
            case BaseState.FollowMode:
                FollowPlayer();
                break;
            case BaseState.MoveToPoint:
                MoveToPoint();
                break;
        }
    }
    protected void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, _playerPos.position);

        if (distance > _followDis)
        {
            _agent.isStopped = false;//true�ňړ���~�Afalse�ōĊJ
            _agent.SetDestination(_playerPos.position);
            _animator.SetBool("Run", true);
        }
        else
        {
            _agent.isStopped = true;
            _animator.SetBool("Run", false);
        }
    }
    protected void MoveToPoint()
    {
        if (!_agent.hasPath)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_targetPos);
            Debug.Log($"{name}: �ړ��J�n - �ړI�n: {_targetPos}");
            _animator.SetBool("Run", true);
        }
        else if (!_agent.pathPending && _agent.remainingDistance <= 0.5f)
        {
            _agent.isStopped = true;
            _animator.SetBool("Run", false);
            Debug.Log($"{name}: �ړI�n�ɓ���");
            OnArrivedTarget();
            baseState = BaseState.Idle;
        }

    }
    /// <summary>
    /// �ړ��������ꏊ�ł����鏈��������
    /// </summary>
    protected abstract void OnArrivedTarget();
    protected virtual void Idle()
    {
        _animator.SetBool("Run", false);
    }
    public virtual void CommandMove(Vector3 pos)
    {
        _targetPos = pos;
        if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            _targetPos = hit.position; // NavMesh��̗L���Ȉʒu�ɒ���
            Debug.Log($"{name}: �L���ȖړI�n: {_targetPos}");
        }
        else
        {
            Debug.LogError($"{name}: �����ȖړI�n: {pos}");
            return; // �����ȏꍇ�͈ړ����Ȃ�
        }
        // ��Ԃ�ύX����O�ɃG�[�W�F���g�����Z�b�g
        _agent.isStopped = false;
        _agent.ResetPath(); // �����̃p�X���N���A
        baseState = BaseState.MoveToPoint;
    }
    public virtual void SetFollow()
    {
        baseState = BaseState.FollowMode;
    }
}
