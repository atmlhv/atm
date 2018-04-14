using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatusDisplay : MonoBehaviour {

    Text levelText;
    Text presentTime;
    Text maxTime;
    Slider expSlider;
    int level;


	// Use this for initialization
	void Awake () {
        levelText = transform.Find("LevelNumber").GetComponent<Text>();
        presentTime = transform.Find("PresentTime").GetComponent<Text>();
        maxTime = transform.Find("MaxTime").GetComponent<Text>();
        expSlider = transform.Find("ExpSlider").GetComponent<Slider>();
    }
	
    public void SetInitial(Status st)
    {
        TimeSpan timeSpan = new TimeSpan(0,0,0);
        int i = 0;
        level = st.level;
        levelText.text = st.level.ToString();
        presentTime.text = timeSpan.ToString();
        timeSpan = new TimeSpan(0, 0, st.maxTime);
        maxTime.text = timeSpan.ToString();
        expSlider.maxValue = st.maxExp - st.prevExp;
        expSlider.value = st.exp - st.prevExp;
    }

    public IEnumerator TimeCount(int mt)
    {
        float tt = 0;
        TimeSpan ts;
        while (tt < mt)
        {
            tt += Time.deltaTime;
            ts = new TimeSpan(0, 0, (int)tt);
            presentTime.text = ts.ToString();
            yield return null;
        }
    }

    public void RefleshSlider(Status st)
    {
        if (level != st.level) {
            level = st.level;
            levelText.text = st.level.ToString();
            expSlider.maxValue = st.maxExp - st.prevExp;
            expSlider.value = st.exp - st.prevExp;
            TimeSpan timeSpan = new TimeSpan(0, 0, st.maxTime);
            maxTime.text = timeSpan.ToString();
        }
        else
        {
            expSlider.value = st.exp - st.prevExp;
        }
    }

    public void ResetTime()
    {
        TimeSpan timeSpan = new TimeSpan(0, 0, 0);
        presentTime.text = timeSpan.ToString();

    }

	// Update is called once per frame
	void Update () {
		
	}
}
