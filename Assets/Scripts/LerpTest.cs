using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    Vector3 target;
    Vector3 init;
    [SerializeField]
    Transform oponent;
    // Start is called before the first frame update
    void Start()
    {
        init = GetComponent<Transform>().position;
        target = new Vector3(5, 5, 0);
        StartCoroutine(StartPage());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator doit()
    {
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
            yield return null;
        }
        yield return StartCoroutine(oppomove());
        
    }

    IEnumerator oppomove()
    {
        
        while (oponent.position != target + 2*Vector3.right)
        {
            Debug.Log("a");
            oponent.position = Vector3.MoveTowards(oponent.position, target, Time.deltaTime);
            yield return null;
        }
    }
    public IEnumerator StartPage()
    {
        print("in StartPage()");
        yield return StartCoroutine (FinishFirst(5.0f));
        DoLast();
    }

    IEnumerator FinishFirst(float waitTime)
    {
        print("in FinishFirst");
        yield return new WaitForSeconds(waitTime);
        print("leave FinishFirst");
    }

    void DoLast()
    {
        print("do after everything is finished");
        print("done");
    }
}
