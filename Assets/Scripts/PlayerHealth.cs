using NUnit.Framework.Constraints;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;

    public AudioClip hitClip;
    public AudioClip deathClip;
    public AudioClip ItemPickUpClip;
    private AudioSource audioSource;
    private Animator animator;

    private PlayerMovement movement;
    private PlayerShooter shooter;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();  
        shooter = GetComponent<PlayerShooter>();    
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.value = Health/MaxHealth;

        movement.enabled = true;
        shooter.enabled = true; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDamage(10f, Vector3.zero, Vector3.zero);
        }
    }

    public override void OnDamage(float damage,Vector3 hitpoint,Vector3 hitNormal )
    {
        if(IsDead) return;  

        base.OnDamage(damage, hitpoint, hitNormal);
        healthSlider.value = Health/MaxHealth;    
        audioSource.PlayOneShot(hitClip);      
    }

    protected override void Die()
    {
        base.Die();
        healthSlider.gameObject.SetActive(false);
        animator.SetTrigger("Die");
        audioSource.PlayOneShot(deathClip); 

        movement.enabled = false;
        shooter.enabled=false;  
    }

    public void Heal(int ammount)
    {
        Health = Mathf.Min(Health + (float)ammount, MaxHealth);
        healthSlider.value = Health / MaxHealth;
    }
}
