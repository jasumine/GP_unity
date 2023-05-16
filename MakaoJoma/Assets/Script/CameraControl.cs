using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public CarController target;
    private Vector3 offset;

    public float minDistance, maxDistance;
    private float activeDistance;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.transform.position;
        activeDistance = minDistance;
        offset.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        activeDistance = minDistance + 
            ((maxDistance - minDistance) * (target.RB.velocity.magnitude / target.Maxspeed)); // �ӵ��� Ŀ���� �� �ָ������Եȴ�.
        transform.position = target.transform.position + (offset * activeDistance);
    }
}
