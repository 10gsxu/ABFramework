using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;

public class LuaUIBehaviour : MonoBehaviour {

	void Awake () {
        CallMethod("LUIManager", "RegistGameObject", gameObject);
	}

	public void AddButtonListener(LuaFunction action) {
		Button btn = transform.GetComponent<Button> ();
		if (btn != null) {
			btn.onClick.AddListener (delegate() {
				action.Call (gameObject);
			});
		}
	}

    /// <summary>
    /// Calls the method.
    /// </summary>
    protected void CallMethod(string module, string func, GameObject args)
    {
        string funcName = module + "." + func;
        LuaClient.Instance.CallFuncWithGameObject(funcName, args);
    }
		
}
