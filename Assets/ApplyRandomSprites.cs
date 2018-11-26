using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyRandomSprites : MonoBehaviour {

    [SerializeField]
    Sprite[] sprites;

	// Use this for initialization
	void Start () {
        sprites = Resources.LoadAll<Sprite>("Graphics/Planets");
        ApplySprites();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ApplySprites()
    {
        foreach(Transform t in transform)
        {
            int i = Random.Range(0, 4);
            t.GetComponent<SpriteRenderer>().sprite = sprites[i];
            }
    }
}
