using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics; //ms정확도를 위해서 스탑워치를 쓰기위한 선언
using debug = UnityEngine.Debug;//스탑워치의 디버깅함수와 유니티 디버깅이 중복됨으로 정확하게 정의하기 위해 

// 2022.09.15 김경주 작성
// 노드의 프리팹
public class PrefabNodes : MonoBehaviour
{
    string nodekey = "0"; // 키보드 키
    float distance = 5f; // 거리 
    Stopwatch sw = new Stopwatch(); //스탑 워치선언 사용하는 이유 1000ms 까지 정확한 판정을 하기 위해서 
    Vector3 vec; // 파티클 터지는 백터 위치
    public GameObject[] EffectParticle = new GameObject[4]; // 판정 이펙트 효과 얻어오는 인스턴스
    public int NodeSpeed = 4; //속도 제어 레벨디자인을 위해서 만든 노드 움직이는 속도

    public string NodeKey // 키보드 키 입력받는 프로퍼티 (ReadText 에서 입력받음)
    {
        set { nodekey = value; }
    }

    //스타트는 유니티 엔진에서 2프레임후에 발동함
    void Start()
    {
        StartCoroutine(NodePerfectZone()); //노드 이벤트 감시하는 코루틴 생성
        vec = new Vector3(this.gameObject.transform.position.x, 0, 0); // 백터의 x 위치는 노드의 위치 기반 
        distance = distance / NodeSpeed; //레벨 디자인을 위해서 속도를 나중에 노드 빠르기를 낮출수 있기 때문에
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * NodeSpeed); // 노드를 +y축으로 이동시기 위해서 
    }
    IEnumerator NodePerfectZone()
    {
        sw.Start();  // 스탑원치 시작 
        while (true)
        {
            if (!gameObject|| sw.ElapsedMilliseconds > 8000 / NodeSpeed) //while 문 탈출 조건
                break;

            if (this.gameObject.transform.position.y < 0.5 && this.gameObject.transform.position.y > -0.5) // 노드가 판정 라인에 올라왔는지 확인
            {
                if (Input.GetKeyDown(nodekey)) //키를 입력 받았는지 확인 
                {
                    if (!SingletonManager.GetInstance().isAtteckButtonOn) //공격 에니메이션이 꺼저있다면 
                        SingletonManager.GetInstance().isAtteckButtonOn = true; //실행하게 만들어줌

                    //판정 기준 (유니티는 DirectX와 다르게 일정한 프레임을 만들수 없음으로 정확한 시간에 따라서 판정을 함)
                    if (sw.ElapsedMilliseconds > distance * 1000 - (100 / NodeSpeed) && sw.ElapsedMilliseconds < distance * 1000 + (100 / NodeSpeed)) 
                    {   //퍼펙트일 경우
                        Instantiate(EffectParticle[0], vec, Quaternion.identity); //퍼펙트 노드 터지는 이펙트 생성
                        SingletonManager.GetInstance().SetJudgmentPerfect(); // 판정을 퍼펙트로 전환
                        SingletonManager.GetInstance().ComboJudgmentExecution();//콤보 판정 실행 
                        sw.Stop(); //스탑워치 종료  
                        Destroy(this.gameObject); // 이 노드 게임 오브젝트를 파괴                 
                        yield break; //코루틴을 바로 종료시킨다
                    }
                    else if ((sw.ElapsedMilliseconds > distance * 1000 - (250 / NodeSpeed) && sw.ElapsedMilliseconds < distance * 1000 - (100 / NodeSpeed)) || (sw.ElapsedMilliseconds > distance * 1000 + (100 / NodeSpeed) && sw.ElapsedMilliseconds < distance * 1000 + (250 / NodeSpeed)))
                    {   //좋음일 경우
                        Instantiate(EffectParticle[1], vec, Quaternion.identity); //좋음 노드 터지는 이펙트 생성
                        SingletonManager.GetInstance().SetJudgmentGood(); // 판정을 좋음으로 전환
                        SingletonManager.GetInstance().ComboJudgmentExecution();//콤보 판정 실행 
                        sw.Stop(); //스탑워치 종료 
                        Destroy(this.gameObject); // 이 노드 게임 오브젝트를 파괴  
                        yield break; //코루틴을 바로 종료시킨다
                    }
                    else if ((sw.ElapsedMilliseconds > distance * 1000 - (400 / NodeSpeed) && sw.ElapsedMilliseconds < distance * 1000 - (250 / NodeSpeed)) || (sw.ElapsedMilliseconds > distance * 1000 + (250 / NodeSpeed) && sw.ElapsedMilliseconds < distance * 1000 + (400 / NodeSpeed)))
                    {   //나쁨일 경우
                        Instantiate(EffectParticle[2], vec, Quaternion.identity); // 나쁨 노드 터지는 이펙트 생성
                        SingletonManager.GetInstance().SetJudgmentBad(); // 판정을 나쁨 전환
                        SingletonManager.GetInstance().ComboJudgmentExecution();//콤보 판정 실행 
                        sw.Stop(); //스탑워치 종료 
                        Destroy(this.gameObject); // 이 노드 게임 오브젝트를 파괴  
                        yield break; //코루틴을 바로 종료시킨다
                    }
                    else
                    {   //실패일 경우
                        Instantiate(EffectParticle[3], vec, Quaternion.identity); // 실패 노드 터지는 이펙트 생성
                        SingletonManager.GetInstance().SetJudgmentMiss(); // 판정을 실패로 전환
                        SingletonManager.GetInstance().ComboJudgmentExecution();//콤보 판정 실행
                        break; //실패했을경우 기회를 더줄까 고민했으나 게임이 쉬워질수 있음으로 
                    }
                }
            }
            yield return null; //유니티 엔진 순서 Update -> null -> LateUpdate 
        }

        if (this.gameObject) //게임오브젝트가 만약에 파괴되지 않고 살아 있다면
        {
            Instantiate(EffectParticle[3], vec, Quaternion.identity);
            SingletonManager.GetInstance().ComboJudgmentFail();
            sw.Stop(); //스탑워치 종료
            Destroy(this.gameObject); // 이 노드 게임 오브젝트를 파괴 
        }

    }
}
