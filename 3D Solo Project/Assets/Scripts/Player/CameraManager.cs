using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform targetTr;//따라갈 목표
    [SerializeField] Transform pivotCamTr;//수직 카메라 위치
    [SerializeField] LayerMask layerMask;//레이케스트에 부딫힐 레이어
    Transform cam; //메인카메라 위치
    private Vector2 camRoDir;//카메라 회전 방향
    private Vector3 cameraVector;
    private float _lookAngle;//좌,우 앵글 값
    private float _pivotAngle;//상,하 앵글 값
    private float _camRoSpeed;//카메라 회전속도
    private float _maxAngle;//최대 상,하 앵글 값
    private float _minAngle;//최소 상,하 앵글 값
    private float _camOffSet;//가까워질 카메라값
    private float _defaltPos;

    private void Awake()
    {
        cam = Camera.main.transform;
        _camRoSpeed = 2f;
        _maxAngle = 90f;
        _minAngle = -70f;
        _defaltPos = cam.localPosition.z;
    }

    private void LateUpdate()
    {
        CameraFallowTarget();
        CameraRotation();
        CameraCollision();
    }

    public Vector2 CamRoDir { get => camRoDir; set => camRoDir = value; }

    //카메라가 캐릭터 따라가게함
    private void CameraFallowTarget()
    {
        transform.position = targetTr.position;
    }

    //카메라 회전
    private void CameraRotation()
    {
        _lookAngle += camRoDir.x * _camRoSpeed;
        _pivotAngle -= camRoDir.y * _camRoSpeed;
        _pivotAngle = Mathf.Clamp(_pivotAngle, _minAngle, _maxAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = _lookAngle;
        transform.rotation = Quaternion.Euler(rotation);

        rotation = Vector3.zero;
        rotation.x = _pivotAngle;
        pivotCamTr.localRotation = Quaternion.Euler(rotation);
    }

    //카메라가 물체에 부딫혔을때 통과되지 않게 하기
    private void CameraCollision()
    {
        float mainCamPos = _defaltPos;
        RaycastHit hit;
        Vector3 raycastDir = cam.position - pivotCamTr.position;
        raycastDir.Normalize();

        if (Physics.Raycast(transform.position, raycastDir, out hit, Mathf.Abs(mainCamPos), layerMask))
        {
            float distance = Vector3.Distance(pivotCamTr.position, hit.point);

            mainCamPos = -distance - _camOffSet;
        }
        cameraVector.z = Mathf.Lerp(cam.localPosition.z, mainCamPos, 0.2f);
        cam.localPosition = cameraVector;
    }
}
