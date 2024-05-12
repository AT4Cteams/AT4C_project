using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    private float _soundVolume;

    public AudioSource sound;
    public float stopSeconds;
    public string soundName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall"))
        {
            float value = this.GetComponent<Rigidbody>().velocity.magnitude;
            float maxValue = 30f;

            Sound.AutoAdjustGenerate(value, maxValue, transform.position, _soundVolume);

            //StartCoroutine(playSound());

            var direction = GetComponent<Rigidbody>().velocity;

            var normal = collision.contacts[0].normal;

            Vector3 result = Vector3.Reflect(direction, normal);

            GetComponent<Rigidbody>().velocity = result;
        }
    }

    private IEnumerator playSound()
    {
        SoundManager.Instance.Play(soundName);

        yield return new WaitForSeconds(stopSeconds);

        SoundManager.Instance.Stop(soundName);

        Destroy(gameObject);
    }
}
