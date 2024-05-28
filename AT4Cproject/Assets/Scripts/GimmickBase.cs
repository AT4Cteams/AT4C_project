using UnityEngine;

abstract public class GimmickBase : MonoBehaviour
{
    [SerializeField] [Tag] protected string[] _collisionTagName;


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

    ////掴めるオブジェクトかどうかを判別するbool
    //private bool isCanGrabObject(GameObject gameObject)
    //{
    //    return gameObject.TryGetComponent<Outline>(out Outline component) && gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb);
    //}



    ////ドアノブかどうかを判定するbool
    //private bool isNob(GameObject gameObject, bool nullCheck = true)
    //{
    //    if (nullCheck) return gameObject != null && (gameObject.CompareTag("doornob") || gameObject.CompareTag("doornob2"));
    //    else return gameObject.CompareTag("doornob") || gameObject.CompareTag("doornob2");
    //}

    ////物を掴む関数
    //private void Grab(GameObject gameObject, Transform hand)
    //{
    //    gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //    gameObject.transform.position = hand.position;
    //    gameObject.transform.SetParent(hand);
    //    gameObject.transform.localRotation = Quaternion.identity;
    //}

    ////物を手放す関数
    //private void Release(GameObject gameObject, Vector3 originScale)
    //{
    //    if (gameObject != null)
    //    {
    //        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
    //        {
    //            gameObject.GetComponent<Rigidbody>().isKinematic = false;
    //            gameObject.transform.SetParent(null);
    //            gameObject = null;
    //            _isGrabNob = false;
    //        }
    //    }
    //}

    ////物を投げる関数
    //private void Throw(GameObject gameObject)
    //{
    //    if (gameObject != null)
    //    {
    //        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
    //        {
    //            gameObject.GetComponent<Rigidbody>().isKinematic = false;
    //            gameObject.transform.SetParent(null);
    //            gameObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward.normalized * _shootPower);
    //            gameObject = null;
    //            _isGrabNob = false;
    //        }
    //    }
    //}

    public virtual void PressedL2() { }
    public virtual void PressedR2() { }
    public virtual void PressedL1() { }
    public virtual void PressedR1() { }

    public virtual void OnColEnter(Collision collision) { }
    public virtual void OnColStay(Collision collision) { }
    public virtual void OnColExit(Collision collision) { }

    public virtual void OnTriEnter(Collider collision) { }
    public virtual void OnTriStay(Collider collision) { }
    public virtual void OnTriExit(Collider collision) { }
}