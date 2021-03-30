using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour,IPoolObject {

	public float lifetime = 2.0f;

	public void OnObjectSpawn()
    {
		Invoke("Close", lifetime);
	}
	void Close()
	{
		gameObject.SetActive(false);
	}
}
