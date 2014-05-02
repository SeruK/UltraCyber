using UnityEngine;

public static class GameInput
{
#if UNITY_STANDALONE_OSX

#else
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
#endif

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

	public static float GetXboxAxisValue(uint in_index, Xbox360Axis in_axis)
	{
		return Input.GetAxis(GetXboxAxisName(in_index, in_axis));
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
