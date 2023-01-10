using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //유니티 UI를 사용하기위해서

// 2022.09.15 김경주 작성
//싱글톤 콤보 관련 클래스
public partial class SingletonManager : MonoBehaviour
{
    public static int mScore; // 콤보 계산된 총점수(다른사람 작업 부분)
    public int life = 30; // 생명 (다른사람 작업 부분)
    public bool loseLife = false; // 생명을 잃었는지 (다른사람 작업부분)
    private int comboCount = 0; // 콤보 관련 변수

    const int JudgmentCount = 4; //판정 갯수 완벽함, 좋음, 나쁨, 미스
    public GameObject[] UI = new GameObject[JudgmentCount]; //인스펙터로 UI 얻는 부분
    public GameObject[] Effect = new GameObject[JudgmentCount]; // 인스펙터로 effect 얻는 부분

    enum NodeJudgmentState //노드콤보 판정 상태 (협의된 내용)
    {
        perfect, //완벽함 0 
        good,  //좋음 1
        bad, // 나쁨 2
        miss // 미스 3
    };
    NodeJudgmentState nodeJudgmentState;

    //싱글톤 구현
    private SingletonManager() { }  // 싱글톤 처음 사용할 때 호출
    private static SingletonManager instance = null;  //자기자신 맴버 변수
    public static SingletonManager GetInstance() //싱글톤 호출할때 부르는 변수
    {
        if (instance == null)
        {
            Debug.LogError("싱글톤을 불러오는데 실패했습니다.");
        }
        return instance;
    }

    public void SetJudgmentPerfect() //노드 판정 퍼팩트로 전환
    {
        nodeJudgmentState = NodeJudgmentState.perfect;
    }
    public void SetJudgmentGood() //노드 판정 좋음 전환
    {
        nodeJudgmentState = NodeJudgmentState.good;
    }
    public void SetJudgmentBad() //노드 판정 나쁨 전환
    {
        nodeJudgmentState = NodeJudgmentState.bad;
    }
    public void SetJudgmentMiss() //노드 판정 실패 전환
    {
        nodeJudgmentState = NodeJudgmentState.miss;
    }

    //유니티를 실행하고 1프레임에 발동시켜야하는것 
    public void Awake()
    {
        instance = this; //싱글톤 넣어주기
    }

    //스타트는 유니티 엔진에서 2프레임후에 발동함
    void Start()
    {
        StartSpineAnimation(); //스파인 에니메이션 시작 부분 (위치 :SingletonSpineAnimation.cs)
    }

    void Update()
    {

    }

    public void ComboJudgmentExecution() //콤보 판정 실행
    {
        switch (nodeJudgmentState) //판정 케이스 
        {
            case NodeJudgmentState.perfect: // 퍼펙트일 경우
                comboCount++; //콤보 횟수 추가
                mScore += 100; //점수 100 점 추가
                JudgmentEffect(0); //완벽한 이팩트 실행
                break; 
            case NodeJudgmentState.good: // 좋음일 경우
                comboCount++; //콤보 횟수 추가
                mScore += 75; //점수 75점 추가
                JudgmentEffect(1); //좋음 이팩트 실행
                break;
            case NodeJudgmentState.bad: //나쁨일 경우
                comboCount++; //콤보 횟수 추가
                mScore += 50;  //점수 50점 추가
                JudgmentEffect(2); //나쁨 이팩트 실행
                break;
            case NodeJudgmentState.miss: //실패했을 경우
                comboCount = 0; //콤보 초기화
                loseLife = true; //생명력 보석 에니메이션 발동
                JudgmentEffect(3); //실패 이팩트 실행
                break;
        }
    }
    
    public void ComboJudgmentFail() //노드를 노쳐서 실패하게 된다면
    {
        comboCount = 0; //콤보 초기화
        life -= 5; //5번이 한번의 생명력 보석
        loseLife = true; //생명력 보석 에니메이션 발동
        JudgmentEffect(3); //실패 이팩트 실행
    }

    private void JudgmentEffect(int i) // 판정 i 번째 UI와 이펙트
    {
        Instantiate(UI[i], new Vector3(0, 2.5f, 0), Quaternion.identity);  //UI 생성
        Instantiate(Effect[i], new Vector3(0, 2.5f, 0), Quaternion.identity);  //effect 생성
    }
}

