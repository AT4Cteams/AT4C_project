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

    protected Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) OnFloorEnter(collision);
        else if (collision.gameObject.CompareTag("Wall")) OnWallEnter(collision);
        else if (collision.gameObject.CompareTag("Enemy")) OnEnemyEnter(collision);
        else if (collision.gameObject.CompareTag("Player")) OnPlayerEnter(collision);

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall")) OnColEnter(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) OnFloorStay(collision);
        else if (collision.gameObject.CompareTag("Wall")) OnWallStay(collision);
        else if (collision.gameObject.CompareTag("Enemy")) OnEnemyStay(collision);
        else if (collision.gameObject.CompareTag("Player")) OnPlayerStay(collision);

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall")) OnColStay(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) OnFloorExit(collision);
        else if (collision.gameObject.CompareTag("Wall")) OnWallExit(collision);
        else if (collision.gameObject.CompareTag("Enemy")) OnEnemyExit(collision);
        else if (collision.gameObject.CompareTag("Player")) OnPlayerExit(collision);

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall")) OnColExit(collision);
    }

    protected virtual void OnColEnter(Collision collision) { }
    protected virtual void OnColStay(Collision collision) { }
    protected virtual void OnColExit(Collision collision) { }

    protected virtual void OnFloorEnter(Collision collision) { }
    protected virtual void OnFloorStay(Collision collision) { }
    protected virtual void OnFloorExit(Collision collision) { }

    protected virtual void OnWallEnter(Collision collision) { }
    protected virtual void OnWallStay(Collision collision) { }
    protected virtual void OnWallExit(Collision collision) { }

    protected virtual void OnEnemyEnter(Collision collision) { }
    protected virtual void OnEnemyStay(Collision collision) { }
    protected virtual void OnEnemyExit(Collision collision) { }

    protected virtual void OnPlayerEnter(Collision collision) { }
    protected virtual void OnPlayerStay(Collision collision) { }
    protected virtual void OnPlayerExit(Collision collision) { }

    protected void PlaySound()
    {
        if (_stopSeconds == 0f) return;
        StartCoroutine(playSound());
    }

    private IEnumerator playSound()
    {
        SoundManager.Instance.Play(_soundName);

        yield return new WaitForSeconds(_stopSeconds);

        SoundManager.Instance.Stop(_soundName);

        Destroy(gameObject);
    }

    protected void Bounce(Collision collision)
    {
        var direction = _rigidbody.velocity;

        var normal = collision.contacts[0].normal;

        Vector3 result = Vector3.Reflect(direction, normal);

        GetComponent<Rigidbody>().velocity = result * 1.2f;
    }

    protected virtual void Broken() { Destroy(this.gameObject); }
}
