using UnityEngine;

public class ObjectInstantiator : IObjectInstantiator {

	/* This wrapper is mostly to support unit testing without calling unity's static API */
	public T Instantiate<T>(T prefab) where T : Object {
		return Object.Instantiate(prefab);
	}

	public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object {
		return Object.Instantiate(prefab, position, rotation);
	}
}