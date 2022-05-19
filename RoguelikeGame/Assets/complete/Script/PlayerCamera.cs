using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    public GameObject target; // 카메라가 따라갈 대상 
    public float moveSpeed; // 카메라 이동속도 
    private Vector3 targetPosition; // 대상의 현재 위치

    private void Update()
    {   // 대상이 있는지 확인 
        if (target.gameObject != null)
        {
            // this는 카메라 
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);
            // 카메라 이동 
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
