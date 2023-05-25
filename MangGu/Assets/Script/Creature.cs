using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // action사용가능

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

    // delegate(함수 포인터)를 선언하고 변수처럼 사용한다.
    // 변수에 함수를 더하거나 빼서 사용할 수 있다.(함수체인)
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
