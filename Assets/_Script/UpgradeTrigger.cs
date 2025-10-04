using UnityEngine;
using UnityEngine.UI;
public class UpgradeTrigger : MonoBehaviour
{
    [SerializeField] private float _upgradeTime = 3f;
    [SerializeField] private Image _progressBar;//fillamoutUI
    [Header("�Q��")]
    [SerializeField] private Base _base;//���N���X

    private float _timer;
    private bool _insidePlayer;
    private bool _canUpgrade = false;

    private void Update()
    {
        if (_insidePlayer)
        {
            if (_canUpgrade)
            {
                _timer += Time.deltaTime;//�^�C�}�[
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
            //���������Ă邩�`�F�b�N����bool�Ŗ߂�
            _canUpgrade = Inventory.Instance.CheckResource(_base.ReqTree, _base.ReqStone, _base.ReqWheat);
            if (_canUpgrade)
            {
                Debug.Log("�����^�C�}�[�J�n�I�I");
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
        Debug.Log("�i���L�����Z��");
    }
    private void Finsish()
    {
        Debug.Log("�i���I�I");
        _base.Upgrade();
        _insidePlayer = false;
        _canUpgrade = false;
        _timer = 0f;
        _progressBar.fillAmount = Mathf.Clamp01(_timer / _upgradeTime);
    }
}
