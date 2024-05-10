/**
 * @brief 音クラス
 * @author 村上
 * 
 * @details Sound.Generate(Sound.SoundLevel.レベル数, 生成するポジション); ←で生成することができます。 @n
 *          音の聞こえる範囲などは後々変更が必要
 */

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public enum SoundLevel
{
    lv1,
    lv2,
    lv3,
    lv4,
    lv5,
}

public class Sound : MonoBehaviour
{
    private Vector3 _startSize;
    private float _finishTime = 0f;
    private float _startTime = 0f;

    [SerializeField]
    private float[] _maxVolume = new float[5];
    private float _volume = 0f;

    [SerializeField]
    private float[] _maxSize = new float[5];

    [SerializeField]
    private Material[] _material = new Material[5];

    [SerializeField]
    private GameObject _areaObject;

    [HideInInspector]
    public SoundLevel level;

    public bool isHit { get; set; }
    public float volume { get { return _volume; }}

    // Start is called before the first frame update
    void Start()
    {
        _startSize = transform.localScale;
        _finishTime = 0.5f;
        _startTime = Time.time;
        //Invoke("Finish", .3f);
    }

    // Update is called once per frame
    void Update()
    {
        Expention();
    }

    private void Finish()
    {
        Destroy(this.gameObject);
    }

    private void SetLevel(SoundLevel soundLevel)
    {
        level = soundLevel;

        SetMaxVolume((int)soundLevel);

        _areaObject.GetComponent<MeshRenderer>().material = _material[(int)level];
    }

    public static void Generate(SoundLevel soundLevel, Vector3 position)
    {
        GameObject newSound = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");

        newSound.GetComponent<Sound>().SetLevel(soundLevel);

        Instantiate(newSound, position, Quaternion.identity);
    }


    // 速度によってレベル変更
    public static void VelocityToGenerate(GameObject gameObject)
    {
        GameObject newSound = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");
        float velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        SoundLevel sl;

        if (velocity > 20) sl = SoundLevel.lv5;
        else if (velocity > 15) sl = SoundLevel.lv4;
        else if (velocity > 10) sl = SoundLevel.lv3;
        else if (velocity > 15) sl = SoundLevel.lv2;
        else sl = SoundLevel.lv1;

        newSound.GetComponent<Sound>().SetLevel(sl);

        Instantiate(newSound, gameObject.transform.position, Quaternion.identity);
    }

    private void Expention()
    {
        Vector3 targetScale = new Vector3(_maxSize[(int)level], _maxSize[(int)level], _maxSize[(int)level]);

        float elapsed = Time.time - _startTime;
        float t = Mathf.Clamp01(elapsed / _finishTime);
        transform.localScale = Vector3.Lerp(_startSize, targetScale, t);

        if (t >= 1f)
        {
            Destroy(this.gameObject);
        }

        _volume = _maxVolume[(int)level] * (1f - t);
    }

    private void SetMaxVolume(int level)
    {
        _volume = _maxVolume[level];
    }
}
