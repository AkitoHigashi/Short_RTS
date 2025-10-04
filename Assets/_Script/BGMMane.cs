using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMMane : MonoBehaviour
{

    [System.Serializable]
    public class BGMData
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    [SerializeField] private BGMData[] sceneBGMs;
    [SerializeField] private AudioSource audioSource;

    private static BGMMane instance;

    void Awake()
    {
        // 既に存在する場合は削除
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSourceがない場合は追加
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // シーン変更時のイベント登録
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    void PlayBGMForScene(string sceneName)
    {
        // シーンに対応するBGMを探す
        for (int i = 0; i < sceneBGMs.Length; i++)
        {
            if (sceneBGMs[i].sceneName == sceneName)
            {
                // BGMを再生
                audioSource.clip = sceneBGMs[i].bgmClip;
                audioSource.Play();
                return;
            }
        }

        // 対応するBGMがない場合は停止
        audioSource.Stop();
    }
}


