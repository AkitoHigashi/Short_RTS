using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;//����ǂ��킩��Ȃ�

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IPlayerActions//�p������
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
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    //�E�N���b�N�����Ƃ�
    public void OnWhistle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 worldPos = GetMouseWorldPositon();
            if (worldPos != Vector3.zero)//�L���ʒu��������
            {
                Debug.Log("�E�N���b�N");
                _minionController.Return(worldPos);
            }
        }
    }
    //���N���b�N
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("���N���b�N");
            Vector3 worldPos = GetMouseWorldPositon();
            _minionController.GoTo(worldPos);//���N���b�N�������Ƃ��̃|�C���g�̈ʒu�𑗂�
        }
    }
    /// <summary>
    /// �}�E�X�̈ʒu��3D��Ԃ̍��W�ɕϊ�
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMouseWorldPositon()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();//�C���v�b�g�V�X�e���ŃJ�����̃|�W�V�������擾
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);//���C���J���������΂����C�L���X�g

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _grounMask))//���̃��C�L���X�g�ɃI�u�W�F�N�g���擾
        {
            return hit.point;//�|�W�V������Ԃ�
        }
        return Vector3.zero;//�q�b�g���Ȃ��ꍇ
    }


    public void OnCrouch(InputAction.CallbackContext context) { }

    public void OnInteract(InputAction.CallbackContext context) { }

    public void OnJump(InputAction.CallbackContext context) { }

    public void OnLook(InputAction.CallbackContext context) { }

    public void OnNext(InputAction.CallbackContext context) { }

    public void OnPrevious(InputAction.CallbackContext context) { }

    public void OnSprint(InputAction.CallbackContext context) { }
}
