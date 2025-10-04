using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour
{
    [Header("基本設定")]
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private int _attackDamage = 20;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private Transform _playerBase;

    private NavMeshAgent _navAgent;
    private Animator _animator;
    private int _currentHP;
    private float _lastAttackTime;
    private bool _isDead = false;
    private bool _hasReachedBase = false;

    private void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _currentHP = _maxHP;
        _navAgent.speed = _moveSpeed;

        // プレイヤー拠点を探す
        if (_playerBase == null)
        {
            _playerBase = GameObject.FindGameObjectWithTag("PlayerBase").transform;
        }

        // 拠点に向かって移動開始
        _navAgent.SetDestination(_playerBase.position);
        _animator.SetBool("Run", true);
    }

    private void Update()
    {
        if (_isDead) return;

        if (!_hasReachedBase)
        {
            // 拠点との距離チェック
            float distance = Vector3.Distance(transform.position, _playerBase.position);
            if (distance <= _attackRange)
            {
                _hasReachedBase = true;
                _navAgent.ResetPath();
                _animator.SetBool("Run", false);
                _animator.SetBool("Idel", true);
            }
        }
        else
        {
            // 攻撃
            if (Time.time >= _lastAttackTime + _attackCooldown)
            {
                Attack();
                _lastAttackTime = Time.time;
            }
        }
    }

    private void Attack()
    {
        // 拠点にダメージ
        _playerBase.GetComponent<PlayerBase>().TakeDamage(_attackDamage);

        // 攻撃アニメーション
        _animator.SetTrigger("Attack");
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            _isDead = true;
            gameObject.SetActive(false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        // 攻撃範囲
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        // プレイヤー拠点への線
        if (_playerBase != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _playerBase.position);
        }
    }
}

