using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(LineRenderer))]
public class RaycastGun : MonoBehaviour
{
	//public GameObject gun;
	public ActionBasedController controller;
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
		
		if (controller.activateAction.action.ReadValue<float>()>0 && fireTimer > fireRate)
		{
			fireTimer = 0;
			laserLine.SetPosition(0, laserOrigin.position);
			Vector3 spread = new Vector3(Random.value * 0.5f - 0.25f, Random.value * 0.5f - 0.25f, Random.value * 0.5f - 0.25f);
			Vector3 direction = controller.transform.forward + spread;
			RaycastHit hit;
			if (Physics.Raycast(controller.transform.transform.position, direction, out hit, gunRange))
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