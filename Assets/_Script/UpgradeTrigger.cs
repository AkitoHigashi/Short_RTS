using UnityEngine;
using UnityEngine.UI;
public class UpgradeTrigger : MonoBehaviour
{
    [SerializeField] private float _upgradeTime = 3f;
    [SerializeField] private Image _progressBar;//fillamoutUI
    [Header("参照")]
    [SerializeField] private Base _base;//基底クラス

    private float _timer;
    private bool _insidePlayer;
    private bool _canUpgrade = false;

    private void Update()
    {
        if (_insidePlayer)
        {
            if (_canUpgrade)
            {
                _timer += Time.deltaTime;//タイマー
                _progressBar.fillAmount = Mathf.Clamp01(_timer / _upgradeTime);

                if (_timer >= _upgradeTime)
                {
                    Finsish();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //資源持ってるかチェックしてboolで戻す
            _canUpgrade = Inventory.Instance.CheckResource(_base.ReqTree, _base.ReqStone, _base.ReqWheat);
            if (_canUpgrade)
            {
                Debug.Log("強化タイマー開始！！");
                _insidePlayer = true;
                _timer = 0f;
            }
            else
            {
                _progressBar.fillAmount = 0f;
                _insidePlayer = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Cancel();
        }
    }
    private void Cancel()
    {
        _insidePlayer = false;
        _canUpgrade = false;
        _timer = 0f;
        _progressBar.fillAmount = 0f;
        Debug.Log("進化キャンセル");
    }
    private void Finsish()
    {
        Debug.Log("進化！！");
        _base.Upgrade();
        _insidePlayer = false;
        _canUpgrade = false;
        _timer = 0f;
        _progressBar.fillAmount = Mathf.Clamp01(_timer / _upgradeTime);
    }
}
