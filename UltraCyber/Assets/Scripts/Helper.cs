using System;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    private static System.Random randy = new System.Random();

    public static System.Random Random
    {
        get { return randy; }
    }

    public static Color GetRandomColor()
    {
        return new Color((float)randy.NextDouble(), (float)randy.NextDouble(), (float)randy.NextDouble(), 1);
    }

    public static Color GetRandomColorWithSetAlpha(float anAlpha)
    {
        return new Color((float)randy.NextDouble(), (float)randy.NextDouble(), (float)randy.NextDouble(), anAlpha);
    }

    public static void SpecialErase<T>(ref List<T> list, int index)
    {
        list[index] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
    }

     public static void CreateDebugSphere(Vector3 aPos, Color aColor, float aScale = 0.25f)
    {
        GameObject spherie = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        spherie.transform.localScale = Vector3.one * aScale;
        spherie.GetComponent<Renderer>().material.color = aColor;
        spherie.transform.position = aPos;
    }

    public static void StartAllParticlesOnComponent(Transform aTrans)
    {
		if(aTrans == null)
		{
			Debug.LogError ("Tried starting particles on a null transform");
			return;
		}
		
        ParticleSystem[] particles = aTrans.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < particles.Length; ++i)
        {
            particles[i].Play();
        }

        ParticleEmitter[] particleEmittors = aTrans.GetComponentsInChildren<ParticleEmitter>();

        for (int i = 0; i < particleEmittors.Length; ++i)
        {
            particleEmittors[i].emit = true;
        }
    }

    public static void StopAllParticlesOnComponent(Transform aTrans)
    {
		if(aTrans == null)
		{
			Debug.LogError ("Tried stopping particles on a null transform");
			return;
		}
		
        ParticleSystem[] particles = aTrans.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < particles.Length; ++i)
        {
            particles[i].Stop();
        }

        ParticleEmitter[] particleEmittors = aTrans.GetComponentsInChildren<ParticleEmitter>();

        for (int i = 0; i < particleEmittors.Length; ++i)
        {
            particleEmittors[i].emit = false;
        }
    }

    public static void RemoveAllParticlesOnComponent(Transform aTrans)
    {
		if(aTrans == null)
		{
			Debug.LogError ("Tried removing particles on a null transform");
			return;
		}
		
        ParticleSystem[] particles = aTrans.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < particles.Length; ++i)
        {
            particles[i].Clear();
        }

        ParticleEmitter[] particleEmittors = aTrans.GetComponentsInChildren<ParticleEmitter>();

        for (int i = 0; i < particleEmittors.Length; ++i)
        {
            particleEmittors[i].ClearParticles();
        }
    }
	
	//add some safety to this?
//	public static Bounds GetScaledBoundsInWorld(GameObject aGameObject)
//	{
//		Bounds returnBounds = aGameObject.renderer.bounds;//GetComponent<MeshFilter>()	mesh.bounds;
//		
//	//	returnBounds.extents = Quaternion.
////		returnBounds.extents = aGameObject.transform.rotation.eulerAngles;
//		//returnBounds.extents = Vector3.Scale(returnBounds.extents, aGameObject.transform.rotation.eulerAngles);
//	//	returnBounds.extents = Vector3.Scale(returnBounds.extents,aGameObject.transform.localScale);//	1.5f;
//	//	returnBounds.center = aGameObject.transform.position;
//		return returnBounds;
//	}

    public static bool AreParticlesActiveOnComponent(Transform aTrans)
    {
		if(aTrans == null)
		{
			Debug.LogError ("Tried checking particles on a null transform");
			return false;
		}
		
        ParticleSystem[] particles = aTrans.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < particles.Length; ++i)
        {
            if (particles[i].IsAlive())
                return true;
        }

        ParticleEmitter[] particleEmitters = aTrans.GetComponentsInChildren<ParticleEmitter>();

        for (int i = 0; i < particleEmitters.Length; ++i)
        {
            if (particleEmitters[i].particleCount > 0)
                return true;
        }

        return false;
    }

    public static Transform FindChildTransformIncludeInactive(GameObject aParent, string aChildName)
    {
        Transform[] children = aParent.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.name == aChildName)
                return child;
        }
        return null;
    }
}