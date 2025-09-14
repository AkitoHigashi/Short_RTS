using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;//これ良くわからない

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IPlayerActions//継承する
{
    [SerializeField] float _moveSpeed = 10.0f;
    [SerializeField] float _rotationSpeed = 15f;
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
        //Debug.Log();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }


    public void OnNext(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }
}
