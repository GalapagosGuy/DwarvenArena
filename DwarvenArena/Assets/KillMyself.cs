using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMyself : MonoBehaviour
{
    public void OnClick()
    {
        Destroy(this, .1f);
    }
}
