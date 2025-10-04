using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// ミニオンに付ける基底クラス
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
        // プレイヤーが未割り当ての場合、自動で検索
        if (_player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
                Debug.Log($"{name}: プレイヤーを自動で検出しました");
            }
            else
            {
                Debug.LogError($"{name}: プレイヤーが見つからない！");
                return;
            }
        }
        initialize(_player);
    }

    /// <summary>
    /// 生成したミニオンに付けるコンポーネント
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
            _agent.isStopped = false;//trueで移動停止、falseで再開
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
            Debug.Log($"{name}: 移動開始 - 目的地: {_targetPos}");
            _animator.SetBool("Run", true);
        }
        else if (!_agent.pathPending && _agent.remainingDistance <= 0.5f)
        {
            _agent.isStopped = true;
            _animator.SetBool("Run", false);
            Debug.Log($"{name}: 目的地に到着");
            OnArrivedTarget();
            baseState = BaseState.Idle;
        }

    }
    /// <summary>
    /// 移動させた場所でさせる処理を書く
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
            _targetPos = hit.position; // NavMesh上の有効な位置に調整
            Debug.Log($"{name}: 有効な目的地: {_targetPos}");
        }
        else
        {
            Debug.LogError($"{name}: 無効な目的地: {pos}");
            return; // 無効な場合は移動しない
        }
        // 状態を変更する前にエージェントをリセット
        _agent.isStopped = false;
        _agent.ResetPath(); // 既存のパスをクリア
        baseState = BaseState.MoveToPoint;
    }
    public virtual void SetFollow()
    {
        baseState = BaseState.FollowMode;
    }
}
