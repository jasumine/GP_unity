using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Creature
{
    public LayerMask whatisTarget;
    private Creature target;
    private bool hasTarget
    {
        get
        {
            if(target != null && !target.dead)
                return true;
            else 
                return false;
        }
    }

    private NavMeshAgent pathFinder;


    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
    }

    public override void Die()
    {
        base.Die();
    }

    private void Awake() 
    {
        pathFinder = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        StartCoroutine(UpdatePath());

    }

    //private IEnumerator UpdatePath()
    //{
    //    while(!dead)
    //    {
    //        if(hasTarget)
    //        {
    //            pathFinder.isStopped = false; // 움직인다
    //            pathFinder.SetDestination(target.transform.position); // 타겟의 위치를 목적지로
    //        }
    //        else
    //        {
    //            pathFinder.isStopped = true; // 이동 멈춤
    //            Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatisTarget); 
    //            // 게임오브젝트의 위치에서 반지름20만큼 레이어가 target을 넣은 collider 배열

    //            for(int i = 0;i<colliders.Length;i++)
    //            {
    //                Creature creature = colliders[i].GetComponent<Creature>();

    //                if(creature != null && !creature.dead)
    //                {
    //                    target = creature;
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    yield return new WaitForSeconds(0.25f);
    //}

   
    void Update()
    {
        
    }
}
