using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip audioClip;
        public float playedTime;    //前回再生した時間
        public AudioSource audioSource; //再生中の AudioSource
        public float minDistance = 1f; // 最小距離
        public float maxDistance = 50f; // 最大距離
    }

    [SerializeField]
    private SoundData[] soundDatas;

    //AudioSource（スピーカー）を同時に鳴らしたい音の数だけ用意
    private AudioSource[] audioSourceList = new AudioSource[20];

    //別名(name)をキーとした管理用Dictionary
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

    //一度再生してから、次再生出来るまでの間隔(秒)
    [SerializeField]
    private float playableDistance = 0.2f;

    //１つであることを保証するため＆グローバルアクセス用
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
        else if (Instance != this) //自身が他にもあるようなら
        {
            Destroy(gameObject); //削除
            return;
        }

        //auidioSourceList配列の数だけAudioSourceを自分自身に生成して配列に格納
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            audioSourceList[i] = gameObject.AddComponent<AudioSource>();
            audioSourceList[i].spatialBlend = 1.0f; // 3Dサウンドにする
        }

        //soundDictionaryにセット
        foreach (var soundData in soundDatas)
        {
            soundDictionary.Add(soundData.name, soundData);
        }
    }

    //未使用のAudioSourceの取得 全て使用中の場合はnullを返却
    private AudioSource GetUnusedAudioSource()
    {
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            if (audioSourceList[i].isPlaying == false) return audioSourceList[i];
        }

        return null; //未使用のAudioSourceは見つかりませんでした
    }

    //指定されたAudioClipを未使用のAudioSourceで再生
    private void Play(AudioClip clip, Vector3 position, float minDistance, float maxDistance)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource == null) return; //再生できませんでした
        audioSource.clip = clip;
        audioSource.transform.position = position; // 音源の位置を設定
        audioSource.minDistance = minDistance; // 最小距離を設定
        audioSource.maxDistance = maxDistance; // 最大距離を設定
        audioSource.Play();
    }

    //指定された別名で登録されたAudioClipを再生
    public void Play(string name, Vector3 position)
    {
        if (soundDictionary.TryGetValue(name, out var soundData)) //管理用Dictionary から、別名で探索
        {
            if (Time.realtimeSinceStartup - soundData.playedTime < playableDistance) return;    //まだ再生するには早い
            soundData.playedTime = Time.realtimeSinceStartup;//次回用に今回の再生時間の保持
            Play(soundData.audioClip, position, soundData.minDistance, soundData.maxDistance); //見つかったら、再生
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}");
        }
    }

    //再生停止
    // 指定された AudioClip を再生している AudioSource を停止
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
        Debug.LogWarning("指定された AudioClip を再生している AudioSource が見つかりませんでした");
    }

    // 指定された別名に関連付けられた AudioClip を再生している AudioSource を停止
    public void Stop(string name)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            Stop(soundData.audioClip);
            Debug.Log(name + "再生停止");
        }
        else
        {
            Debug.LogWarning($"サウンド「{name}」が登録されていません");
        }
    }
}
