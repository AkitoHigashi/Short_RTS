using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
/// <summary>
/// �v���C���[�̎�������X�N���v�g
/// </summary>
public class Player_Resource : MonoBehaviour
{
    [SerializeField] private ResourceUI_WorldSpace RUWS;
    [SerializeField] private string _wheatTag = "Wheat";
    [SerializeField] private string _treeTag = "Tree";
    [SerializeField] private string _stoneTag = "Stone";
    [SerializeField, Header("�������")] private float _duration = 6f;

    private Coroutine _treeCoroutine;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(_wheatTag))
        {
        }
        else if (other.CompareTag(_treeTag))
        {
            _treeCoroutine = StartCoroutine(TreeProcess(other.transform));
            Debug.Log("�R���[�`���J�n");
        }
        else if (other.CompareTag(_stoneTag))
        {
            Debug.Log("�΂̒�");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_wheatTag))
        {

        }
        else if (other.CompareTag(_treeTag) && _treeCoroutine != null)
        {
            StopCoroutine(_treeCoroutine);
            _treeCoroutine = null;
            Debug.Log("�R���[�`����~");
        }
        else if (other.CompareTag(_stoneTag))
        {

        }
    }
  // private IEnumerator WheatProcess() { }
    private IEnumerator TreeProcess(Transform target)
    {
        while (true)
        {
            float timer = 0f;
            while (timer < _duration)//�^�C�}�[����
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / _duration);//0-1�͈̔͂ŏo��

                yield return null;
            }
            RUWS.Spawn_Resource(target);
        }
    }
}
