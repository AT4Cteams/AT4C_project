/**
 * @brief 音クラス
 * @author 村上
 * 
 * @details Sound.Generate(Sound.SoundLevel.レベル数, 生成するポジション); ←で生成することができます。 @n
 *          音の聞こえる範囲などは後々変更が必要
 */

using System.Collections;
using System.Collections.Generic;
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
    [HideInInspector]
    public SoundLevel level;

    [SerializeField]
    private Material[] _material = new Material[5];

    [SerializeField]
    private GameObject _areaObject;

    public bool isHit { get; set; }

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

        newSound.GetComponent<Sound>().SetLevel((SoundLevel)soundLevel);

        Instantiate(newSound, position, Quaternion.identity);
    }

    public static void Generate(SoundLevel soundLevel, Vector3 position)
    {
        GameObject newSound = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/Sound.prefab");

        newSound.GetComponent<Sound>().SetLevel(soundLevel);

        Instantiate(newSound, position, Quaternion.identity);
    }
}
