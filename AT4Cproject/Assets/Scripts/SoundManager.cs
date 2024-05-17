using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip audioClip;
        public float playedTime;    // 前回再生した時間
        public AudioSource audioSource; // 再生中のAudioSource
        public float minDistance = 1f; // 最小距離
        public float maxDistance = 50f; // 最大距離
    }

    [SerializeField]
    private SoundData[] soundDatas;

    [SerializeField]
    private AudioSource[] audioSourceList;

    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

    [SerializeField]
    private float playableDistance = 0.2f;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            audioSourceList[i] = gameObject.AddComponent<AudioSource>();
            audioSourceList[i].spatialBlend = 1.0f; // 3Dサウンドにする
        }

        foreach (var soundData in soundDatas)
        {
            soundDictionary.Add(soundData.name, soundData);
        }
    }

    private AudioSource GetUnusedAudioSource()
    {
        foreach (var audioSource in audioSourceList)
        {
            if (!audioSource.isPlaying) return audioSource;
        }
        return null;
    }

    private void Play(AudioClip clip, Vector3 position, float minDistance, float maxDistance)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource == null)
        {
            Debug.LogWarning("未使用のAudioSourceが見つかりませんでした");
            return;
        }

        audioSource.clip = clip;
        audioSource.transform.position = position;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        audioSource.Play();
    }

    public void Play(string name, Vector3 position, float minDistance, float maxDistance)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            if (Time.realtimeSinceStartup - soundData.playedTime < playableDistance) return;
            soundData.playedTime = Time.realtimeSinceStartup;
            Play(soundData.audioClip, position, minDistance, maxDistance);
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません: {name}");
        }
    }

    public void Stop(AudioClip clip)
    {
        foreach (var audioSource in audioSourceList)
        {
            if (audioSource.clip == clip && audioSource.isPlaying)
            {
                audioSource.Stop();
                return;
            }
        }
        Debug.LogWarning("指定されたAudioClipを再生しているAudioSourceが見つかりませんでした");
    }

    public void Stop(string name)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            Stop(soundData.audioClip);
            Debug.Log($"{name}再生停止");
        }
        else
        {
            Debug.LogWarning($"サウンド「{name}」が登録されていません");
        }
    }
}
