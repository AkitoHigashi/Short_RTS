using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class TimeUI : MonoBehaviour
{
    [SerializeField, Header("�������Ԃ�UI")] private TextMeshProUGUI _timeText;
    [SerializeField, Header("��������")] private float _limitTime = 120f;//��������
    [SerializeField, Header("�^�C���A�b�v����UI")] private Image _timeUpImage;
    [SerializeField, Tooltip("�^�C���A�b�vUI�̕\���܂ł̎���")] private float _upUISpeed = 1f;

    private float _currentTime;//���̎���
    private bool _isTimeUp = false;
    void Start()
    {
        _currentTime = _limitTime;
        TimerUpdate();
        _timeUpImage.rectTransform.localScale = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        if (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;//�^�C�}�[�������鏈��
            if (_currentTime < 0)
            {
                _currentTime = 0;//�}�C�i�X�ɂȂ�Ȃ��悤�ɕ␳
            }
            TimerUpdate();
        }
        else if (!_isTimeUp)//�^�C���A�b�v���̏���
        {
            Debug.Log("�^�C���A�b�v!!");
            _timeUpImage.rectTransform.DOScale(1f, _upUISpeed).SetEase(Ease.OutElastic);
            _isTimeUp = true;
        }
    }
    private void TimerUpdate()
    {
        int minutes = Mathf.FloorToInt(_currentTime / 60);//60�Ŋ����ď����_��؂�̂�
        int seconds = Mathf.FloorToInt(_currentTime % 60);//60�Ŋ����ė]���\��
        string text = string.Format("{0:00}:{1:00}", minutes, seconds);//string�̃t�H�[�}�b�g���w��@0�Ԗڂ̈�����00�̌^�ŕ\��1�Ԗڂ������悤��
        _timeText.text = "��������" + text;
    }
}
