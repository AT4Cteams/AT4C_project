using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class FootstepSE : MonoBehaviour
{
    [System.Serializable]
    public class AudioClips
    {
        public string groundTypeTag; // 地面の種類を表すタグ
        public AudioClip[] audioClips; // 対応するオーディオクリップの配列
    }

    [SerializeField] List<AudioClips> listAudioClips = new List<AudioClips>(); // 足音の種類ごとにタグとオーディオクリップを登録するリスト
    [SerializeField] float pitchRange = 0.1f; // ピッチの範囲

    private Dictionary<string, int> tagToIndex = new Dictionary<string, int>(); // タグをインデックスに変換する辞書
    private int groundIndex = 0; // 現在の地面のインデックス

    private AudioSource source; // オーディオソース

    private void Awake()
    {
        // 最初にアタッチされたオーディオソースを使用する
        source = GetComponents<AudioSource>()[0];

        // タグをインデックスに変換する辞書を初期化する
        for (int i = 0; i < listAudioClips.Count; ++i)
        {
            tagToIndex.Add(listAudioClips[i].groundTypeTag, i);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトのタグを確認し、対応するインデックスを設定する
        if (tagToIndex.ContainsKey(collision.gameObject.tag))
        {
            if (collision.gameObject.tag != "Untagged")
            {
                groundIndex = tagToIndex[collision.gameObject.tag];
            }
        }

        Debug.Log("地面のタグ: " + collision.gameObject.tag);
    }

    public void PlayFootstepSE()
    {
        // 現在の地面タイプに対応するオーディオクリップを取得する
        AudioClip[] clips = listAudioClips[groundIndex].audioClips;

        // 指定された範囲でピッチをランダムに設定する
        source.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);

        // 現在の地面タイプのオーディオクリップからランダムに選んで再生する
        AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot(clipToPlay);

        Debug.Log("再生するクリップ: " + clipToPlay.name);
    }
}
