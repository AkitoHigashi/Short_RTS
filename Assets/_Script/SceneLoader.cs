using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image _loadImage;//ロード画面
    [SerializeField] private TextMeshProUGUI _loadText;//ロードテキスト
    [SerializeField] private float _fadeTime = 0.5f;
    public static SceneLoader Instance;//シングルトン

    private void Awake()
    {
        if (Instance != null && Instance != this)//インスタンスはヌルでインスタンスに入ってるとき
        {
            Destroy(Instance);
            return;
        }
        Instance = this; //生成
        DontDestroyOnLoad(gameObject);//シーンを跨いでも破棄されない
        //最初は透明にする
        _loadImage.color = new Color(_loadImage.color.r, _loadImage.color.g, _loadImage.color.b, 0f);
        _loadText.color = new Color(_loadText.color.r, _loadText.color.g, _loadText.color.b, 0f);
    }
    //シーン切り替えメソッド
    public void LoadScene(string sceneName)
    {   //フェードイン
        _loadText.DOFade(1f, _fadeTime);
        _loadImage.DOFade(1f, _fadeTime)
            .OnComplete(() =>
            {
                //シーン切り替え
                SceneManager.LoadScene(sceneName);

                _loadImage.DOFade(0f, _fadeTime);
                _loadText.DOFade(0f, _fadeTime);
            });

 

    }
}
