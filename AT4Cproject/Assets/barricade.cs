using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricade : GimmickBase
{
    public override void OnColEnter(Collision collision)
    {
        //âπñ¬ÇÁÇ∑?
        //îjï–Ç∆Ç©èoÇ∑?

        Destroy(this.gameObject);
    }
}
