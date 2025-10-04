using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;//これ良くわからない

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IPlayerActions//継承する
{
    [Header("Player Movement")]
    [SerializeField] float _moveSpeed = 10.0f;
    [SerializeField] float _rotationSpeed = 15f;

    [Header("Minion Control")]
    [SerializeField] private MinionController _minionController;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _grounMask = -1;
    private InputSystem_Actions _actions;
    private Rigidbody _rb;
    private Animator _animator;
    private Vector2 _moveInput;

    void Awake()
    {
        _actions = new();//new InputSystem_Actions();のこと
        _actions.Enable();//すべての入力アクションを有効
        _actions.Player.SetCallbacks(this);//Playerに対応した入力を受け取るとこのスクリプトを呼び出すように指示
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    void OnDisable()
    {
        _actions.Player.RemoveCallbacks(this);//Awake() で行ったコールバックの登録を解除
        _actions.Disable();//すべての入力アクションを無効
        _actions.Dispose();//InputSystem_Actions インスタンスが使用していたリソースを解放するために呼び出す
        _actions = null;//GC解放のため
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveInput != Vector2.zero)
        {
            Vector3 direction = new Vector3(_moveInput.x, 0f, _moveInput.y);
            transform.forward = Vector3.Lerp(transform.forward, direction.normalized, _rotationSpeed * Time.deltaTime);
        }
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(_moveInput.x, 0f, _moveInput.y);
        _rb.linearVelocity = movement.normalized * _moveSpeed;
    }
    private void LateUpdate()
    {
        _animator.SetFloat("MoveSpeed", _moveInput.magnitude);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    //右クリックしたとき
    public void OnWhistle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 worldPos = GetMouseWorldPositon();
            if (worldPos != Vector3.zero)//有効位置だった時
            {
                Debug.Log("右クリック");
                _minionController.Return(worldPos);
            }
        }
    }
    //左クリック
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("左クリック");
            Vector3 worldPos = GetMouseWorldPositon();
            _minionController.GoTo(worldPos);//左クリック押したときのポイントの位置を送る
        }
    }
    /// <summary>
    /// マウスの位置を3D空間の座標に変換
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMouseWorldPositon()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();//インプットシステムでカメラのポジションを取得
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);//メインカメラから飛ばすレイキャスト

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _grounMask))//そのレイキャストにオブジェクトを取得
        {
            return hit.point;//ポジションを返す
        }
        return Vector3.zero;//ヒットしない場合
    }


    public void OnCrouch(InputAction.CallbackContext context) { }

    public void OnInteract(InputAction.CallbackContext context) { }

    public void OnJump(InputAction.CallbackContext context) { }

    public void OnLook(InputAction.CallbackContext context) { }

    public void OnNext(InputAction.CallbackContext context) { }

    public void OnPrevious(InputAction.CallbackContext context) { }

    public void OnSprint(InputAction.CallbackContext context) { }
}
