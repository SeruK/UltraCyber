using UnityEngine;
using UE = UnityEngine;
using UI = UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public sealed class WebGLFullscreenButton : UI.Button
{
	#region Types
	#region Serialized Types
#pragma warning disable 0649
#pragma warning restore 0649
	#endregion // Serialized Types
	#endregion // Types

	#region Fields
	#region Serialized Fields
#pragma warning disable 0649
#pragma warning restore 0649
	#endregion // Serialized Fields
	#endregion // Fields

	#region Properties
	#endregion // Properties

	#region Methods
#if !UNITY_WEBGL
	protected new void Awake()
	{
		base.Awake();

		gameObject.SetActive(false);
	}
#endif // !UNITY_WEBGL

	public override void OnPointerDown(PointerEventData eventData)
	{
		Screen.fullScreen = !Screen.fullScreen;
	}
	#endregion // Methods
}
