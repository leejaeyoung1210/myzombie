using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    private Status currentStatus;

    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var prevStatus = currentStatus;
            currentStatus = value;

            switch (currentStatus)
            {
                case Status.Idle:
                    animator.SetBool("HasTarget", false);
                    agent.isStopped = true; // 이동멈춤
                    break;
                case Status.Trace:
                    animator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    animator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    animator.SetTrigger("Die");
                    agent.isStopped = true;
                    //zomvieCollider.enabled = flase;

                    break;
                default:
                    break;
            }

        }
    }

    public Transform target;

    public ParticleSystem bloodSplatParticle;


    public float traceDistance;
    public float attackDistance;


    public float damage = 10f;
    public float lastAttackTime;
    public float attackInterval = 0.5f;

    private Animator animator;
    private AudioSource audioSource;
    public AudioClip dieClip;
    public AudioClip hitClip;

    private NavMeshAgent agent;
    private Collider zomvieCollider;

    public Renderer zombierenderer;

    public ZombieData data;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        zomvieCollider = GetComponent<Collider>();

        Setup(data);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        zomvieCollider.enabled = true;
        CurrentStatus = Status.Idle;
    }

    public void Setup(ZombieData data)
    {
        MaxHealth = data.maxHp;
        damage = data.damage;   
        agent.speed = data.speed;

        //이렇게할경우 복제본이 생성//테스트용임 / 메테리얼을 여러개 만들어서 교체하는형식으로 해야함
        zombierenderer.material.color = data.color; 
    }

    private void Update()
    {        
        switch (currentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
            default:
                break;
        }

    }

    private void UpdateDie()
    {
        return;
    }

    private void UpdateAttack()
    {
        if (target == null ||
          (target != null &&
         Vector3.Distance(transform.position, target.position) > attackDistance))
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookat = target.position;
        lookat.y = transform.position.y;
        transform.LookAt(lookat);

        if (lastAttackTime + attackInterval < Time.time)
        {
            lastAttackTime = Time.time;
            var damagalbe = target.GetComponent<IDamagable>();
            if (damagalbe != null)
            {
                damagalbe.OnDamage(damage, transform.position, -transform.forward);
            }
        }

    }

    private void UpdateTrace()
    {
        if (target != null &&
           Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }

        if (target == null ||
             Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        agent.SetDestination(target.position);
    }



    private void UpdateIdle()
    {
        if (target != null &&
            Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        target = FindTarget(traceDistance);
    }


    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 h)
    {
        base.OnDamage(damage, hitPosition, h);

        audioSource.PlayOneShot(hitClip);
        bloodSplatParticle.transform.position = hitPosition;
        bloodSplatParticle.transform.forward = h;

        bloodSplatParticle.Play();

    }

    protected override void Die()
    {
        base.Die();
        CurrentStatus = Status.Die;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        audioSource.PlayOneShot(dieClip);

    }

    public LayerMask targetLayer;
    protected Transform FindTarget(float radius)
    {//콜라이더 배열 반환
        var colliders = Physics.OverlapSphere(transform.position, radius, targetLayer.value);
        if (colliders.Length == 0)
        {
            return null;
        }

        // 콜라이더를 Distande(기준) 오름차순
        var target = colliders.OrderBy(
            x => Vector3.Distance(x.transform.position, transform.position)).First();


        return target.transform;

    }



}
