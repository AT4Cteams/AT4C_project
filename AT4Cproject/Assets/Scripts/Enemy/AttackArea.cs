/**
 * @brief エネミーの攻撃判定範囲
 * @author 村上
 * 
 * @details 当たっている間だけisHitにtrueを返す
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [HideInInspector]
    public bool isHit;

    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isHit = false;
        }
    }
}
