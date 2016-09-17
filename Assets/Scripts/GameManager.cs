using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public GameSettings gameSettings;

	void Awake()
	{
		if(instance != null)
		{
			Debug.Log("Multiple gamemanagers running");
		}
		else
		{
			instance = this;
		}
	}
	
	
	
	#region Player registering
	private static Dictionary<string, Player> players = new Dictionary<string, Player>();

	public static void RegisterPlayer(string networkID, Player player)
	{
		string playerID = "Player " + networkID;
		players.Add(playerID, player);
		player.transform.name = playerID;
	}

	public static void UnregisterPlayer(string playerID)
	{
		players.Remove(playerID);
	}

	public static Player GetPlayer(string playerID)
	{
		return players[playerID];
	}
	#endregion


//	void OnGUI()
//	{
//		GUILayout.BeginArea(new Rect(150, 150, 200, 400));
//
//		GUILayout.BeginVertical();
//
//		foreach(string playerID in players.Keys)
//		{
//			GUILayout.Label(playerID + ": " + players[playerID].transform.name);
//		}
//
//		GUILayout.EndVertical();
//
//		GUILayout.EndArea();
//	}

}
