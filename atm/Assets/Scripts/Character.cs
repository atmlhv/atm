using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodTouches;

public class Character : MonoBehaviour {

    /*この辺にステータスのあれこれを定義*/
    private int level;
    private float power;
    private int maxPerson;
    private int maxTime;
    private int person = 0;
    public List<GameObject> maleObject;
    

	// Use this for initialization
	void Start () {
        ReadStatus();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*この辺でステータスを読み込む*/

    private void ReadStatus()
    {

        level = 1;
        power = 0.25f;
        maxPerson = 5;
        maxTime = (int)(maxPerson / (power - .00001f));

    }

    public IEnumerator Move(Vector3 PurposePosition,bool IsCamera = false,float SpeedConst = .8f)
    {

        Vector3 initialPosition = transform.position;
        float a = 0;
        Transform cameraTransform;
        Vector3 initialCameraPosition;
            cameraTransform = GameObject.Find("Main Camera").transform;
            initialCameraPosition = cameraTransform.position;


        while (a < 1)
        {
            a += Time.deltaTime * SpeedConst;
            transform.position = (1f - a) * initialPosition + a * PurposePosition;
            if (IsCamera) {
                cameraTransform.position = initialCameraPosition + (1f - a) * initialPosition + a * PurposePosition;
            }
            yield return null;
        }

        transform.position = PurposePosition;

    }

    public IEnumerator Fishing(Status st, List<GameObject> objList)
    {
        float kankaku = 1/st.power;
        float r_max = 1f;
        float r;
        float theta;
        GameObject male = Resources.Load<GameObject>("Prefab/Male");
        maleObject = new List<GameObject>();
        person = 0;
        yield return null;
        while (person < st.maxPerson)
        {
            float tt = 0;
            while (tt < kankaku)
            {
                
                if (GodTouch.GetPhase() == GodPhase.Began)
                {
                    break;
                }
                tt += Time.deltaTime;
                yield return null;
            }
            if (GodTouch.GetPhase() == GodPhase.Began)
            {
                break;
            }
            person++;
            r = UnityEngine.Random.Range(0, r_max);
            theta = UnityEngine.Random.Range(0, Mathf.PI);
            maleObject.Add(Instantiate(objList[UnityEngine.Random.Range(0,objList.Count)], transform.position + new Vector3(r * Mathf.Cos(theta), -0.5f + -r * Mathf.Sin(theta), -0.1f), Quaternion.identity,transform));

        }

        while (true)
        {
            if (GodTouch.GetPhase() == GodPhase.Began)
            {
                break;
            }
            yield return null;
        }
    }

    /*
    public bool IsAwait(float initialTime,float tt)
    {

    }
    */
}
