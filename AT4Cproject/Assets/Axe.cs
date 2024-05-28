using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : GimmickBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PressedL1()
    {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.SetParent(null);
            HorrorPlayer.player.grabObjL = null;
    }
}
