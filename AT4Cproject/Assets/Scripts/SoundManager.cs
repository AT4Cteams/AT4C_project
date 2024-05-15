using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip audioClip;
        public float playedTime;    //�O��Đ���������
        public AudioSource audioSource; //�Đ����� AudioSource
        public float minDistance = 1f; // �ŏ�����
        public float maxDistance = 50f; // �ő勗��
    }

    [SerializeField]
    private SoundData[] soundDatas;

    //AudioSource�i�X�s�[�J�[�j�𓯎��ɖ炵�������̐������p��
    private AudioSource[] audioSourceList = new AudioSource[20];

    //�ʖ�(name)���L�[�Ƃ����Ǘ��pDictionary
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

    //��x�Đ����Ă���A���Đ��o����܂ł̊Ԋu(�b)
    [SerializeField]
    private float playableDistance = 0.2f;

    //�P�ł��邱�Ƃ�ۏ؂��邽�߁��O���[�o���A�N�Z�X�p
    public static SoundManager Instance
    {
        private set;
        get;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this) //���g�����ɂ�����悤�Ȃ�
        {
            Destroy(gameObject); //�폜
            return;
        }

        //auidioSourceList�z��̐�����AudioSource���������g�ɐ������Ĕz��Ɋi�[
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            audioSourceList[i] = gameObject.AddComponent<AudioSource>();
            audioSourceList[i].spatialBlend = 1.0f; // 3D�T�E���h�ɂ���
        }

        //soundDictionary�ɃZ�b�g
        foreach (var soundData in soundDatas)
        {
            soundDictionary.Add(soundData.name, soundData);
        }
    }

    //���g�p��AudioSource�̎擾 �S�Ďg�p���̏ꍇ��null��ԋp
    private AudioSource GetUnusedAudioSource()
    {
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            if (audioSourceList[i].isPlaying == false) return audioSourceList[i];
        }

        return null; //���g�p��AudioSource�͌�����܂���ł���
    }

    //�w�肳�ꂽAudioClip�𖢎g�p��AudioSource�ōĐ�
    private void Play(AudioClip clip, Vector3 position, float minDistance, float maxDistance)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource == null) return; //�Đ��ł��܂���ł���
        audioSource.clip = clip;
        audioSource.transform.position = position; // �����̈ʒu��ݒ�
        audioSource.minDistance = minDistance; // �ŏ�������ݒ�
        audioSource.maxDistance = maxDistance; // �ő勗����ݒ�
        audioSource.Play();
    }

    //�w�肳�ꂽ�ʖ��œo�^���ꂽAudioClip���Đ�
    public void Play(string name, Vector3 position)
    {
        if (soundDictionary.TryGetValue(name, out var soundData)) //�Ǘ��pDictionary ����A�ʖ��ŒT��
        {
            if (Time.realtimeSinceStartup - soundData.playedTime < playableDistance) return;    //�܂��Đ�����ɂ͑���
            soundData.playedTime = Time.realtimeSinceStartup;//����p�ɍ���̍Đ����Ԃ̕ێ�
            Play(soundData.audioClip, position, soundData.minDistance, soundData.maxDistance); //����������A�Đ�
        }
        else
        {
            Debug.LogWarning($"���̕ʖ��͓o�^����Ă��܂���:{name}");
        }
    }

    //�Đ���~
    // �w�肳�ꂽ AudioClip ���Đ����Ă��� AudioSource ���~
    public void Stop(AudioClip clip)
    {
        for (int i = 0; i < audioSourceList.Length; i++)
        {
            if (audioSourceList[i].clip == clip && audioSourceList[i].isPlaying)
            {
                audioSourceList[i].Stop();
                return;
            }
        }
        Debug.LogWarning("�w�肳�ꂽ AudioClip ���Đ����Ă��� AudioSource ��������܂���ł���");
    }

    // �w�肳�ꂽ�ʖ��Ɋ֘A�t����ꂽ AudioClip ���Đ����Ă��� AudioSource ���~
    public void Stop(string name)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            Stop(soundData.audioClip);
            Debug.Log(name + "�Đ���~");
        }
        else
        {
            Debug.LogWarning($"�T�E���h�u{name}�v���o�^����Ă��܂���");
        }
    }
}
