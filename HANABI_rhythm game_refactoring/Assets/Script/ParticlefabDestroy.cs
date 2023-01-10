using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//김경주 작성
//파티클 생성후 메모리에서 계속 무한루프 돌기 때문에 지워주는 클래스 
public class ParticlefabDestroy : MonoBehaviour
{
    // 유니티 시작후 2프레임에 발동
    void Start()
    {
        Invoke("EndPrefab", 0.2f); // 0.2초후 EndPrefab 함수 발동
    }

    void EndPrefab()
    {
        Destroy(this.gameObject); //이 게임 오브젝트 삭제
    }
}

