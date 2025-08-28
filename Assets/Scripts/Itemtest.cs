using UnityEngine;

public class Itemtest : MonoBehaviour
{
    public enum ItemType
    {
        Coin,
        Ammo,
        Heart,
    }
    public ItemType Type { get; private set; }

    private Rigidbody typebody;

    private void Start()
    {
        typebody = GetComponent<Rigidbody>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            switch (Type)
            {
                case ItemType.Coin:
                    break;
                case ItemType.Ammo:
                    break;
                case ItemType.Heart:
                    break;
                default:
                    break;
            }
        }
    }

}
