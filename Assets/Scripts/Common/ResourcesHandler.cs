using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesHandler : MonoBehaviour {
	// ----------------------------------------------------------------
	//  Instance
	// ----------------------------------------------------------------
	public static ResourcesHandler Instance; // There can only be one.
	private void Awake() {
		// T... two??
		if (Instance != null) {
			// THERE CAN ONLY BE ONE.
			DestroyImmediate(this.gameObject);
			return;
		}
		// There's no instance already...
		else {
			if (Application.isEditor) { // In the UnityEditor?? Look for ALL InputManagers! We'll get duplicates when we reload our scripts.
				ResourcesHandler[] allOthers = FindObjectsOfType<ResourcesHandler>();
				for (int i = 0; i < allOthers.Length; i++) {
					if (allOthers[i] == this) { continue; } // Skip ourselves.
					Destroy(allOthers[i].gameObject);
				}
			}
		}

		// There could only be one. :)
		Instance = this;
		//DontDestroyOnLoad(this.gameObject);
	}

	// Common
	[SerializeField] public GameObject go_CubeLine;
	// References
	[SerializeField] public GameObject Bullet;
	[SerializeField] public GameObject Ghoul;

}
