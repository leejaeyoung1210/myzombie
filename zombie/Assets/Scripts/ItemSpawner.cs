using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;

    public float minTime = 2f;
    public float maxTime = 8f;
    public float spawnTime;
    public float respawn;

    public float itemlife = 10f;

 

    private void Update()
    {      
        spawnTime += Time.deltaTime;
        if(spawnTime> respawn)
        {
            Spawnitem();
            spawnTime = 0f;
        }

        if (spawnTime > itemlife)
        { 

        }
    }

    private void Awake()
    {
        
        respawn = Random.Range(minTime, maxTime);
    }

    private void Spawnitem()
    {


        respawn = Random.Range(minTime, maxTime);
    }


}
