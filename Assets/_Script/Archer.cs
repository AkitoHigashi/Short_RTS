using UnityEngine;

public class Archer : MinionBase
{
    [Header("攻撃設定")]
    [SerializeField] private int _attackDamage = 30;
    [SerializeField] private float _attackRange = 20f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private LayerMask _targetLayerMask; // 敵と拠点の両方を含む

    private Transform _currentTarget;
    private float _lastAttackTime;

    protected override void Update()
    {
        // 待機状態の時のみ敵検知
        if (baseState == BaseState.Idle)
        {
            _currentTarget = FindNearestTarget();
            if (_currentTarget != null)
            {
                // 攻撃モード：移動停止
                _agent.isStopped = true;
                _animator.SetBool("Run", false);

                // ターゲット方向を向く
                Vector3 direction = (_currentTarget.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);

                // 攻撃実行
                if (Time.time >= _lastAttackTime + _attackCooldown)
                {
                    Attack();
                    _lastAttackTime = Time.time;
                }
                return; // 攻撃中は他の処理をしない
            }
        }
        else
        {
            // 追従中や移動中は敵を無視
            _currentTarget = null;
        }

        // 通常の行動（追従/移動/待機）
        base.Update();
    }

    private Transform FindNearestTarget()
    {
        // 攻撃範囲内のターゲット（敵ユニット＋敵拠点）を検知
        Collider[] targets = Physics.OverlapSphere(transform.position, _attackRange, _targetLayerMask);
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        // 最も近いターゲットを選択
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

        // 1. 通常の敵ユニット（EnemyUnit）をチェック
        EnemyUnit enemyUnit = _currentTarget.GetComponent<EnemyUnit>();
        if (enemyUnit != null)
        {
            enemyUnit.TakeDamage(_attackDamage);
            damageDealt = true;
            Debug.Log($"アーチャーが敵ユニットに{_attackDamage}ダメージ！");
        }

        // 2. 敵拠点（EnemyBase）をチェック
        if (!damageDealt)
        {
            EnemyBase enemyBase = _currentTarget.GetComponent<EnemyBase>();
            if (enemyBase != null)
            {
                enemyBase.TakeDamage(_attackDamage);
                damageDealt = true;
                Debug.Log($"アーチャーが敵拠点に{_attackDamage}ダメージ！");
            }
        }

        // 3. 汎用的なダメージインターフェース（IDamageable）をチェック
        if (!damageDealt)
        {
            IDamageable damageable = _currentTarget.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_attackDamage);
                damageDealt = true;
                Debug.Log($"アーチャーがターゲットに{_attackDamage}ダメージ！");
            }
        }

        // ダメージを与えられなかった場合の警告
        if (!damageDealt)
        {
            Debug.LogWarning($"ターゲット {_currentTarget.name} にダメージを与えることができませんでした！適切なコンポーネントがありません。");
        }

        // エフェクト用弾丸を発射
        if (_bulletPrefab != null && _firePoint != null)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            Vector3 direction = (_currentTarget.position - _firePoint.position).normalized;

            // Unity 6対応：安全なRigidbody操作
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * _bulletSpeed;
            }

            Destroy(bullet, 1f);
        }

        // 攻撃アニメーション再生
        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }
    }

    protected override void OnArrivedTarget()
    {
        // 移動完了時の処理（特になし）
    }

    private void OnDrawGizmosSelected()
    {
        // 攻撃範囲を可視化
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}

// 汎用的なダメージインターフェース
public interface IDamageable
{
    void TakeDamage(int damage);
}