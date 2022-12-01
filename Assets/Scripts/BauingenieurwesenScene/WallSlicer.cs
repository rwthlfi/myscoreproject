using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;

public class WallSlicer : MonoBehaviour
{
	[Header("Sliced Reference")]
	public Material capMaterial;

	// Start is called before the first frame update
	void Start()
    {
        
    }

	void Update()
	{
		/*
		if (Input.GetMouseButtonDown(0))
		{
			//DisabledBrickPhysics(false);
			SliceTheFirst();
		}
		else if (Input.GetMouseButtonDown(1))
		{
			SliceAll();
		}
		*/
	}


	/// <summary>
	/// only to slice the first obj detected
	/// </summary>
	public void SliceTheFirst()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.forward, out hit))
		{
			GameObject victim = hit.collider.gameObject;

			GameObject[] pieces = MeshCut.Cut(victim, transform.position, transform.right, capMaterial);

			if (!pieces[1].GetComponent<Rigidbody>())
				pieces[1].AddComponent<Rigidbody>();

			Destroy(pieces[1], 1);
		}
	}

	/// <summary>
	/// to slice all the object that is in Hitbox vicinity 
	/// </summary>
	public void SliceAll()
	{

		
		RaycastHit[] hits;
		hits = Physics.RaycastAll(transform.position, transform.forward, 100.0F);

		//print("hits " + hits.Length);

		for (int i = 0; i < hits.Length; i++)
		{
			RaycastHit hit = hits[i];
			GameObject victim = hit.collider.gameObject;

			GameObject[] pieces = MeshCut.Cut(victim, transform.position, transform.right, capMaterial);

			if (!pieces[1].GetComponent<Rigidbody>())
				pieces[1].AddComponent<Rigidbody>();

			Destroy(pieces[1], 1);
		}
		
		
	}


	void OnDrawGizmosSelected()
	{

		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
		Gizmos.DrawLine(transform.position, transform.position + -transform.up * 0.5f);

	}
}
