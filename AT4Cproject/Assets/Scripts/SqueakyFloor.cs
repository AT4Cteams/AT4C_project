using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquakyFloor : MonoBehaviour
{
    [SerializeField] private GameObject player;     // �v���C���[�̃Q�[���I�u�W�F�N�g
    Rigidbody rbPlayer;     // �v���C���[�̃��W�b�h�{�f�B

    void Start()
    {
        rbPlayer = player.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[�̈ړ����x�ɂ���Ă����މ��ʂ��ω�
            float playerMag = rbPlayer.velocity.magnitude;
            if (playerMag >= 1)
            {
                Sound.Generate(playerMag, collision.transform.position);
            }
        }
    }
}
