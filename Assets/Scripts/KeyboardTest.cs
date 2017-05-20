using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardTest : MonoBehaviour {
	
	public Canvas canvas;
	public List<InputField> inputFieldsInSection;

	private bool isInputFieldFocused = false;
	private Vector2 screenPosUnscaled = Vector2.zero;
	private int targetDifference = 0;
	private RectTransform _transform;

	// Use this for initialization
	void Start () {
		_transform = this.GetComponent<RectTransform> ();
	}

	void Update()
	{
		foreach (InputField inputField in inputFieldsInSection) {
			if (inputField.isFocused) {
				if (!isInputFieldFocused) {
					screenPosUnscaled = RectTransformUtility.WorldToScreenPoint (null, inputField.GetComponent<RectTransform> ().position);
					isInputFieldFocused = true;
				}
				targetDifference = (int)((GetKeyboardHeight () + inputField.GetComponent<RectTransform>().sizeDelta.y / 2 + Screen.height * 0.1f - screenPosUnscaled.y) / canvas.scaleFactor);
			}
		}
		if (targetDifference >= 0) {
			_transform.offsetMax = new Vector2(_transform.offsetMax.x, Mathf.Lerp(_transform.offsetMax.y, targetDifference, 0.15f));
			_transform.offsetMin = new Vector2(_transform.offsetMin.x, Mathf.Lerp(_transform.offsetMin.y, targetDifference, 0.15f));
		}
	}

	public void ResetInputFieldPosition()
	{
		_transform.offsetMax = new Vector2(_transform.offsetMax.x, Mathf.Lerp(_transform.offsetMax.y, 0f, 0.15f));
		_transform.offsetMin = new Vector2(_transform.offsetMin.x, Mathf.Lerp(_transform.offsetMin.y, 0f, 0.15f));
		isInputFieldFocused = false;
		targetDifference = 0;
	}

	//获取虚拟键盘高度
	public static int GetKeyboardHeight()
	{
		#if !UNITY_EDITOR
		#if UNITY_ANDROID
		using(AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
		AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

		using(AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
		{
		View.Call("getWindowVisibleDisplayFrame", Rct);

		return Screen.height - Rct.Call<int>("height");
		}
		}
		#elif UNITY_IOS
		return (int)TouchScreenKeyboard.area.height;
		#endif
		#else
		return 0;
		#endif
	}


}
