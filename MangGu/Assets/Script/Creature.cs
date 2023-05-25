using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // action��밡��

public class Creature : MonoBehaviour
{

    public float Hp
    {
        get;
        protected set;
    }

    //public int GetHp() { return Hp; }
    //void setHp(int hp) { Hp = hp; }

    public bool dead
    {
        get;
        protected set;
    }

    public event Action onDeath;

    public virtual void OnDamage(int damage)
    {
        Hp -= damage;
        if(Hp<=0 && !dead)
        {
            Die();
        }
    }
    
    public virtual void Die()
    {
        if (onDeath != null)
            onDeath();
        dead = true;
    }

    delegate void MyDelegate();
    MyDelegate myDelegate;

    // delegate(�Լ� ������)�� �����ϰ� ����ó�� ����Ѵ�.
    // ������ �Լ��� ���ϰų� ���� ����� �� �ִ�.(�Լ�ü��)
    //myDelegate = Print;
    //myDelegate += Print2;
    //myDelegate -= Print;
    //myDelegate();
 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Print()
    {
        Debug.Log("!!!");
    }

    private void Print2()
    {
        Debug.Log("hello");
    }

}
