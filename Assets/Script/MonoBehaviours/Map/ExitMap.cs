using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMap : MonoBehaviour
{
    private GameObject map;

    void Start()
    {
        map = this.transform.parent.gameObject;
    }

    
}
