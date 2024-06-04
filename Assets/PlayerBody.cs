using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerBody : MonoBehaviour
{
	[SerializeField] private Transform player_head, player_feet;
	[SerializeField] private DynamicMoveProvider move_provider;

	private void Update()
	{
		if (new Vector3(player_head.localPosition.x, 0, player_head.localPosition.z).magnitude>0.1)
		{
			move_provider.MoveRigCustom(new Vector3(0, 0.001f, 0));
			player_head.localPosition = Vector3.zero;
		}
	}
}
