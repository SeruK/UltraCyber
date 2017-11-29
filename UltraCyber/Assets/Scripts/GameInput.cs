using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections.Generic;
#if USE_REWIRED
using RE = Rewired;
#endif // USE_REWIRED

public static class GameInput
{
//#if UNITY_STANDALONE_OSX
//	public enum Xbox360Button
//	{
//		A = 16,
//		B = 17,
//		X = 18,
//		Y = 19,
//		LB = 4,
//		RB = 5,
//		Back = 10,
//		Start = 9
//	}
//#else
	public enum Xbox360Button
	{
		A = 0,
		B = 1,
		X = 2,
		Y = 3,
		LT = 4,
		RT = 5,
		Back = 6,
		Start = 7
	}
//#endif

	public enum Xbox360Axis
	{
		LeftX,
		LeftY,
		RightX,
		RightY,
		LT,
		RT,
		DpadX,
		DpadY
	}

	public enum Button
	{
		Jump,
		Shoot,
		Restart,
		ReturnMainMenu,
	}

	public enum Axis
	{
		MoveHorizontal,
		AimVertical,
	}

	#region Interface
	public static bool GetAnyButtonDown()
	{
#if USE_REWIRED
		for(int i = 0; i < 2; ++i)
		{
			RE.Player player = RE.ReInput.players.GetPlayer(i);
			if(player.GetAnyButtonDown()) { return true; }
		}

		return false;
#else // USE_REWIRED
		if(Input.anyKeyDown) { return true; }

		for(uint i = 0; i < 2; ++i)
		{
			bool anyDown =
				GameInput.GetXboxButton(i, GameInput.Xbox360Button.A) ||
				GameInput.GetXboxButtonDown(i, GameInput.Xbox360Button.B) ||
				GameInput.GetXboxButtonDown(i, GameInput.Xbox360Button.Start)
			;
			if(anyDown) { return true; }
        }

		return false;
#endif // USE_REWIRED
    }

	public static int GetAxis(uint index, Axis axis)
	{
#if USE_REWIRED
		RE.Player player = RE.ReInput.players.GetPlayer((int)index);
		float value = player.GetAxis(AxisToRewiredAction(axis));
		return value < 0.0f ? -1 : value > 0.0f ? 1 : 0;
#else // USE_REWIRED
		switch(axis)
		{
		case Axis.MoveHorizontal:
			if(HoriDirKeyHeld(index)) { return GetHoriKeyDir(index); }
			break;
		case Axis.AimVertical:
			if(VertDirKeysHeld(index)) { return GetVertKeyDir(index); }
			break;
		}

		Xbox360Axis? axis360 = AxisTo360(axis);
		return axis360 == null ? 0 : GetXboxAxis(index, axis360.Value);
#endif // USE_REWIRED
	}

	public static bool GetButton(uint index, Button button)
	{
#if USE_REWIRED
		RE.Player player = RE.ReInput.players.GetPlayer((int)index);
		return player.GetButton(ButtonToRewiredAction(button));
#else // USE_REWIRED
		if(Input.GetKey(ButtonToKeyCode(index, button))) { return true; }

		Xbox360Button? btn360 = ButtonTo360(button);
		return btn360 == null ? false : GetXboxButton((uint)index, btn360.Value);
#endif // USE_REWIRED
    }

	public static bool GetButtonDown(uint index, Button button)
	{
#if USE_REWIRED
		RE.Player player = RE.ReInput.players.GetPlayer((int)index);
		return player.GetButtonDown(ButtonToRewiredAction(button));
#else // USE_REWIRED
		if(Input.GetKeyDown(ButtonToKeyCode(index, button))) { return true; }

		Xbox360Button? btn360 = ButtonTo360(button);
		return btn360 == null ? false : GetXboxButtonDown((uint)index, btn360.Value);
#endif // USE_REWIRED
	}

	public static bool GetButtonUp(uint index, Button button)
	{
#if USE_REWIRED
		RE.Player player = RE.ReInput.players.GetPlayer((int)index);
		return player.GetButtonUp(ButtonToRewiredAction(button));
#else // USE_REWIRED

		if(Input.GetKeyUp(ButtonToKeyCode(index, button))) { return true; }

		Xbox360Button? btn360 = ButtonTo360(button);
		return btn360 == null ? false : GetXboxButtonUp((uint)index, btn360.Value);
#endif // USE_REWIRED
	}
#endregion // Interface

#region Mapping
#if USE_REWIRED
	static string AxisToRewiredAction(Axis axis)
	{
		switch(axis)
		{
		case Axis.AimVertical: return "AimVertical";
		case Axis.MoveHorizontal: return "MoveHorizontal";
		default: throw new Exception("Unhandled axis");
		}
	}

