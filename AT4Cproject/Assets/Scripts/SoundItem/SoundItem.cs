using UnityEngine;
using System.Collections;

abstract public class SoundItem : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    protected float _soundVolume;

    [SerializeField]
    protected float _stopSeconds;

    [SerializeField]
    protected string _soundName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall"))
        {
            //Sound.Generate((SoundLevel)_soundLevel, transform.position);
            //Sound.VelocityToGenerate(this.gameObject);

            float velocity = this.GetComponent<Rigidbody>().velocity.magnitude;
            float maxVelocity = 30f;

            Sound.AutoAdjustGenerate(velocity, maxVelocity, transform.position, _soundVolume, false);

        }
    }

    private void PlaySound()
    {
        StartCoroutine(playSound());
    }

    private IEnumerator playSound()
    {
        SoundManager.Instance.Play(_soundName);

        yield return new WaitForSeconds(_stopSeconds);

        SoundManager.Instance.Stop(_soundName);

        Destroy(gameObject);
    }
}
