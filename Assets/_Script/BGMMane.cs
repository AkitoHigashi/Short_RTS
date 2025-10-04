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
        // ���ɑ��݂���ꍇ�͍폜
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource���Ȃ��ꍇ�͒ǉ�
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // �V�[���ύX���̃C�x���g�o�^
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    void PlayBGMForScene(string sceneName)
    {
        // �V�[���ɑΉ�����BGM��T��
        for (int i = 0; i < sceneBGMs.Length; i++)
        {
            if (sceneBGMs[i].sceneName == sceneName)
            {
                // BGM���Đ�
                audioSource.clip = sceneBGMs[i].bgmClip;
                audioSource.Play();
                return;
            }
        }

        // �Ή�����BGM���Ȃ��ꍇ�͒�~
        audioSource.Stop();
    }
}