	static string ButtonToRewiredAction(Button button)
	{
		switch(button)
		{
		case Button.Jump: return "Jump";
		case Button.Shoot: return "Shoot";
		case Button.Restart: return "Restart";
		case Button.ReturnMainMenu: return "ReturnMainMenu";
		default: throw new Exception("Unhandled button");
		}
	}
#endif // USE_REWIRED

	static Xbox360Axis? AxisTo360(Axis axis)
	{
		switch(axis)
		{
		case Axis.AimVertical: return Xbox360Axis.DpadY;
		case Axis.MoveHorizontal: return Xbox360Axis.DpadX;
		default: throw new Exception("Unhandled axis");
		}
	}

	static Xbox360Button? ButtonTo360(Button button)
	{
		switch(button)
		{
		case Button.Jump: return Xbox360Button.A;
		case Button.Shoot: return Xbox360Button.B;
		case Button.Restart: return Xbox360Button.Start;
		case Button.ReturnMainMenu: return Xbox360Button.Back;
		}

		return null;
	}

	static KeyCode ButtonToKeyCode(uint player, Button button)
	{
		switch(button)
		{
		case Button.Jump: return player == 0 ? KeyCode.F : KeyCode.K;
		case Button.Shoot: return player == 0 ? KeyCode.G : KeyCode.L;
		case Button.Restart: return KeyCode.R;
		case Button.ReturnMainMenu: return KeyCode.Escape;
		default: throw new Exception("Unhandled Button");
		}
	}

	static bool HoriDirKeyHeld(uint player)
	{
		return player == 0 ?
			(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) :
				(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow));
	}

	static bool VertDirKeysHeld(uint player)
	{
		return player == 0 ?
			(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) :
				(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow));
	}

	static int GetHoriKeyDir(uint player)
	{
		return player == 0 ?
			(Input.GetKey(KeyCode.A) ? -1 : 1) :
				(Input.GetKey(KeyCode.LeftArrow) ? -1 : 1);
	}

	static int GetVertKeyDir(uint player)
	{
		return player == 0 ?
			(Input.GetKey(KeyCode.W) ? 1 : -1) :
				(Input.GetKey(KeyCode.UpArrow) ? 1 : -1);
	}
#endregion // Mapping

	public static float GetXboxAxisValue(uint in_index, Xbox360Axis in_axis)
	{
		return Input.GetAxis(GetXboxAxisName(in_index, in_axis));
	}

	public static int GetXboxAxis(uint in_index, Xbox360Axis in_axis)
	{
		float val = GetXboxAxisValue(in_index, in_axis);
		return Mathf.Approximately(val, 0.0f) ? 0 : Mathf.CeilToInt(val);
	}

	private static KeyCode GetXboxKeyCode(uint in_index, Xbox360Button in_button)
	{
		return GetJoystickKey(in_index, (uint)in_button);
	}

	public static bool GetXboxButton(uint in_index, Xbox360Button in_button)
	{
		return Input.GetKey(GetXboxKeyCode(in_index, in_button));
	}

	public static bool GetXboxButtonDown(uint in_index, Xbox360Button in_button)
	{
		return Input.GetKeyDown(GetXboxKeyCode(in_index, in_button));
	}

	public static bool GetXboxButtonUp(uint in_index, Xbox360Button in_button)
	{
		return Input.GetKeyUp(GetXboxKeyCode(in_index, in_button));
	}

	/*********************************************/

	private static KeyCode GetJoystickKey(uint in_index, uint in_buttonNumber)
	{
		DebugUtil.Assert(in_index <= 3);
		DebugUtil.Assert(in_buttonNumber <= 19);

		string str = "Joystick" + (in_index + 1u) + "Button" + in_buttonNumber;
		return (KeyCode)System.Enum.Parse(typeof(KeyCode), str);
	}

	private static string GetXboxAxisName(uint in_index, Xbox360Axis in_axis)
	{
		string str = null;
		switch(in_axis)
		{
		case Xbox360Axis.LeftX:
			str = "L_XAxis_";
			break;
			
		case Xbox360Axis.LeftY:
			str = "L_YAxis_";
			break;
			
		case Xbox360Axis.RightX:
			str = "R_XAxis_";
			break;
			
		case Xbox360Axis.RightY:
			str = "R_YAxis_";
			break;
			
		case Xbox360Axis.LT:
			str = "TriggersL_";
			break;
			
		case Xbox360Axis.RT:
			str = "TriggersR_";
			break;
			
		case Xbox360Axis.DpadX:
			str = "DPad_XAxis_";
			break;
			
		case Xbox360Axis.DpadY:
			str = "DPad_YAxis_";
			break;
			
		default:
			DebugUtil.Assert(false);
			break;
		}
		return str + (in_index + 1u);
	}
}
