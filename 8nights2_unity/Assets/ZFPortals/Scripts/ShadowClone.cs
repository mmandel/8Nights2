using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;


namespace ZenFulcrum.Portal { 

/**
 * Mimics the appearance of an object.
 * 
 * When an object straddles a portal, the polygons will normally only be rendered on one side, causing visual glitches.
 * To combat this, we use Tobirama Senju's technique, automatically, to create something that looks 
 * identical to the original, but on the other side of the portal, causing a seamless appearance.
 */
public class ShadowClone : MonoBehaviour {
	public Portal portal;
	public GameObject realObject;

	private static int id;

	/** 
	 * Creates and returns a new ShadowClone that will mimic the appearance of the given object on the other side of 
	 * the given portal. 
	 */
	public static ShadowClone Create(Transform realObject, Portal portal) {
		if (!Application.isPlaying) {
			Debug.LogWarning("FIXME: Attempting to create ShadowClone in edit mode");
			return null;
		}

		var name = realObject.name + " Portal Clone for " + portal.name + " " + (++id);

		var cloneGO = new GameObject(name);

		cloneGO.hideFlags = HideFlags.NotEditable;
		//note: don't add the "don't save" flag; we don't run in edit mode and setting the "don't save" flag really confuses the editor in this case.

		var clone = cloneGO.AddComponent<ShadowClone>();
		clone.portal = portal;
		clone.realObject = realObject.gameObject;
		clone.transform.localScale = realObject.lossyScale;

		//Also clone current position (while building the object tree) until we Update() to our real location.
		clone.transform.position = realObject.position;
		clone.transform.rotation = realObject.rotation;

		CopyLook(realObject.gameObject, cloneGO);
		CloneChildren(realObject.gameObject, cloneGO);

		return clone;
	}

	/**
	 * Copies the appearance of an existing GO to another blank GO (and nothing else!) 
	 */
	protected static void CopyLook(GameObject src, GameObject dest) {
		if (!src.GetComponent<Renderer>()) return;

		var origMR = src.GetComponent<MeshRenderer>();
		if (origMR) {
			var cloneMR = dest.AddComponent<MeshRenderer>();
#if UNITY_5
			cloneMR.shadowCastingMode = origMR.shadowCastingMode;
			cloneMR.reflectionProbeUsage = origMR.reflectionProbeUsage;
#else
			cloneMR.castShadows = origMR.castShadows;
#endif

			cloneMR.receiveShadows = origMR.receiveShadows;
			cloneMR.useLightProbes = origMR.useLightProbes;
			cloneMR.materials = (Material[])origMR.materials.Clone();
		}

		var origMF = src.gameObject.GetComponent<MeshFilter>();
		if (origMF) {
			var cloneMF = dest.AddComponent<MeshFilter>();
			cloneMF.mesh = origMF.mesh;
		}
	}

	protected static void CloneChildren(GameObject src, GameObject dest) {
		foreach (Transform srcChild in src.transform) {
			var destChild = new GameObject(srcChild.name + " Portal Clone");
			destChild.hideFlags = HideFlags.NotEditable;
			destChild.transform.parent = dest.transform;

			destChild.transform.localScale = srcChild.localScale;
			destChild.transform.position = srcChild.position;
			destChild.transform.rotation = srcChild.rotation;

			CopyLook(srcChild.gameObject, destChild);
			CloneChildren(srcChild.gameObject, destChild);
		}
		
	}

	public void Update() {
		if (!portal || !realObject) {
			enabled = false;
			return;
		}

		portal.TeleportRelativeToDestination(realObject.transform, transform);
	}

	/** Causes this clone to be destroyed next fixed frame. */
	public void DestroyNextFixedFrame() {
		Destroy(this.gameObject, Time.fixedDeltaTime + .001f);
	}

}

}
