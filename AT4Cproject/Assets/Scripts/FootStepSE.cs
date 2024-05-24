using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class FootstepSE : MonoBehaviour
{
    [System.Serializable]
    public class AudioClips
    {
        public string groundTypeTag; // �n�ʂ̎�ނ�\���^�O
        public AudioClip[] audioClips; // �Ή�����I�[�f�B�I�N���b�v�̔z��
    }

    [SerializeField] List<AudioClips> listAudioClips = new List<AudioClips>(); // �����̎�ނ��ƂɃ^�O�ƃI�[�f�B�I�N���b�v��o�^���郊�X�g
    [SerializeField] float pitchRange = 0.1f; // �s�b�`�͈̔�

    private Dictionary<string, int> tagToIndex = new Dictionary<string, int>(); // �^�O���C���f�b�N�X�ɕϊ����鎫��
    private int groundIndex = 0; // ���݂̒n�ʂ̃C���f�b�N�X

    private AudioSource source; // �I�[�f�B�I�\�[�X

    private void Awake()
    {
        // �ŏ��ɃA�^�b�`���ꂽ�I�[�f�B�I�\�[�X���g�p����
        source = GetComponents<AudioSource>()[0];

        // �^�O���C���f�b�N�X�ɕϊ����鎫��������������
        for (int i = 0; i < listAudioClips.Count; ++i)
        {
            tagToIndex.Add(listAudioClips[i].groundTypeTag, i);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �Փ˂����I�u�W�F�N�g�̃^�O���m�F���A�Ή�����C���f�b�N�X��ݒ肷��
        if (tagToIndex.ContainsKey(collision.gameObject.tag))
        {
            if (collision.gameObject.tag != "Untagged")
            {
                groundIndex = tagToIndex[collision.gameObject.tag];
            }
        }

        Debug.Log("�n�ʂ̃^�O: " + collision.gameObject.tag);
    }

    public void PlayFootstepSE()
    {
        // ���݂̒n�ʃ^�C�v�ɑΉ�����I�[�f�B�I�N���b�v���擾����
        AudioClip[] clips = listAudioClips[groundIndex].audioClips;

        // �w�肳�ꂽ�͈͂Ńs�b�`�������_���ɐݒ肷��
        source.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);

        // ���݂̒n�ʃ^�C�v�̃I�[�f�B�I�N���b�v���烉���_���ɑI��ōĐ�����
        AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot(clipToPlay);

        Debug.Log("�Đ�����N���b�v: " + clipToPlay.name);
    }
}
