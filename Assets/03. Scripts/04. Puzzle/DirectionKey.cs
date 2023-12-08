using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionKey : Puzzle
{
    float distance = 100f;
    [SerializeField]
    LayerMask layerMaskKey;

    public GameObject key = null;
    Vector3 keyVec;
    public Vector3 keyPoint;

    float rangeValue = 0.045f;
    public bool keyInput = false;
    public bool keySole = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            KeyPoint();//키초기 위치 저장
        }
        else if (Input.GetMouseButton(0))
        {
            KeyPosition();//키움직임
        }
        else if (Input.GetMouseButtonUp(0))
        {
            key.transform.position = keyPoint; //키위치 리셋
        }
        if (keySole)
        { Debug.Log("자물쇠 풀림"); }
    }
    void KeyPoint()//키값 초기화를 위한 값저장
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, layerMaskKey))
        {
            keyPoint = hit.transform.position;//기본 키값 저장
            key = hit.transform.gameObject;//레이 맞은 오브젝트 정보
            keyInput = true;
        }
    }
    void KeyPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, layerMaskKey))
        {
            keyVec = hit.transform.position;//초기화 해줄 포지션 값을 담는 변수

            if (Mathf.Abs(keyPoint.x - hit.point.x) < Mathf.Abs(keyPoint.y - hit.point.y))
            {
                key.transform.position = new Vector3(keyPoint.x, hit.point.y, keyPoint.z);
            }
            else if (Mathf.Abs(keyPoint.x - hit.point.x) >= Mathf.Abs(keyPoint.y - hit.point.y))
            {
                key.transform.position = new Vector3(hit.point.x, keyPoint.y, keyPoint.z);
            }
            if (Vector3.Distance(keyPoint, keyVec) > rangeValue)
            {

                key.transform.position = keyPoint;
            }
            //너무 마우스 포인터랑 똑같이 움직이니까 빠른거같아서 조금 느리게 움직이게 하고 싶은데 
            //이제 포지션이 음수 양수 다있으니까 원하는대로 잘안움직인다.
        }
    }

    public override void Interact()
    {
        Debug.Log("시네머신, 플레이어 구분");
    }
}
