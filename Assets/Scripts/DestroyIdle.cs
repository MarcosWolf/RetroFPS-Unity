using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIdle : MonoBehaviour
{
    public float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
