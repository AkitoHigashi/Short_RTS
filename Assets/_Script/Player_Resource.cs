using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
/// <summary>
/// プレイヤーの資源回収スクリプト
/// </summary>
public class Player_Resource : MonoBehaviour
{
    [SerializeField] private ResourceUI_WorldSpace RUWS;
    [SerializeField] private string _wheatTag = "Wheat";
    [SerializeField] private string _treeTag = "Tree";
    [SerializeField] private string _stoneTag = "Stone";
    [SerializeField, Header("回収時間")] private float _duration = 6f;

    private Coroutine _treeCoroutine;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(_wheatTag))
        {
        }
        else if (other.CompareTag(_treeTag))
        {
            _treeCoroutine = StartCoroutine(TreeProcess(other.transform));
            Debug.Log("コルーチン開始");
        }
        else if (other.CompareTag(_stoneTag))
        {
            Debug.Log("石の中");
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
            Debug.Log("コルーチン停止");
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
            while (timer < _duration)//タイマー制作
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / _duration);//0-1の範囲で出力

                yield return null;
            }
            RUWS.Spawn_Resource(target);
        }
    }
}
