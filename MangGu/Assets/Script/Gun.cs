using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,      // �߻� �غ��.
        Empty,      // źâ�� ��
        Reloading   // ������
    }
    
    public State state { get; private set; }

    public int damage = 3;

    public int ammoRemain = 100; // ��ü �Ѿ�
    public int magCapacity = 30; // �ִ� źâ�뷮
    public int magAmmo; // źâ�� ������ �Ѿ� ����

    public float firerate = 0.1f; // ź�� �߻� ����
    public float reloadTime = 1.8f; // ������ �ð�
    private float lastFireTime; // ���� ���������� �߻��� ����.

    
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
            ammoToFill = ammoRemain; // ä���� �� �纸�� ���� ���� ������� �����Ѿ��� ���� �ٲ��ش�.

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
