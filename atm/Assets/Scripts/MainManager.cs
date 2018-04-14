using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodTouches;

public class MainManager : MonoBehaviour {

    public Vector3 initialPositionOfCharacter = new Vector3(0,5f,0);
    private Character character;
    private Status status = new Status();
    private StatusDisplay statusDisplay;
    private float calculateTime = 3f;
    private int expPerPerson = 10; //今は仮、ステージやキャラクター依存になる予定
    private int maleSpeicies = 3;
    private GameObject camera;
    private Entity_StatusData en;
    private List<GameObject> maleData = new List<GameObject>();
	// Use this for initialization
	void Start () {
        character = Instantiate(Resources.Load<GameObject>("Prefab/Female"), initialPositionOfCharacter, Quaternion.identity).GetComponent<Character>();
        camera = GameObject.Find("Main Camera");
        string characterString = "Prefab/Character/Male/1/";
        for (int i = 0; i < maleSpeicies; i++)
        {
            maleData.Add(Resources.Load<GameObject>(characterString + (i + 1).ToString()));
        }
        en = Resources.Load<Entity_StatusData>("Data/atm_data");
        SetStatus();
        SetDisplay(status);
        StartCoroutine(UPD());
    }
	
    private IEnumerator UPD()
    {
        while (true)
        {
            if (GodTouch.GetPhase() == GodPhase.Began)
            {
                yield return StartCoroutine(character.Move(Vector3.zero));
                IEnumerator timeCount = statusDisplay.TimeCount(status.maxTime);
                StartCoroutine(timeCount);
                yield return StartCoroutine(character.Fishing(status, maleData));
                StopCoroutine(timeCount);
                statusDisplay.ResetTime();
                yield return StartCoroutine(character.Move(8f * Vector3.up, true,.5f));
                yield return CalculateStatus();
                yield return new WaitForSeconds(.5f);
                yield return StartCoroutine(MoveCamera(10f * Vector3.back));
            }
            yield return null;
        }

    }

    private void SetStatus()
    {
        status.statusParam = en.sheets[0].list;
    }

    private void SetDisplay(Status st)
    {
        statusDisplay = GameObject.Find("StatusDisplay").GetComponent<StatusDisplay>();
        statusDisplay.SetInitial(st);
    }

    private IEnumerator CalculateStatus()
    {
        float interval = calculateTime / status.maxPerson;
        for(int i = 0; i < character.maleObject.Count; i++)
        {
            yield return new WaitForSeconds(interval);
            Destroy(character.maleObject[character.maleObject.Count - 1 - i]);
            status.PlusExp(expPerPerson); //ここはメソッド化してレベルアップ判定も含むようにする
            statusDisplay.RefleshSlider(status);
        }
    }

    private IEnumerator MoveCamera(Vector3 PurposePosition, float SpeedConst = .8f)
    {

        Vector3 initialPosition = camera.transform.position;
        float a = 0;


        while (a < 1)
        {
            a += Time.deltaTime * SpeedConst;
            camera.transform.position = (1f - a) * initialPosition + a * PurposePosition;
          
            yield return null;
        }

        camera.transform.position = PurposePosition;

    }

    // Update is called once per frame
    void Update () {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Time.timeScale = 5f;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Time.timeScale = 1;
        }
    }
}

public class Status
{
    public int level;
    public List<Entity_StatusData.Param> statusParam;
    public float power {
        get
        {
            return statusParam[this.level].power;
        }
    }
    public int maxPerson
    {
        get
        {
            return statusParam[this.level].maxPerson;
        }
    }
    public int exp;

    public int maxExp
    {
        get
        {
            return statusParam[this.level].nextSumExp;
        }
    }

    public int prevExp
    {
        get
        {
            return statusParam[this.level - 1].nextSumExp;
        }

    }
   

    public int maxTime {
        get
        {
            return (int)(maxPerson / (power - .0001f));
        }
    }

    public void PlusExp(int addedExp)
    {
        exp += addedExp;
        if(exp > maxExp)
        {
            level++;
        }
    }

    public Status()
    {
        level = 1;
        exp = 0;
    }
        
}
