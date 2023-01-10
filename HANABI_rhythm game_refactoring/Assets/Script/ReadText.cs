using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//2022.09.01 김경주 작성
public class ReadText : MonoBehaviour
{
    Dictionary<int, string> nodeKeyTable = new Dictionary<int, string>(); //node에 원하는 key 값을 사용하기위해서 맵핑테이블 작성(value값은 키보드 누르는 값)

    const int keyCount = 5;    //키보드 키(A,S,D,J,K)갯수// 작성한 이유: 추후 레벨디자인에서 키보드 변화 할수 있도록 변경
    List<int>[] KeyNodesList = new List<int>[keyCount]; //키보드의 갯수만큼 키 노드들을 담은 리스트를 생성

    public float delaytime = 0.571428571428571f;   // 채보와 협의된 채보 간격시간 
    public GameObject prefabNodes; //노드들을 생성하기위해서 프리팹을 담는 곳
    public float delaytimeCoroutinesTime = 3.75f; //초반 시작하기 위해서 로딩하는 시간

    //유니티를 실행하고 1프레임에 발동시켜야하는것 
    private void Awake()
    {
        //자주 보이는 실수 종류중 하나 배열을 동적할당하고 그안에 리스트를 따로 동적할당으로 안해주면 NULL error 뜸
        for (int i = 0; i < keyCount; ++i)
        {
            KeyNodesList[i] = new List<int>();
        }

        //텍스트로 읽어오면 좋았지만 프로그래밍적으로 만들어주는 key값 (for문안에 switch case 문을 바꾸기위해서 선택)
        nodeKeyTable.Add(0, "d");
        nodeKeyTable.Add(1, "f");
        nodeKeyTable.Add(2, "space");
        nodeKeyTable.Add(3, "j");
        nodeKeyTable.Add(4, "k");
    }

    //스타트는 유니티 엔진에서 2프레임후에 발동함
    void Start()
    {
        string path = Application.dataPath + "/Resources/sampleExample.txt"; //채보 파일이 위치한 유니티 빌드또는 프로젝트 상대 경로
        string[] textValue = System.IO.File.ReadAllLines(path); // 채보 파읽 읽어오기

        if (textValue.Length > 0)
        {
            for (int i = 0; i < textValue.Length; ++i)
            {
                for (int j = 0; j < textValue[i].Length; ++j)
                {
                    if (textValue[i].Length > keyCount) //혹시 길이가 생각한 키노드보다 넘어간다면 에러나지 않게 코딩
                        continue;

                    KeyNodesList[j].Add(textValue[i][j] - 48); //당시 -48을한 이유: 아스키 코드를 이용하여 작성한 키 system.char 를 system.int 로 빠르게 변환시킬수 있는 효과가 있어서 작성
                }
            }
        }
        Invoke("startCoroutines", delaytimeCoroutinesTime); //시작시간을 좀더 딜레이시키기 위해서
    }
    void Update()
    {

    }

    //일정하게 노드를 생성하기 위해서 코루틴을 이용하여 생성
    IEnumerator CreateNodeNumber(int i)
    {
        int count = 0;

        while (true)
        {
            if (KeyNodesList[i].Count - 1 < count) //while 탈출 조건
                break;
            if (KeyNodesList[i][count] == 1) //만약 채보 텍스트에서 1을 읽었다면 
            {
                GameObject node = Instantiate(prefabNodes, new Vector3(-3+(2*i), -5, 0), Quaternion.identity) as GameObject; //프리팹 노드를 생성
                PrefabNodes nodePrefabScrit =  node.GetComponent<PrefabNodes>(); //prefabNodes의 스크립트 붙이기
                if (nodePrefabScrit) // 찾는스크립트가 존재한다면
                    nodePrefabScrit.NodeKey = nodeKeyTable[i]; //딕셔너리를 이용한 맵핑테이블로 좀더 성능 최적화
            }
            yield return new WaitForSeconds(delaytime); // 채보에서 협의된 시간만큼 지연
            count++;
        }
        yield return null; //코루틴 끝나면 탈출
    }

    public void startCoroutines()
    {
        for (int i = 0; i < keyCount; ++i)
            StartCoroutine(CreateNodeNumber(i)); // 키 갯수만큼  노드를 생성하기위해서 코루틴 작성
    }
}
