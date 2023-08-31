using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(PlayerControl.instance.transform.position, -Vector3.forward);
    }
}
