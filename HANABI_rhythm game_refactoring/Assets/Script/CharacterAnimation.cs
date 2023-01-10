using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2022.09.15 김경주 작성
//스파인 툴에서 에니메이션을 만들어주기 위해서
public class CharacterAnimation
    : MonoBehaviour
{
    enum CharacterAnimationState //아티스트와 협의된 에니메이션 동작
    {
        idle = 0, // 정지상태
        shot_L = 1, // 왼쪽 공격상태
        shot_R = 3  // 오른쪽 공격상태
    }

    int currentAnimationState; //현재 애니메이션 상태에 관해서
    bool isCurrentLeftAnimation; //현재 애니메이션이 왼쪽인가 (협의된 동작 버튼을 누를때마다 idle -> shot_L -> idle -> shot_R 루프)

    //스타트는 유니티 엔진에서 2프레임후에 발동함
    void Start()
    {
        currentAnimationState = 0; //애니메이션 초기상태.
        isCurrentLeftAnimation = false; // 초기 왼쪽이아닌 상태 왼쪽부터 가야됨
        StartCoroutine(CharacterAnimationCoroutine()); //캐릭터 에니메이션 코루틴
    }

    IEnumerator CharacterAnimationCoroutine() //캐릭터 에니메이션 관련 코루틴 함수
    {
        while (true)
        {
            if (this.gameObject == null)
                yield break; // 혹시 가비지 컬렉터에 의해서 수집 안될 경우를 대비해서

            if (currentAnimationState == (int)CharacterAnimationState.idle && SingletonManager.GetInstance().isAtteckButtonOn == true) //현재의 상태가 idle과 공격버튼이 눌렀다면 이걸 해주는이유 혹시라도 코루틴 제어권이 돌아왔을때 미연의 불상사를 방지하기 위해서
            {
                if (isCurrentLeftAnimation == false) // 왼쪽공격 애니메이션이 아니라면 
                {
                    SingletonManager.GetInstance().CharactorShotL(); //왼쪽 공격 모션을 발동시기고
                    currentAnimationState = (int)CharacterAnimationState.shot_L; // 현재 상태는 왼쪽 공격상태
                    isCurrentLeftAnimation = true; // 왼쪽공격을 했음으로 
                    yield return StartCoroutine(ReturnCourtineIdle()); //ReturnCourtineIdel 실행중에 잠시 재어권 대기
                }
                else if (isCurrentLeftAnimation == true) // 왼쪽공격이라면 
                {
                    SingletonManager.GetInstance().CharactorShotR(); //오른쪽 공격 모션을 발동시키고
                    currentAnimationState = (int)CharacterAnimationState.shot_R; // 현재 상태는 오른쪽 공격상태
                    isCurrentLeftAnimation = false; // 왼쪽공격을 했음으로 
                    yield return StartCoroutine(ReturnCourtineIdle()); //ReturnCourtineIdel 실행중에 잠시 재어권 대기
                }
            }
            yield return null; //엡데이트를 실행후에 lateupdate 들어가기전에 코루틴제어권 넘김 이건 프레임마다 실행해주기위해서
        }
    }

    IEnumerator ReturnCourtineIdle() // idle 상태로 되돌리기
    {
        SingletonManager.GetInstance().isAtteckButtonOn = false; //공격버튼은 원래대로 (위에다 놓는 이유 재어권을 아직 게임루프에 넘어가기전에 처리해 주기 위해서)
        yield return new WaitForSecondsRealtime(0.1f); // 공격애니메이션 모션 대기 시간
        SingletonManager.GetInstance().CharactorIdle(); // idel 상태로 변형
        currentAnimationState = (int)CharacterAnimationState.idle; // 현재 상태는 idle 상태로 변경
    }
}
