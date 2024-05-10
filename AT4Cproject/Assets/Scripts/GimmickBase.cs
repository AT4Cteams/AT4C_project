using UnityEngine;

abstract public class GimmickBase : MonoBehaviour
{
    [SerializeField] [Tag] protected string[] _collisionTagName;
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected Vector2 _raySize;
    [SerializeField] protected float groundCheckOffsetY;
    [SerializeField] protected bool _noRotationLock;



    void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < _collisionTagName.Length; i++)
        {
            if (collision.gameObject.CompareTag(_collisionTagName[i])) OnColEnter(collision);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < _collisionTagName.Length; i++)
        {
            if (collision.gameObject.CompareTag(_collisionTagName[i])) OnColStay(collision);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        for (int i = 0; i < _collisionTagName.Length; i++)
        {
            if (collision.gameObject.CompareTag(_collisionTagName[i])) OnColExit(collision);
        }
    }


    void OnTriggerEnter(Collider collision)
    {
        for (int i = 0; i < _collisionTagName.Length; i++)
        {
            if (collision.gameObject.CompareTag(_collisionTagName[i])) OnTriEnter(collision);
        }
    }

    void OnTriggerStay(Collider collision)
    {
        for (int i = 0; i < _collisionTagName.Length; i++)
        {
            if (collision.gameObject.CompareTag(_collisionTagName[i])) OnTriStay(collision);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        for (int i = 0; i < _collisionTagName.Length; i++)
        {
            if (collision.gameObject.CompareTag(_collisionTagName[i])) OnTriExit(collision);
        }
    }


    public virtual void OnColEnter(Collision collision) { }
    public virtual void OnColStay(Collision collision) { }
    public virtual void OnColExit(Collision collision) { }

    public virtual void OnTriEnter(Collider collision) { }
    public virtual void OnTriStay(Collider collision) { }
    public virtual void OnTriExit(Collider collision) { }
}