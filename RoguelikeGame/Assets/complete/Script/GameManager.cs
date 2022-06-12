using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 타일 담아줄 배열을 생성
    [SerializeField] public GameObject[] landArray;
    // 주인공 오브젝트
    [SerializeField] public GameObject hero;
    // 타일 하나의 크기 
    [SerializeField] public float UnitSize;

    // 시야 밖에 타일이 없으면 타일을 갱신한다.
    readonly float halfSight = 2;

    readonly float speed = 5;


    Vector2[] border;

    void Start()
    {


        // border 초기화하고 주인공이 (0,0)에 있으므로 전체 타의 왼쪽 끝 좌표는 -UnitSize * 9f, 오른쪽 끝 좌표는 UnitySize * 5f이다.
        // 둘을 더하면 UnitSize * 18이며 이는 전체 타일의 크기와 같다.
        // 수직 방향값도 동일 
        border = new Vector2[]
        {
            new Vector2(-UnitSize * 9f, UnitSize * 5f),
            new Vector2(UnitSize * 9f, -UnitSize * 5f),
        };

    }

    void Update()
    {
        //키 입력이 없으면 업데이트 하지 않음
        if (!Input.anyKey)
            return;

        //이동방향을 정한다.
        Vector3 delta;
        switch (Input.inputString)
        {
            case "w":
                delta = Vector2.up;
                break;
            case "a":
                delta = Vector2.left;
                break;
            case "s":
                delta = Vector2.down;
                break;
            case "d":
                delta = Vector2.right;
                break;
            default:
                return;

        }

        //지금 프레임에 이동할 거리를 구함.
        delta *= Time.deltaTime * speed;

        //캐릭터 위치 업데이트
        hero.transform.position += delta;

        //카메라의 위치를 업데이트한다.
        Camera.main.transform.position += delta;

        //시야 영역 중 타일이 없는 경우 체크
        BounryCheck();
    }

    void BounryCheck()
    {   //오른쪽 시야 영역 중 타일이 없을 때 
        if (border[1].x < hero.transform.position.x + halfSight)
        {
            //타일 영역을 타일 하나 사이즈만큼 오른쪽으로 이동한다.
            //이는 다음 영역 체크 시 혼돈을 피하기 위함
            border[1] -= Vector2.right * UnitSize;
            border[2] -= Vector2.right * UnitSize;

            MoveWorld(0);
        }
        //왼쪽 시야 영역 중 타일이 없을 때 이어짐
        else if (border[0].x < hero.transform.position.x - halfSight)
        {
            border[1] -= Vector2.right * UnitSize;
            border[2] -= Vector2.right * UnitSize;

            MoveWorld(2);
        }
        //위쪽 시야 영역 중 타일이 없을 때 이어짐
        else if (border[0].y < hero.transform.position.y + halfSight)
        {
            border[1] -= Vector2.right * UnitSize;
            border[2] -= Vector2.right * UnitSize;

            MoveWorld(1);
        }
        //아래쪽 시야 영역 중 타일이 없을 때 이어짐
        else if (border[1].y < hero.transform.position.y - halfSight)
        {
            border[1] -= Vector2.right * UnitSize;
            border[2] -= Vector2.right * UnitSize;

            MoveWorld(3);
        }
    }
    // 타일을 움직임 
    void MoveWorld(int dir)
    {
        GameObject[] _landArray = new GameObject[180];
        System.Array.Copy(landArray, _landArray, 180);

        switch (dir)
        {
            //타일의 가장 왼쪽 3개 줄을 오른쪽으로 보낸다.
            case 0:
                for (int i = 0; i < 180; i++)
                {
                    //새 좌표를 설정한다. 좌측으로 1칸 가려면 아래 배열에서 18을 빼면 된다.
                    //예를 들어, 19에서 1로 가려면 18을 빼면 된다.
                    int revise = i - 18;
                      
                    //만약 3을 뺀 값이 0 이상이라면 값을 보정함.
                    //예를 들어, 0은 -3이 아닌 6으로, 1은 -2가 아닌 7로 보낸다.
                    if (revise < 0)
                    {
                        landArray[180 + revise] = _landArray[i];
                        //실제 오브젝트 타일을 바꿈
                        _landArray[i].transform.position += Vector3.right * UnitSize * 18;
                    }
                    else
                        //타일 배열의 인덱스를 업데이트한다. 실제 오브젝트의 좌표는 그대로이다.
                        landArray[revise] = _landArray[i];
                }
                break;

            //타일의 가장 아래 3개 줄을 윗쪽으로 보낸다.
            case 1:
                for (int i = 0; i < 180; i++)
                {
                    //옮길 줄을 특정한다. 맨 아랫 줄은 인덱스 3으로 나눈 나머지가 2이다.
                    int revise = i % 10;

                    //아래 내용은 서술한 내용과 같음
                    if (revise == 2)
                    {
                        landArray[i - 2] = _landArray[i];
                        _landArray[i].transform.position += Vector3.up * UnitSize * 10;
                    }
                    else
                        landArray[i + 1] = _landArray[i];
                }
                break;
            case 2:
                for (int i = 0; i < 180; i++)
                {
                    int revise = i + 18;

                    if (revise > 8)
                    {
                        landArray[i + 2] = _landArray[i];
                        _landArray[i].transform.position -= Vector3.right * UnitSize * 18;
                    }
                    else
                        landArray[revise] = _landArray[i];

                }
                break;
            case 3:
                for (int i = 0; i < 180; i++)
                {
                    int revise = i % 10;

                    if (revise == 0)
                    {
                        landArray[i + 2] = _landArray[i];
                        _landArray[i].transform.position -= Vector3.up * UnitSize * 10;
                    }
                    else
                        landArray[i - 1] = _landArray[i];

                }
                break;
        }
    }
}
