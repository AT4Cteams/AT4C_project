using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquakyFloor : MonoBehaviour
{
    [SerializeField] private GameObject player;     // プレイヤーのゲームオブジェクト
    Rigidbody rbPlayer;     // プレイヤーのリジッドボディ

    void Start()
    {
        rbPlayer = player.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーの移動速度によってきしむ音量が変化
            float playerMag = rbPlayer.velocity.magnitude;
            if (playerMag >= 1)
            {
                Sound.Generate(playerMag, collision.transform.position);
            }
        }
    }
}
