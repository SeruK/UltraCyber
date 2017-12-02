using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public sealed class ConditionalDeactivate : MonoBehaviour
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
	[Header("Deactivate if...")]
	[SerializeField]
	bool ifRewired;
	[SerializeField]
	bool ifNotRewired;
	[SerializeField]
	bool ifWebGL;
	[SerializeField]
	bool ifNotWebGL;
#pragma warning restore 0649
	#endregion // Serialized Fields
	#endregion // Fields

	#region Properties
	#endregion // Properties

	#region Methods
	protected void Awake()
	{
#if USE_REWIRED
		if(ifRewired) { gameObject.SetActive(false); }
#else // USE_REWIRED
		if(ifNotRewired) { gameObject.SetActive(false); }
#endif // USE_REWIRED

#if UNITY_WEBGL
		if(ifWebGL) { gameObject.SetActive(false); }
#else // UNITY_WEBGL
		if(ifNotWebGL) { gameObject.SetActive(false); }
#endif // UNITY_WEBGL
	}
	#endregion // Methods
}
