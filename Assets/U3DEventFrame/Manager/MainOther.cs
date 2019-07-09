using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainOther : MonoBehaviour {

	void Start () {
        LuaAndCMsgCenter landcCenter = gameObject.AddComponent<LuaAndCMsgCenter>();
        LuaEventProcess.Instance.setMonoChild(landcCenter);
	}
	
	void Update () {
		
	}
}
