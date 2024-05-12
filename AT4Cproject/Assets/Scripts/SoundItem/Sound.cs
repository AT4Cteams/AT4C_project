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
    private float _finishTime;
    private float _startTime = 0f;

    private float _volume;
    private float _maxVolume;

    [SerializeField]
    private GameObject _areaObject;
    private MeshRenderer _mesh;
    private UnityEngine.Color _color;
    private float _alpha;

    public bool isHit { get; set; }
    public float volume { get { return _volume; }}

    void Start()
    {
        _startSize = transform.localScale;
        _finishTime = 0.5f;
        _startTime = Time.time;

        _mesh = _areaObject.GetComponent<MeshRenderer>();
        _color = _mesh.material.color;
        _alpha = _color.a;
    }

    void Update()
    {
        // 徐々に音の聞こえる範囲が広がり、音量は減少
        Expention();
    }

    public static void Generate(SoundLevel soundLevel, Vector3 position)
    {
        GameObject soundPrefub = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");

        GameObject newSound = Instantiate(soundPrefub, position, Quaternion.identity);

        float value = (float)((int)soundLevel + 1) * 20f;

        Sound sound = newSound.GetComponent<Sound>();
        sound.SetVolume(value);
    }

    public static void Generate(float volume, Vector3 position)
    {
        GameObject soundPrefub = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");

        GameObject newSound = Instantiate(soundPrefub, position, Quaternion.identity);

        Sound sound = newSound.GetComponent<Sound>();
        sound.SetVolume(volume);
    }

    public static GameObject AutoAdjustGenerate(float currentValue, float maxValue, Vector3 position, float maxSoundVolume)
    {
        GameObject soundPrefub = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");

        GameObject newSound = Instantiate(soundPrefub, position, Quaternion.identity);

        float rate = currentValue / maxValue;

        float volume = maxSoundVolume * rate;

        newSound.GetComponent<Sound>().SetVolume(volume);

        return newSound;
    }

    public static void AutoAdjustGenerate(float currentValue, float maxValue, Vector3 position, float maxSoundVolume, bool show)
    {
        GameObject newSound = AutoAdjustGenerate(currentValue, maxValue, position, maxSoundVolume);
        newSound.GetComponent<MeshRenderer>().enabled = show;
    }

    private void Expention()
    {
        Vector3 targetScale = new Vector3(_maxVolume, _maxVolume, _maxVolume);

        float elapsed = Time.time - _startTime;
        float t = Mathf.Clamp01(elapsed / _finishTime);
        transform.localScale = Vector3.Lerp(_startSize, targetScale, t);

        if (t >= 1f)
        {
            Destroy(this.gameObject);
        }

        _volume = _maxVolume * (1f - t);

        float alpha = _alpha * (1f - t);
        _mesh.material.color = new UnityEngine.Color(_color.r, _color.g, _color.b, alpha);
    }

    private void SetVolume(float value)
    {
        _volume = value;
        _maxVolume = _volume;
    }
}
