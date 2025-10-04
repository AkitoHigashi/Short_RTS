using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

/// <summary>
/// �v���C���[�̎�������V�X�e��
/// ���\�[�X�G���A�ɓ���Ǝ����Ŏ���������J�n���A�G���A����o��ƒ�~����
/// </summary>
public class Player_Resource : MonoBehaviour
{
    [Header("���\�[�X�ݒ�")]
    [SerializeField] private ResourceUI_WorldSpace RUWS;
    [Header("�v���O���XUI�ݒ�")]
    [SerializeField] private Image _progressBar;
    [SerializeField] private Camera _camera;
    [Header("���\�[�X�^�O�ݒ�")]
    [SerializeField] private string _wheatTag = "Wheat";
    [SerializeField] private string _treeTag = "Tree";
    [SerializeField] private string _stoneTag = "Stone";

    [Header("������Ԑݒ�")]
    [SerializeField, Tooltip("���\�[�X1���������̂ɂ����鎞�ԁi�b�j")]
    private float _duration = 6f;

    // ���݂̏�����Ԃ��Ǘ�����ϐ�
    private Coroutine _currentCoroutine = null;  // ���ݎ��s���̃R���[�`��
    private Transform _currentTarget = null;     // ���ݏ������̃��\�[�X�G���A

    private void Start()
    {
        if (RUWS == null)
        {
            RUWS = FindAnyObjectByType<ResourceUI_WorldSpace>();
            if (RUWS == null)
            {
                Debug.LogError("ResourceUI_WorldSpace���V�[���Ɍ�����Ȃ�");
            }
        }
    }
    /// <summary>
    /// ���\�[�X�G���A�ɓ��������̏���
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // ���\�[�X�G���A���ǂ����𔻒�
        if (IsResourceArea(other.tag))
        {
            StartNewResourceProcess(other.tag, other.transform);
        }
    }

    /// <summary>
    /// ���\�[�X�G���A����o�����̏���
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        // ���ݏ������̃G���A����o���ꍇ�̂ݏ������~
        if (_currentTarget == other.transform)
        {
            StopCurrentResourceProcess();
            Debug.Log($"{other.tag}�G���A({other.name})�Ŏ�������I���I");
        }
    }

    /// <summary>
    /// �w�肳�ꂽ�^�O�����\�[�X�G���A���ǂ����𔻒�
    /// </summary>
    /// <param name="tag">���肷��^�O</param>
    /// <returns>���\�[�X�G���A�̏ꍇtrue</returns>
    private bool IsResourceArea(string tag)
    {
        return tag == _wheatTag || tag == _treeTag || tag == _stoneTag;
    }

    /// <summary>
    /// �V�������\�[�X����������J�n
    /// </summary>
    /// <param name="resourceType">���\�[�X�̎��</param>
    /// <param name="target">�����Ώۂ�Transform</param>
    private void StartNewResourceProcess(string resourceType, Transform target)
    {
        // �����̏���������Β�~
        StopCurrentResourceProcess();

        // �V�����������J�n
        _currentTarget = target;
        _currentCoroutine = StartCoroutine(ResourceProcess(resourceType, target));

        Debug.Log($"{resourceType}�G���A({target.name})�Ŏ�������J�n�I");
    }

    /// <summary>
    /// ���݂̃��\�[�X����������~
    /// </summary>
    private void StopCurrentResourceProcess()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            _currentTarget = null;
            Debug.Log("���\�[�X����������~���܂���");
        }
    }

    /// <summary>
    /// ���\�[�X����̃��C�����[�v
    /// �w�肵���Ԋu�Ń��\�[�X�𐶐���������
    /// </summary>
    /// <param name="resourceType">���\�[�X�̎�ށi�f�o�b�O�p�j</param>
    /// <param name="target">���\�[�X�����Ώۂ�Transform</param>
    /// <returns>�R���[�`��</returns>
    private IEnumerator ResourceProcess(string resourceType, Transform target)
    {
        while (true)
        {
            // ������Ԃ̃J�E���g�_�E��
            float timer = 0f;
            while (timer < _duration)
            {
                timer += Time.deltaTime;

                // �i�������v�Z�i0�`1�͈̔́j
                float progress = Mathf.Clamp01(timer / _duration);
                UpdateProgressUI(progress);

                // ���̃t���[���܂őҋ@
                yield return null;
            }

            // ���\�[�X�𐶐�
            RUWS.Spawn_Resource(resourceType, target);
            Debug.Log($"{resourceType}���\�[�X��������܂����I");
        }
    }
    private void UpdateProgressUI(float progress)
    { // �v���O���X�o�[���X�V
        if (_progressBar != null)
        {
            _progressBar.DOFillAmount(progress, 0.4f);
        }
    }
    private void Update()
    {
        if (_progressBar != null)
        {
            _progressBar.transform.LookAt(_camera.transform.position);
        }

    }

    /// <summary>
    /// �I�u�W�F�N�g�j�����̃N���[���A�b�v����
    /// ���������[�N��h�����߂ɕK�v
    /// </summary>
    private void OnDestroy()
    {
        StopCurrentResourceProcess();
    }
}