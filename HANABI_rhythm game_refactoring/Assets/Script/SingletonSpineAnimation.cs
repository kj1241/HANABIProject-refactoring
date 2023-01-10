using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity; //스파인 에니메이션을 사용하기위해서

//2022.09.15 김경주 작성
//싱글톤 클래스에서 캐릭터의 스파인 애니메이션 부분만 분리한 클래스
public partial class SingletonManager : MonoBehaviour
{
    [System.NonSerialized] public SkeletonAnimation skill; //스파인 스켈레톤 에니메이션 선언부분
    [System.NonSerialized] public GameObject girl, boy; // 캐릭터가 남,여
    private string curAnimation = ""; //현재 스켈레톤 에니메이션
    [System.NonSerialized] public bool isAtteckButtonOn = false; //현재 애니메이션 공격버튼이 눌렸는지 확인

    void StartSpineAnimation() // 메인씬으로 올때 캐릭터 선택 부분에서 어떤 캐릭터로 선택되었는지 받아온다.
    {
        if (GameManager.FemaleCharactorSelect) //캐릭터 선택화면에서 여자를 골랐다면 (다른 사람 작업 부분)
        {
            girl.SetActive(true); //여자의 게임오브젝트를 활성화 시킨다. (다른 사람 작업 부분)
            skill = girl.GetComponent<SkeletonAnimation>(); //애니메이션을 이용하기 위해서 캐릭터에 스파인 스켈레톤 에니메이션 컴포넌트를 부착(김경주 작업)
        }
        else if (GameManager.MaleCharactorSelect) //캐릭터 선택화면에서 남자를 골랐다면 (다른 사람 작업 부분)
        {
            boy.SetActive(true); //남자의 게임오브젝트를 활성화 시킨다. (다른 사람 작업 부분)
            skill = boy.GetComponent<SkeletonAnimation>(); //애니메이션을 이용하기 위해서 캐릭터에 스파인 스켈레톤 에니메이션 컴포넌트를 부착(김경주 작업)
        }
        else
        {
            Debug.Log("남녀 둘 다 선택되지 않은 예외"); //디버깅용으로 에러 방지(김경주 작업)
        }
    }

    void SetAnimation(string name, bool loop, float speed) //스파인으로 만든 스켈렌톤 에니메이션 설정 함수(이름, 루프, 속도)
    {
        if (name == curAnimation) //먄약 지금 진행중인 에니메이션이 같다면 
        {
            return;
        }
        else
        {
            skill.state.SetAnimation(0, name, loop).TimeScale = speed; //스켈레톤 에니메이션을 설정하고 거기에 속도를 설정
            curAnimation = name; //현재 이름을 갱신해준다.
        }

    }

    public void CharactorIdle() //캐릭터 일시정지 상태
    {
        skill.state.SetAnimation(0, "idle", true).TimeScale = 0.01f; // 트랙0 에 "idle'에니메이션을 할당하고 무한루프 돌린다. 타임스케일은 0.01초
    }
    public void CharactorShotR() //캐릭터 오른쪽 공격 상태
    {
        skill.state.SetAnimation(0, "shot_r", true).TimeScale = 0.01f; // 트랙0 에 "shot_r'에니메이션을 할당하고 무한루프 돌린다. 타임스케일은 0.01초
    }
    public void CharactorShotL() //캐릭터 왼쪽 공격 상태
    {
        skill.state.SetAnimation(0, "shot_l", true).TimeScale = 0.01f; // 트랙0 에 "shot_l'에니메이션을 할당하고 무한루프 돌린다. 타임스케일은 0.01초
    }
}
