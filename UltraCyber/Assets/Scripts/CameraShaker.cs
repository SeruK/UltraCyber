using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {
	
	private float m_ShakeDecay;
	private float m_ShakeIntensity;
	Vector3 m_Shake = Vector3.zero;
	
	private static CameraShaker m_Instance = null;
	
	public static CameraShaker Instance
	{
		get
		{
			if (m_Instance == null)
				m_Instance = FindObjectOfType(typeof(CameraShaker)) as CameraShaker;
			
			return m_Instance;
		}
	}
	
	public void Shake(float anIntensity = 0.2f, float aDecay = 0.4f)
	{
		m_ShakeIntensity = anIntensity;
		m_ShakeDecay = aDecay;
	}
	
	void Update () {

		if (m_ShakeIntensity > 0)
		{
			transform.position -= m_Shake;
			m_Shake = Random.insideUnitSphere * m_ShakeIntensity;
			m_Shake.z = 0;
			transform.position += m_Shake;
			
			Quaternion rotation = transform.rotation;
			
			transform.rotation.Set(
				rotation.x + Random.Range(-m_ShakeIntensity, m_ShakeIntensity) * Time.deltaTime,
				rotation.y + Random.Range(-m_ShakeIntensity, m_ShakeIntensity) * Time.deltaTime,
				0,//rotation.z + Random.Range(-m_ShakeIntensity, m_ShakeIntensity) * Time.deltaTime,
				rotation.w + Random.Range(-m_ShakeIntensity, m_ShakeIntensity) * Time.deltaTime);
			
			m_ShakeIntensity -= m_ShakeDecay * Time.deltaTime;
		}
	}
}
