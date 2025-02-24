using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform targetTr;//���� ��ǥ
    [SerializeField] Transform pivotCamTr;//���� ī�޶� ��ġ
    [SerializeField] LayerMask layerMask;//�����ɽ�Ʈ�� �΋H�� ���̾�
    Transform cam; //����ī�޶� ��ġ
    private Vector2 camRoDir;//ī�޶� ȸ�� ����
    private Vector3 cameraVector;
    private float _lookAngle;//��,�� �ޱ� ��
    private float _pivotAngle;//��,�� �ޱ� ��
    private float _camRoSpeed;//ī�޶� ȸ���ӵ�
    private float _maxAngle;//�ִ� ��,�� �ޱ� ��
    private float _minAngle;//�ּ� ��,�� �ޱ� ��
    private float _camOffSet;//������� ī�޶�
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

    //ī�޶� ĳ���� ���󰡰���
    private void CameraFallowTarget()
    {
        transform.position = targetTr.position;
    }

    //ī�޶� ȸ��
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

    //ī�޶� ��ü�� �΋H������ ������� �ʰ� �ϱ�
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
