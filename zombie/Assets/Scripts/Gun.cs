using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;

public class Gun : MonoBehaviour
{
    public UiManager uiManager;
    public enum State
    {
        Ready,
        Empty,
        Reloading,
    }

    private State currentState = State.Ready;

    public State CurrentState
    {
        get { return currentState; }
        private set
        {
            currentState = value;
            switch (currentState)
            {
                case State.Ready:
                    break;
                case State.Empty:
                    break;
                case State.Reloading:
                    break;


            }
        }
    }



    public GunData gunData;

    public ParticleSystem muzzleEffect;
    public ParticleSystem shellEffect;

    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public Transform firePosition;

    public int ammoRemain;
    public int magAmmo;

    private float lastFireTime;



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;

    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        lastFireTime = 0f;

        CurrentState = State.Ready;

        uiManager.SetAmmpText(magAmmo,ammoRemain);  
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Ready:
                UpdateReady();
                break;
            case State.Empty:
                UpdatEmpty();
                break;
            case State.Reloading:
                UpdateReloading();
                break;
        }
        //uiManager.SetAmmpText(magAmmo, ammoRemain);
    }

    private void UpdateReady()
    {
        return;
    }
    private void UpdatEmpty()
    {
        return;
    }
    private void UpdateReloading()
    {
        return;
    }

    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {//동기: 멀티안되는친구 /  비동기:멀티가능한친구  3~4가지잇음 

        audioSource.PlayOneShot(gunData.shootClip);

        muzzleEffect.Play();
        shellEffect.Play();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePosition.position);
        Vector3 endPos = hitPosition;
        lineRenderer.SetPosition(1, endPos);

        yield return new WaitForSeconds(0.2f);
        lineRenderer.enabled = false;

    }


    public void Fire()
    {
        if (currentState == State.Ready && Time.time > (lastFireTime + gunData.timeBetFire))
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 hitPosition = Vector3.zero;


        RaycastHit hit;
        if (Physics.Raycast(firePosition.position, firePosition.forward, out hit, gunData.firedistance))
        {
            hitPosition = hit.point;
            //hit.collider.CompareTag(""); //예시
            //hit.collider.GetComponent<Collider>().Monster//예시
            var target = hit.collider.GetComponent<IDamagable>();
            if (target != null)
            {
                
                target.OnDamage(gunData.damage, hitPosition, hit.normal);
            }

        }
        else
        {
            hitPosition = firePosition.position + firePosition.forward * gunData.firedistance;
        }
        StartCoroutine(CoShotEffect(hitPosition));

        --magAmmo;
        uiManager.SetAmmpText(magAmmo, ammoRemain);
        if (magAmmo == 0)
        {
            CurrentState = State.Empty;
        }
        
    }

    public bool Reload()
    {       
        if (CurrentState == State.Reloading || ammoRemain == 0 || magAmmo == gunData.magCapacity)
            return false;

        //CurrentState = State.Reloading;

        StartCoroutine(CoReload());

        return true;
    }

    IEnumerator CoReload()
    {        
          
        CurrentState = State.Reloading;
        audioSource.PlayOneShot(gunData.reloadClip);

        yield return new WaitForSeconds(gunData.reloadTime);

        magAmmo += ammoRemain;
        if (magAmmo >= gunData.magCapacity)
        {
            magAmmo = gunData.magCapacity;
            ammoRemain -= magAmmo;
        }
        else
        {
            ammoRemain = 0;
        }
        uiManager.SetAmmpText(magAmmo, ammoRemain);
        CurrentState = State.Ready;

    }
    public void AddAmmo(int amount)
    {
        ammoRemain = Mathf.Min(magAmmo + amount, gunData.startAmmoRemain);
        uiManager.SetAmmpText(magAmmo, ammoRemain);
    }
}




