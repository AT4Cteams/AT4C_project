using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricade : GimmickBase
{
    public override void OnColEnter(Collision collision)
    {
        //���炷?
        //�j�ЂƂ��o��?

        Destroy(this.gameObject);
    }
}
