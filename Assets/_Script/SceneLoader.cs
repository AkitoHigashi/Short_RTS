using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image _loadImage;//���[�h���
    [SerializeField] private TextMeshProUGUI _loadText;//���[�h�e�L�X�g
    [SerializeField] private float _fadeTime = 0.5f;
    public static SceneLoader Instance;//�V���O���g��

    private void Awake()
    {
        if (Instance != null && Instance != this)//�C���X�^���X�̓k���ŃC���X�^���X�ɓ����Ă�Ƃ�
        {
            Destroy(Instance);
            return;
        }
        Instance = this; //����
        DontDestroyOnLoad(gameObject);//�V�[�����ׂ��ł��j������Ȃ�
        //�ŏ��͓����ɂ���
        _loadImage.color = new Color(_loadImage.color.r, _loadImage.color.g, _loadImage.color.b, 0f);
        _loadText.color = new Color(_loadText.color.r, _loadText.color.g, _loadText.color.b, 0f);
    }
    //�V�[���؂�ւ����\�b�h
    public void LoadScene(string sceneName)
    {   //�t�F�[�h�C��
        _loadText.DOFade(1f, _fadeTime);
        _loadImage.DOFade(1f, _fadeTime)
            .OnComplete(() =>
            {
                //�V�[���؂�ւ�
                SceneManager.LoadScene(sceneName);

                _loadImage.DOFade(0f, _fadeTime);
                _loadText.DOFade(0f, _fadeTime);
            });

 

    }
}
