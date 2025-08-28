using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public float maxHp = 100;
    public float damage = 20;
    public float speed = 8;
    public Color color = Color.white;   
}
