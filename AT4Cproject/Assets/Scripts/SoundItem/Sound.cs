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
    private bool _isVisible = true;
    private bool _footSoundMode = false;

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

        if(!_isVisible)
            _mesh.material.color = new UnityEngine.Color(0f, 0f, 0f, 0f);
    }

    void Update()
    {
        // 徐々に音の聞こえる範囲が広がり、音量は減少
        Expention();
    }

    public static void Generate(SoundLevel soundLevel, Vector3 position)
    {
        //GameObject soundPrefub = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");
        GameObject soundPrefub = LoadPrefubData.instance._sound;

        GameObject newSound = Instantiate(soundPrefub, position, Quaternion.identity);

        float value = (float)((int)soundLevel + 1) * 20f;

        Sound sound = newSound.GetComponent<Sound>();
        sound.SetVolume(value);
    }

    public static void Generate(float volume, Vector3 position)
    {
        //GameObject soundPrefub = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");
        GameObject soundPrefub = LoadPrefubData.instance._sound;

        GameObject newSound = Instantiate(soundPrefub, position, Quaternion.identity);

        Sound sound = newSound.GetComponent<Sound>();
        sound.SetVolume(volume);
    }

    public static GameObject AutoAdjustGenerate(float currentValue, float maxValue, Vector3 position, float maxSoundVolume)
    {
        //GameObject soundPrefub = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");
        GameObject soundPrefub = LoadPrefubData.instance._sound;

        GameObject newSound = Instantiate(soundPrefub, position, Quaternion.identity);

        float rate = currentValue / maxValue;

        float volume = maxSoundVolume * rate;

        newSound.GetComponent<Sound>().SetVolume(volume);

        return newSound;
    }

    public static void AutoAdjustGenerate(float currentValue, float maxValue, Vector3 position, float maxSoundVolume, bool visible)
    {
        GameObject newSound = AutoAdjustGenerate(currentValue, maxValue, position, maxSoundVolume);
        if(!visible)
            newSound.GetComponent<Sound>().NoVisible();
    }

    public static void AutoAdjustGenerate(float currentValue, float maxValue, Vector3 position, float maxSoundVolume, bool visible, bool footSoundMode)
    {
        GameObject newSound = AutoAdjustGenerate(currentValue, maxValue, position, maxSoundVolume);
        if(!visible)
            newSound.GetComponent<Sound>().NoVisible();
        newSound.GetComponent<Sound>()._footSoundMode = footSoundMode;

    }

    private void Expention()
    {
        Vector3 targetScale = new Vector3(_maxVolume, _maxVolume, _maxVolume);
        if (_footSoundMode)
        {
            targetScale = new Vector3(_maxVolume, 0.01f, _maxVolume);
        }

        float elapsed = Time.time - _startTime;
        float t = Mathf.Clamp01(elapsed / _finishTime);
        transform.localScale = Vector3.Lerp(_startSize, targetScale, t);

        if (t >= 1f)
        {
            Destroy(this.gameObject);
        }

        _volume = _maxVolume * (1f - t);

        if(_isVisible)
        {
            float alpha = _alpha * (1f - t);
            if (!_footSoundMode)
            {
                _mesh.material.color = new UnityEngine.Color(_color.r, _color.g, _color.b, alpha);
            }
            else
            {
                _mesh.material.color = new UnityEngine.Color(_color.r, _color.g, _color.b, alpha / 5);
            }
        }
    }

    private void SetVolume(float value)
    {
        _volume = value;
        _maxVolume = _volume;
    }

    private void NoVisible()
    {
        _isVisible = false;
    }
}
