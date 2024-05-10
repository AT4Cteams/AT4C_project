using UnityEngine;
using System.Collections;

public class SoundItem : MonoBehaviour
{
    public AudioSource sound;
    public float stopSeconds;
    public string soundName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            StartCoroutine(playSound());
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
