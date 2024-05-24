using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

[RequireComponent(typeof(LineRenderer))]
public class RaycastGun : MonoBehaviour
{
	public GameObject controller;
	public Transform laserOrigin;
	public float gunRange = 5f;
	public float fireRate = 0.002f;
	public float laserDuration = 0.01f;

	public GameObject LazerPoint;

	LineRenderer laserLine;
	float fireTimer;

	void Awake()
	{
		laserLine = GetComponent<LineRenderer>();
	}

	void Update()
	{	

		fireTimer += Time.deltaTime;
		
		while (fireTimer > fireRate)
		{
			fireTimer = 0;

			
			
			laserLine.SetPosition(0, laserOrigin.position);
			Vector3 rayOrigin = controller.transform.transform.position;
			Vector3 spread = new Vector3(Random.value * 0.5f - 0.25f, Random.value * 0.5f - 0.25f, Random.value * 0.5f - 0.25f);
			Vector3 direction = controller.transform.forward + spread;
			RaycastHit hit;
			if (Physics.Raycast(rayOrigin, direction, out hit, gunRange))
			{
				laserLine.SetPosition(1, hit.point);
				Instantiate(LazerPoint, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
				
			}
			//else
			//{
			//	laserLine.SetPosition(1, rayOrigin + (direction * gunRange));
			//}
			StartCoroutine(ShootLaser());
		}
	}

	IEnumerator ShootLaser()
	{
		laserLine.enabled = true;
		yield return new WaitForSeconds(laserDuration);
		laserLine.enabled = false;
	}
}