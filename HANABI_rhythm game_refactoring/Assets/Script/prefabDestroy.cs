using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class PrefabDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EndPrefab", 0.2f);
    }

    void EndPrefab()
    {
        Destroy(gameObject);
    }
}
