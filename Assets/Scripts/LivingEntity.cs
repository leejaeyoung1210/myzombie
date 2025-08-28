using System;
using Unity.Services.Analytics;
using UnityEngine;

public class LivingEntity : MonoBehaviour,IDamagable
{
    public float MaxHealth = 100f;

    public float Health {  get; protected set; }  
    public bool IsDead { get; private set; }

    public event Action OnDeath;

   protected virtual void OnEnable()
    {
        IsDead = false;
        Health = MaxHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 h)
    {
     Health -= damage;  
        
        if(Health <= 0 && !IsDead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if(OnDeath != null)
        {
            OnDeath();  
        }       
        IsDead = true;
    }

    
}
