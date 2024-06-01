using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(LineRenderer))]
public class RaycastGun : MonoBehaviour
{
	//public GameObject gun;
	public ActionBasedController controller;
	public Transform laserOrigin;
	public float gunRange = 5f;
	public float fireSpread = 0.5f;
	public float laserDuration = 0.01f;
	public uint laserCount = 1;

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
		
		if (controller.activateAction.action.ReadValue<float>()>0)
		{
			fireTimer = 0;
			laserLine.SetPosition(0, laserOrigin.position);
			RaycastHit hit;

			for (int i = 0; i < laserCount; i++) 
			{
				if (Physics.Raycast(controller.transform.transform.position, 
					controller.transform.forward + new Vector3(Random.value * fireSpread - fireSpread / 2,
															   Random.value * fireSpread - fireSpread / 2,
															   Random.value * fireSpread - fireSpread / 2),
					out hit, gunRange))
				{
					laserLine.SetPosition(1, hit.point);
					Instantiate(LazerPoint, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
				}

				StartCoroutine(ShootLaser());
			}

		}
	}

	IEnumerator ShootLaser()
	{
		laserLine.enabled = true;
		yield return new WaitForSeconds(laserDuration);
		laserLine.enabled = false;
	}
}