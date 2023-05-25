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
    //            pathFinder.isStopped = false; // �����δ�
    //            pathFinder.SetDestination(target.transform.position); // Ÿ���� ��ġ�� ��������
    //        }
    //        else
    //        {
    //            pathFinder.isStopped = true; // �̵� ����
    //            Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatisTarget); 
    //            // ���ӿ�����Ʈ�� ��ġ���� ������20��ŭ ���̾ target�� ���� collider �迭

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
