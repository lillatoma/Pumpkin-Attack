using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPrecall : MonoBehaviour
{
    public GameObject explosionPrefab;

    public void EndPrecall()
    {
        GameObject go = Instantiate(explosionPrefab);
        go.transform.position = transform.position;
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
