using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;//����ǂ��킩��Ȃ�

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IPlayerActions//�p������
{
    [SerializeField] float _moveSpeed = 10.0f;
    [SerializeField] float _rotationSpeed = 15f;
    private InputSystem_Actions _actions;
    private Rigidbody _rb;
    private Animator _animator;
    private Vector2 _moveInput;

    void Awake()
    {
        _actions = new();//new InputSystem_Actions();�̂���
        _actions.Enable();//���ׂĂ̓��̓A�N�V������L��
        _actions.Player.SetCallbacks(this);//Player�ɑΉ��������͂��󂯎��Ƃ��̃X�N���v�g���Ăяo���悤�Ɏw��
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    void OnDisable()
    {
        _actions.Player.RemoveCallbacks(this);//Awake() �ōs�����R�[���o�b�N�̓o�^������
        _actions.Disable();//���ׂĂ̓��̓A�N�V�����𖳌�
        _actions.Dispose();//InputSystem_Actions �C���X�^���X���g�p���Ă������\�[�X��������邽�߂ɌĂяo��
        _actions = null;//GC����̂���
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
