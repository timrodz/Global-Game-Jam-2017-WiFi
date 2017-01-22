using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 public class TileTextureAnimator : MonoBehaviour 
{
     public float scrollSpeed = 0.5f;
	 public float Amplitude = 0.001f;

     public Renderer rend;

	 private float _baseOffset;

     void Start() {
         rend = GetComponent<Renderer>();
		_baseOffset = rend.material.mainTextureOffset.x;
     }

     void Update() {
		float offset = Time.time * scrollSpeed; 
		//rend.material.mainTextureOffset = new Vector2 (offset % 0.5f, 0);
		rend.material.mainTextureOffset = new Vector2(_baseOffset + Amplitude * Mathf.Sin(offset), 0);
     }
 }
