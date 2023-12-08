using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Windows;

public class DirectionKeySolve : MonoBehaviour
{
    DirectionKey puzzleKey;

    float checkValue = 0.03f; //조건 체크가 되기 시작하는 이동 범위

    int[] solveKeyNum = new int[] { 2, 3, 1, 1, 2, 4 };
    int[] inputKeyNum = new int[6];
    int i;

    private void Awake()
    {
        puzzleKey = GetComponent<DirectionKey>();
        int i = 0;
    }
    private void Update()
    {
        if (puzzleKey.keyInput == true)//인풋값 받을 수 있는 상태에만 실행
        {
            keyArrayCheck();
        }
        if (i == 6)
        {
            puzzleKey.keySole = true;
        }
    }
    void keyArrayCheck()
    {
        if (InputNum() == 0)
            return;
        inputKeyNum[i] = InputNum();
        if (solveKeyNum[i] == inputKeyNum[i])
            i++;
        else if (solveKeyNum[i] != inputKeyNum[i])
            i = 0;
        puzzleKey.keyInput = false;

        Debug.Log(i);
    }
    int InputNum()
    {
        if ((puzzleKey.keyPoint.x - puzzleKey.key.transform.position.x) < -checkValue) //왼쪽으로 갔을 때
        {
            return 3;
        }
        else if ((puzzleKey.keyPoint.x - puzzleKey.key.transform.position.x) > checkValue) //오른쪽으로 갔을 때
        {
            return 1;
        }
        else if ((puzzleKey.keyPoint.y - puzzleKey.key.transform.position.y) < -checkValue) //아래로 갔을 때
        {
            return 2;
        }
        else if ((puzzleKey.keyPoint.y - puzzleKey.key.transform.position.y) > checkValue) //위로 갔을 때
        {
            return 4;
        }
        else
        {
            puzzleKey.keyInput = true;
            return 0;
        }

    }


}
