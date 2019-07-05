using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace U3DEventFrame {
	public class UIBehaviour : MonoBehaviour {

		//把控件Script注册到UIManager
		//可以直接查找 物体， 把物体本身 注册到UIManager

		void Awake () {
            UIManager.Instance.RegistGameObject(name, gameObject);
        }

        void Start() {
		}

		void OnDestroy() {
			UIManager.Instance.UnRegistGameObject (name);
		}

		public void AddButtonListener(UnityAction action) {
			if (action != null) {
				Button btn = transform.GetComponent<Button> ();
				btn.onClick.AddListener (action);
			}
		}

		public void RemoveButtonListener(UnityAction action) {
			if (action != null) {
				Button btn = transform.GetComponent<Button> ();
				btn.onClick.RemoveListener (action);
			}
		}

		public void AddToggleListener(UnityAction<bool> action) {
			if (action != null) {
				Toggle btn = transform.GetComponent<Toggle> ();
				btn.onValueChanged.AddListener (action);
			}
		}

		public void RemoveToggleListener(UnityAction<bool> action) {
			if (action != null) {
				Toggle btn = transform.GetComponent<Toggle> ();
				btn.onValueChanged.RemoveListener (action);
			}
		}
	}
}
