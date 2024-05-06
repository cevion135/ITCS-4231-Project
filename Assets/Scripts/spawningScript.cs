using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawningScript : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] GameObject thisSpawner;
   // [SerializeField] Vector3 spawnerLocation;

    // Start is called before the first frame update
    void Start()
    {
       
    }
    int r = 0;

    void Update()
    {

        //Vector3 offset = new Vector3( Random.Range(-10f, 10f), 0 , Random.Range(-10f, 10f));
        while(r < 10){
            Vector3 offset = new Vector3( Random.Range(-15f, 15f), 0 , Random.Range(-15f, 15f));
        Instantiate(spawnObject, (thisSpawner.transform.position + offset), Quaternion.identity);
        r++;
        }
    }
    
}
