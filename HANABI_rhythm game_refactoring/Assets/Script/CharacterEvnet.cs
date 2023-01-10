using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // 이벤트 리스너를 만들기 위해 선언

//2022.09.15 김경주 작성
//캐릭터에 입력받을 이벤트 리스너를 만드는 클래스
public class CharacterEvnet : MonoBehaviour
{
    public UnityEvent unityEvent;

    //스타트는 유니티 엔진에서 2프레임후에 발동함
    void Start()
    {
        //콜백 등록하기위해서 
        //unityEvent.AddListener();
        
    }

    void Update()
    {
        
    }
}
