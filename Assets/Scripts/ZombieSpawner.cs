using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    public UiManager uiManager;

    public Zombie prefab;

    public ZombieData[] zombieDatas;
    public Transform[] spwanPoints;

    private List<Zombie> zombies = new List<Zombie>();

    private int wave;

    private void Update()
    {
        if(zombies.Count == 0)
        {
            SpawnWave();
        }
        uiManager.SetWaverInfo(wave,zombies.Count);
    }

    private void SpawnWave()
    {
        wave++;
        int count = Mathf.RoundToInt(wave * 1.5f);
        for (int i = 0; i < count; i++)
        {
            CreateZombie();
        }
    }


    public void CreateZombie()
    {
        var point = spwanPoints[Random.Range(0, spwanPoints.Length)];   
        var zombie = Instantiate(prefab,point.position,point.rotation);
        zombie.Setup(zombieDatas[Random.Range(0, zombieDatas.Length)]);

        zombies.Add(zombie);

        //Ä¸ÃÄ
        zombie.OnDeath += () => zombies.Remove(zombie);
        zombie.OnDeath += () => uiManager.SetWaverInfo(wave, zombies.Count);
        zombie.OnDeath += () => Destroy(zombie.gameObject,5f);
    }
}
