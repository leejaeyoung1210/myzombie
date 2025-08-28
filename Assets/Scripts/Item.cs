using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Types
    {
        Coin,
        Ammo,
        Health,
    }

    public Types itemType;
    public int value;
    public void Use(GameObject other)
    {
        switch (itemType)
        {
            case Types.Coin:
                {
                    Debug.Log("coin");
                }
                break;
            case Types.Ammo:
                {
                    var shooter = other.GetComponent<PlayerShooter>();
                    shooter?.gun?.AddAmmo(value);
                }
                break;
            case Types.Health:
                {
                    var playerHealth = other.GetComponent<PlayerHealth>();
                    playerHealth?.Heal(value);
                }
                break;
            default:
                break;
                        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Use(other.gameObject);
            Destroy(gameObject);
        }
    }
}


