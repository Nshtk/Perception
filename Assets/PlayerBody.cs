using UnityEngine;


public class PlayerBody : MonoBehaviour
{
	[SerializeField] private Transform player_head, player_feet;

	private void Awake()
	{

	}
	private void Start()
	{
		
	}
	private void Update()
	{
		Vector3 feet_position = new Vector3(player_head.localPosition.x, player_feet.localPosition.y, player_head.localPosition.z);
		if (Vector3.Distance(gameObject.transform.position, feet_position)>5)
			gameObject.transform.position = feet_position;
	}
}
