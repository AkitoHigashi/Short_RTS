using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class TimeUI : MonoBehaviour
{
    [SerializeField, Header("制限時間のUI")] private TextMeshProUGUI _timeText;
    [SerializeField, Header("制限時間")] private float _limitTime = 120f;//制限時間
    [SerializeField, Header("タイムアップ時のUI")] private Image _timeUpImage;
    [SerializeField, Tooltip("タイムアップUIの表示までの時間")] private float _upUISpeed = 1f;

    private float _currentTime;//今の時間
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
            _currentTime -= Time.deltaTime;//タイマーを下げる処理
            if (_currentTime < 0)
            {
                _currentTime = 0;//マイナスにならないように補正
            }
            TimerUpdate();
        }
        else if (!_isTimeUp)//タイムアップ時の処理
        {
            Debug.Log("タイムアップ!!");
            _timeUpImage.rectTransform.DOScale(1f, _upUISpeed).SetEase(Ease.OutElastic);
            _isTimeUp = true;
        }
    }
    private void TimerUpdate()
    {
        int minutes = Mathf.FloorToInt(_currentTime / 60);//60で割って小数点を切り捨て
        int seconds = Mathf.FloorToInt(_currentTime % 60);//60で割って余りを表示
        string text = string.Format("{0:00}:{1:00}", minutes, seconds);//stringのフォーマットを指定　0番目の引数を00の型で表示1番目も同じように
        _timeText.text = "制限時間" + text;
    }
}
