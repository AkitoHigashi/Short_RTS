using UnityEngine;

public class Archer : MinionBase
{
    [Header("�U���ݒ�")]
    [SerializeField] private int _attackDamage = 30;
    [SerializeField] private float _attackRange = 20f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private LayerMask _targetLayerMask; // �G�Ƌ��_�̗������܂�

    private Transform _currentTarget;
    private float _lastAttackTime;

    protected override void Update()
    {
        // �ҋ@��Ԃ̎��̂ݓG���m
        if (baseState == BaseState.Idle)
        {
            _currentTarget = FindNearestTarget();
            if (_currentTarget != null)
            {
                // �U�����[�h�F�ړ���~
                _agent.isStopped = true;
                _animator.SetBool("Run", false);

                // �^�[�Q�b�g����������
                Vector3 direction = (_currentTarget.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);

                // �U�����s
                if (Time.time >= _lastAttackTime + _attackCooldown)
                {
                    Attack();
                    _lastAttackTime = Time.time;
                }
                return; // �U�����͑��̏��������Ȃ�
            }
        }
        else
        {
            // �Ǐ]����ړ����͓G�𖳎�
            _currentTarget = null;
        }

        // �ʏ�̍s���i�Ǐ]/�ړ�/�ҋ@�j
        base.Update();
    }

    private Transform FindNearestTarget()
    {
        // �U���͈͓��̃^�[�Q�b�g�i�G���j�b�g�{�G���_�j�����m
        Collider[] targets = Physics.OverlapSphere(transform.position, _attackRange, _targetLayerMask);
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        // �ł��߂��^�[�Q�b�g��I��
        foreach (Collider target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = target.transform;
            }
        }

        return nearest;
    }

    private void Attack()
    {
        if (_currentTarget == null) return;

        bool damageDealt = false;

        // 1. �ʏ�̓G���j�b�g�iEnemyUnit�j���`�F�b�N
        EnemyUnit enemyUnit = _currentTarget.GetComponent<EnemyUnit>();
        if (enemyUnit != null)
        {
            enemyUnit.TakeDamage(_attackDamage);
            damageDealt = true;
            Debug.Log($"�A�[�`���[���G���j�b�g��{_attackDamage}�_���[�W�I");
        }

        // 2. �G���_�iEnemyBase�j���`�F�b�N
        if (!damageDealt)
        {
            EnemyBase enemyBase = _currentTarget.GetComponent<EnemyBase>();
            if (enemyBase != null)
            {
                enemyBase.TakeDamage(_attackDamage);
                damageDealt = true;
                Debug.Log($"�A�[�`���[���G���_��{_attackDamage}�_���[�W�I");
            }
        }

        // 3. �ėp�I�ȃ_���[�W�C���^�[�t�F�[�X�iIDamageable�j���`�F�b�N
        if (!damageDealt)
        {
            IDamageable damageable = _currentTarget.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_attackDamage);
                damageDealt = true;
                Debug.Log($"�A�[�`���[���^�[�Q�b�g��{_attackDamage}�_���[�W�I");
            }
        }

        // �_���[�W��^�����Ȃ������ꍇ�̌x��
        if (!damageDealt)
        {
            Debug.LogWarning($"�^�[�Q�b�g {_currentTarget.name} �Ƀ_���[�W��^���邱�Ƃ��ł��܂���ł����I�K�؂ȃR���|�[�l���g������܂���B");
        }

        // �G�t�F�N�g�p�e�ۂ𔭎�
        if (_bulletPrefab != null && _firePoint != null)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            Vector3 direction = (_currentTarget.position - _firePoint.position).normalized;

            // Unity 6�Ή��F���S��Rigidbody����
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * _bulletSpeed;
            }

            Destroy(bullet, 1f);
        }

        // �U���A�j���[�V�����Đ�
        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }
    }

    protected override void OnArrivedTarget()
    {
        // �ړ��������̏����i���ɂȂ��j
    }

    private void OnDrawGizmosSelected()
    {
        // �U���͈͂�����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}

// �ėp�I�ȃ_���[�W�C���^�[�t�F�[�X
public interface IDamageable
{
    void TakeDamage(int damage);
}