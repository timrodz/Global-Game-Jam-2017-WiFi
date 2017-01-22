using UnityEngine;
using System.Collections;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T: MonoBehaviour 
{
	static private T _instance;

	static public T Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<T>();
				if(_instance == null)
					_instance = (new GameObject(typeof(T).ToString())).AddComponent<T>();
			}				

			return _instance;
		}
	}
}