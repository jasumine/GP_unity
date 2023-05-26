using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,      // 발사 준비됨.
        Empty,      // 탄창이 빔
        Reloading   // 재장전
    }
    
    public State state { get; private set; }

    public int damage = 3;

    public int ammoRemain = 100; // 전체 총알
    public int magCapacity = 30; // 최대 탄창용량
    public int magAmmo; // 탄창에 장전된 총알 갯수

    public float firerate = 0.1f; // 탄알 발사 간격
    public float reloadTime = 1.8f; // 재장전 시간
    private float lastFireTime; // 총을 마지막으로 발사한 시점.

    
    public void Fire()
    {
        if(state == State.Ready && Time.time>= lastFireTime + firerate)
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }

    private void Shoot()
    {
        magAmmo--;
        if(magAmmo<0)
        {
            state = State.Empty;
            Reload();
        }
    }
    
    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain < 0 || magAmmo >= magCapacity)
            return false;


        StartCoroutine(ReloadRoutine());
        return true;
    }

    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;

        yield return new WaitForSeconds(reloadTime);

        int ammoToFill = magCapacity - magAmmo;

        if (ammoRemain < ammoToFill)
            ammoToFill = ammoRemain; // 채워야 할 양보다 남은 양이 적을경우 남은총알의 수로 바꿔준다.

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.Ready;
    }

    void OnEnable()
    {
        magAmmo = ammoRemain;
        state = State.Ready;
        lastFireTime = 0;
    }

    void Update()
    {
        
    }
}
