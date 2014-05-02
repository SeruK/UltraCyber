#define DEBUG

using System.Diagnostics;

public static class DebugUtil
{
	public static void Assert(bool in_condition)
	{
		if (!in_condition)
		{
			throw new System.Exception();
		}
	}

	public static void Assert(bool in_condition, System.Object in_msg)
	{
		if (!in_condition)
		{
			throw new System.Exception("" + in_msg);
		}
	}

	[Conditional("DEBUG")]
	public static void Log(System.Object in_msg)
	{
		UnityEngine.Debug.Log(in_msg);
	}

	[Conditional("DEBUG")]
	public static void LogIf(bool in_condition, System.Object in_msg)
	{
		if (in_condition)
			Log(in_msg);
	}

	[Conditional("DEBUG")]
	public static void LogWarn(System.Object in_msg)
	{
		UnityEngine.Debug.LogWarning(in_msg);
	}

	[Conditional("DEBUG")]
	public static void LogError(System.Object in_msg)
	{
		UnityEngine.Debug.LogError(in_msg);
	}
}