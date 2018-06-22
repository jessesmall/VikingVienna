using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeBar : MonoBehaviour {

    public ScriptableInt Attribute;
    private Image Bar;

	// Use this for initialization
	void Start () {
        Bar = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        var barPercent = (float)Attribute.RuntimeValue / (float)Attribute.InitialValue;
        Bar.fillAmount = 1 * barPercent;
	}

    public void ResetBar()
    {
        Bar.fillAmount = 1f;
    }
}
