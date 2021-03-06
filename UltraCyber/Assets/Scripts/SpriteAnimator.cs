﻿using UnityEngine;
using UE = UnityEngine;
using UnityEngine.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

public sealed class SpriteAnimator : MonoBehaviour
{
	#region Types
	#region Serialized Types
#pragma warning disable 0649
	[Serializable]
	class AnimationDef
	{
		public string name;
		public bool looping;
		public Sprite[] sprites;
	}
#pragma warning restore 0649
	#endregion // Serialized Types

	struct RuntimeAnim
	{
		public AnimationDef def;
		public int frame;
		public float timeSinceLastFrame;
	}
	#endregion // Types

	#region Static Fields
	static List<SpriteAnimator> activeAnimators = new List<SpriteAnimator>();
	#endregion // Static Fields

	#region Fields
	#region Serialized Fields
#pragma warning disable 0649
	[SerializeField]
	[FormerlySerializedAs("rend")]
	SpriteRenderer _rend;
	[SerializeField]
	string playAutomatically;
	[SerializeField]
	public int fps = 12;
	[SerializeField]
	AnimationDef[] anims;
#pragma warning restore 0649
	#endregion // Serialized Fields

	Dictionary<string, AnimationDef> nameToAnim = new Dictionary<string, AnimationDef>();
	Dictionary<string, AnimationDef> runtimeAnims = new Dictionary<string, AnimationDef>();

	RuntimeAnim? defaultAnim;
	RuntimeAnim? overrideAnim;
	#endregion // Fields

	#region Properties
	public SpriteRenderer rend
	{
		get { return _rend; }
		private set { _rend = value; }
	}
	#endregion // Properties

	#region Mono
	//protected void OnValidate()
	//{
	//	if(Game.isSetup)
	//	{
	//		UpdateAnimDict();
	//	}
	//}
	#endregion // Mono

	#region Methods
	#region Interface
	public void PlayAnimation(string name)
	{
		overrideAnim = GetAnim(name);
	}

	public void StopAnimation(string name)
	{
		if(overrideAnim != null && overrideAnim.Value.def.name == name)
		{
			overrideAnim = null;
		}
	}

    public void SetDefaultAnimation(string name)
	{
		if(defaultAnim != null && defaultAnim.Value.def.name == name)
		{
			return;
		}

		defaultAnim = GetAnim(name);
	}

	public void SetDirection(int direction)
	{
		Vector3 s = rend.transform.localScale;
		int sign = Math.Sign(s.x);
		if(sign != direction)
		{
			s.x = -s.x;
			rend.transform.localScale = s;
		}

	}

	public void AddAnimation(string name, bool looping, Sprite[] sprites)
	{
		runtimeAnims[name] = new AnimationDef
		{
			name = name,
			looping = looping,
			sprites = sprites
		};
    }
	#endregion // Interface

	#region System
	//public static void SystemUpdate()
	//{
	//	for(int i = 0; i < activeAnimators.Count; ++i)
	//	{
	//		SpriteAnimator anim = activeAnimators[i];
	//		anim.DoUpdate();
 //       }
 //   }

	void Update()
	{
		if(fps <= 0) { return; }

		UpdateAnimation(ref defaultAnim);

		if(UpdateAnimation(ref overrideAnim))
		{
			overrideAnim = null;
		}
	}

	bool UpdateAnimation(ref RuntimeAnim? anim)
	{
		if(anim == null) { return false; }

		float step = 1.0f / (float)fps;

		var val = anim.Value;
		val.timeSinceLastFrame += Time.deltaTime;

		if(val.timeSinceLastFrame >= step)
		{
			val.timeSinceLastFrame = 0.0f;
			++val.frame;
			if(val.frame >= val.def.sprites.Length)
			{
				if(!val.def.looping) { return true; }

				val.frame = 0;
			}
		}

		if(val.def.sprites.Length > 0)
		{
			rend.sprite = val.def.sprites[val.frame];
		}

		anim = val;

		return false;
	}
	#endregion // System

	AnimationDef FindOrNull(Dictionary<string, AnimationDef> dict, string key)
	{
		AnimationDef val = null;
		dict.TryGetValue(key, out val);
		return val;
    }

	RuntimeAnim? GetAnim(string name)
	{
		AnimationDef def = FindOrNull(nameToAnim, name);
		if(def == null)
		{
			def = FindOrNull(runtimeAnims, name);
		}

		if(def == null) { return null; }

		return new RuntimeAnim
		{
			def = def
		};
	}

	protected void Awake()
	{
		nameToAnim = new Dictionary<string, AnimationDef>();
		UpdateAnimDict();

		SetDefaultAnimation(playAutomatically);
    }

	void UpdateAnimDict()
	{
		if(nameToAnim == null) { return; }

		nameToAnim.Clear();

		for(int i = 0; i < anims.Length; ++i)
		{
			AnimationDef anim = anims[i];
			nameToAnim[anim.name] = anim;
		}
	}
	#endregion // Methods
}
