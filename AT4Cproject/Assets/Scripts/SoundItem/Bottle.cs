using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bottle : SoundItem
{
    [Header("���̑��x�ȏ�łԂ���Ɗ���܂�")]
    [SerializeField]
    private float _capacitySpeed = 1.0f;

    [SerializeField]
    private GameObject _brokenGlass;

    protected override void OnColEnter(Collision collision)
    {
        if (_rigidbody.velocity.magnitude < _capacitySpeed) return;

        Sound.Generate(_soundVolume, transform.position);

        GameObject glass = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefub/BrokenGlass.prefab");
        Instantiate(glass, transform.position, Quaternion.identity);

        Destroy(this.gameObject);

        PlaySound();

    }
}
