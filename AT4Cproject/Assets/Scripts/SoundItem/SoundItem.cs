using UnityEngine;
using System.Collections;

public class SoundItem : MonoBehaviour
{
    [SerializeField]
    [Range(0, 5)]
    private int _soundLevel;

    public AudioSource sound;
    public float stopSeconds;
    public string soundName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall")
        {
            //Sound.Generate((SoundLevel)_soundLevel, transform.position);
            Sound.VelocityToGenerate(this.gameObject);

            //StartCoroutine(playSound());
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
