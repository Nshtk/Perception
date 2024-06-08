using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(LineRenderer))]
public class RaycastGun : MonoBehaviour
{
	[SerializeField] private ActionBasedController _controller;

	[SerializeField] private Transform _laser_origin;
	[SerializeField] private GameObject _lazer_point;
	[SerializeField] private float _laser_range = 5f;
	[SerializeField] private float _laser_spread = 0.5f;
	[SerializeField] private float _laser_line_lifetime = 0.01f;
	[SerializeField] private uint _laser_lines_count_alternative_mode = 20;
	[SerializeField] private float _laser_scan_radius_alternative_mode_horizontal = 1f, _laser_scan_radius_alternative_mode_vertical = 1f;
	[SerializeField] private LineRenderer _line_renderer_prefab;
	private List<LineRenderer> _laser_lines = new List<LineRenderer>();
	private bool _is_laser_lines_rendering_finished=true;

	float offset_alternative_x, offset_alternative_y;

	void Awake()
	{
		offset_alternative_x = -_laser_scan_radius_alternative_mode_horizontal;
		offset_alternative_y = _laser_scan_radius_alternative_mode_vertical;
		for (int i = 0; i < _laser_lines_count_alternative_mode; i++)
		{
			_laser_lines.Add(Instantiate(_line_renderer_prefab));
			_laser_lines[i].enabled = false;
		}
	}
	void Update()
	{
		if (!_is_laser_lines_rendering_finished)
			return;
		if (_controller.activateAction.action.ReadValue<float>()>0)
		{
			float offset = _laser_spread / 2;
			_laser_lines[0].SetPosition(0, _laser_origin.position);
			if(shootLaserPoint(_laser_lines[0], new Vector3(Random.value * _laser_spread - offset, Random.value * _laser_spread - offset, Random.value * _laser_spread - offset)))
			{
				_is_laser_lines_rendering_finished = false;
				StartCoroutine(drawLaserLines(_laser_lines, 1));
			}
		}
		if (_controller.selectAction.action.ReadValue<float>() > 0)
		{
			if (offset_alternative_x < _laser_scan_radius_alternative_mode_horizontal)
			{
				float laser_line_direction_delta_y = _laser_scan_radius_alternative_mode_vertical*2 / _laser_lines_count_alternative_mode;
				float start_point_horizontal = _laser_scan_radius_alternative_mode_vertical;
				offset_alternative_x += 0.1f;
				for (int i = 0; i < _laser_lines_count_alternative_mode; i++)
				{
					_laser_lines[i].SetPosition(0, _laser_origin.position);
					shootLaserPoint(_laser_lines[i], new Vector3(offset_alternative_x, start_point_horizontal, 0));	//Надо что-то придумать с z координатой
					start_point_horizontal -= laser_line_direction_delta_y;
				}
			}
			else if (offset_alternative_y > -_laser_scan_radius_alternative_mode_vertical)		//Some boilerplate
			{
				float laser_line_direction_delta_x = _laser_scan_radius_alternative_mode_horizontal*2 / _laser_lines_count_alternative_mode;
				float start_point_vertical = -_laser_scan_radius_alternative_mode_horizontal;
				offset_alternative_y -= 0.1f;
				for (int i = 0; i < _laser_lines_count_alternative_mode; i++)
				{
					_laser_lines[i].SetPosition(0, _laser_origin.position);
					shootLaserPoint(_laser_lines[i], new Vector3(start_point_vertical, offset_alternative_y, 0));	//Здесь тоже
					start_point_vertical += laser_line_direction_delta_x;
				}
			}
			else
			{
				offset_alternative_x = -_laser_scan_radius_alternative_mode_horizontal;
				offset_alternative_y = _laser_scan_radius_alternative_mode_vertical;
			}
			_is_laser_lines_rendering_finished = false;
			StartCoroutine(drawLaserLines(_laser_lines, _laser_lines.Count));
		}
	}

	IEnumerator drawLaserLines(List<LineRenderer> laser_lines, int laser_lines_count)
	{
		if(laser_lines_count>laser_lines.Count)
			laser_lines_count=laser_lines.Count;

		for (int i=0; i < laser_lines_count; i++)
			laser_lines[i].enabled = true;
		yield return new WaitForSeconds(_laser_line_lifetime);
		for (int i = 0; i < laser_lines_count; i++)
			laser_lines[i].enabled = false;
		_is_laser_lines_rendering_finished = true;

	}
	public bool shootLaserPoint(LineRenderer laser_line, Vector3 shoot_direction)
	{
		RaycastHit raycast_hit;
		bool result = Physics.Raycast(_laser_origin.position, (_laser_origin.transform.forward + shoot_direction).normalized, out raycast_hit, _laser_range);
		//Debug.Log($"FORWARD:{_controller.transform.forward}, SHOOT:{shoot_direction}, DIRECTION:{(_laser_origin.transform.forward + shoot_direction).normalized}");	//В помощь
		if (result)
		{
			laser_line.SetPosition(1, raycast_hit.point);
			Instantiate(_lazer_point, raycast_hit.point, Quaternion.FromToRotation(Vector3.up, raycast_hit.normal));
		}
		return result;
	}
}