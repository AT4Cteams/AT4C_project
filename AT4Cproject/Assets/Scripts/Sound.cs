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

    [SerializeField]
    private static float _originalSize = 40;

    [SerializeField]
    private float[] _volume = new float[5];

    [HideInInspector]
    public SoundLevel level;

    [SerializeField]
    private Material[] _material = new Material[5];

    [SerializeField]
    private GameObject _areaObject;

    public bool isHit { get; set; }
    public float volume { get { return _volume[(int)level]; }}

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Finish", .3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Finish()
    {
        Destroy(this.gameObject);
    }

    private void SetLevel(SoundLevel soundLevel)
    {
        level = soundLevel;

        _areaObject.GetComponent<MeshRenderer>().material = _material[(int)level];
    }

    public static void Generate(int soundLevel, Vector3 position)
    {
        GameObject newSound = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");

        float size = _originalSize * (soundLevel + 1);

        newSound.transform.localScale =  new Vector3(size, 0.1f, size);

        newSound.GetComponent<Sound>().SetLevel((SoundLevel)soundLevel);

        Instantiate(newSound, position, Quaternion.identity);
    }

    public static void Generate(SoundLevel soundLevel, Vector3 position)
    {
        GameObject newSound = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");

        float size = _originalSize * ((int)soundLevel + 1);

        newSound.transform.localScale = new Vector3(size, 0.1f, size);

        newSound.GetComponent<Sound>().SetLevel(soundLevel);

        Instantiate(newSound, position, Quaternion.identity);
    }
}
