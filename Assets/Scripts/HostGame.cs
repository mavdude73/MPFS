﻿using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

	[SerializeField]
	private uint roomSize = 5;

	private string roomName;

	private NetworkManager networkManager;

	void Start ()
	{
		networkManager = NetworkManager.singleton;
		if(networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}
	}

	public void SetRoomName (string name)
	{
		roomName = name;
	}

	public void CreateRoom ()
	{
		if(roomName != "" && roomName != null)
		{
			networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
		}
	}

}
