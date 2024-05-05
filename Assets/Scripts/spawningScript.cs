using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawningScript : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    int r = 0;
    //Random rnd = new Random();
    // Update is called once per frame
    void Update()
    {
        
        while(r < 10){
        Instantiate(spawnObject, new Vector3(x + Random.Range(-10f, 10f), y , z + Random.Range(-10f, 10f)), Quaternion.identity);
        r++;
        }
    }
    
}
