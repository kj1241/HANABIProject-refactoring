using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Life : MonoBehaviour
{
    public GameObject[] mLife = new GameObject[6];
    public int realLife = 0;
    public int realhart = 6;

    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
        if (SingletonManager.GetInstance().loseLife == true)
        {
            realLife = SingletonManager.GetInstance().life / 5;

            if (SingletonManager.GetInstance().life < 1)
                SceneManager.LoadScene("End");
           if(SingletonManager.GetInstance().life<=30&& SingletonManager.GetInstance().life>0)
            {
                if (realLife != realhart)
                {
                    Destroy(mLife[(SingletonManager.GetInstance().life / 5) ]);
                    realhart = realLife;
                }
            }
        }
        
    }
}
