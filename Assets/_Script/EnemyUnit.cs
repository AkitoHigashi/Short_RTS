using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour
{
    [Header("��{�ݒ�")]
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

        // �v���C���[���_��T��
        if (_playerBase == null)
        {
            _playerBase = GameObject.FindGameObjectWithTag("PlayerBase").transform;
        }

        // ���_�Ɍ������Ĉړ��J�n
        _navAgent.SetDestination(_playerBase.position);
        _animator.SetBool("Run", true);
    }

    private void Update()
    {
        if (_isDead) return;

        if (!_hasReachedBase)
        {
            // ���_�Ƃ̋����`�F�b�N
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
            // �U��
            if (Time.time >= _lastAttackTime + _attackCooldown)
            {
                Attack();
                _lastAttackTime = Time.time;
            }
        }
    }

    private void Attack()
    {
        // ���_�Ƀ_���[�W
        _playerBase.GetComponent<PlayerBase>().TakeDamage(_attackDamage);

        // �U���A�j���[�V����
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
        // �U���͈�
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        // �v���C���[���_�ւ̐�
        if (_playerBase != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _playerBase.position);
        }
    }
}

