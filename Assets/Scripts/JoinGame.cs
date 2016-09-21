using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

	List<GameObject> roomList = new List<GameObject>();

	[SerializeField]
	private Text status;

	[SerializeField]
	private GameObject roomListItemPrefab;

	[SerializeField]
	private Transform roomListParent; 

	private NetworkManager networkManager;

	void Start ()
	{
		networkManager = NetworkManager.singleton;
		InitialiseMatchMaker();
		RefreshRoomList();

	}

	void InitialiseMatchMaker ()
	{
		if(networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}
	}

	public void RefreshRoomList()
	{
		ClearRoomList();

		InitialiseMatchMaker();

		networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
		status.text = "Loading...";

	}

	public void OnMatchList (bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		status.text = "";
		if (!success || matchList == null)
		{
			status.text = "Unable to connect to server";
			return;
		}


		foreach(MatchInfoSnapshot match in matchList)
		{
			GameObject roomListItemGameObject = Instantiate (roomListItemPrefab);
			roomListItemGameObject.transform.SetParent(roomListParent);

			RoomListItem _roomListItem = roomListItemGameObject.GetComponent<RoomListItem>();
			if(_roomListItem != null)
			{
				_roomListItem.Setup(match, JoinRoom);
			}

			roomList.Add(roomListItemGameObject);
		}

		if(roomList.Count == 0)
		{
			status.text = "No rooms at present.";
		}

	}

	void ClearRoomList ()
	{
		for(int i = 0; i < roomList.Count; i++)
		{
			Destroy(roomList[i]);
		}

		roomList.Clear();
	}

	public void JoinRoom (MatchInfoSnapshot _match)
	{
		networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
		StartCoroutine(WaitForJoin());
	}

	IEnumerator WaitForJoin ()
	{
		ClearRoomList();
		status.text = "Joining...";

		int countdown = 10;
		while (countdown > 0)
		{
			status.text = "Joining... (" + countdown + ")";

			yield return new WaitForSeconds(1f);

			countdown --;
		}

		// Failed to connect
		status.text = "Failed to connect";
		yield return new WaitForSeconds(1f);

		MatchInfo matchInfo = networkManager.matchInfo;
		if(matchInfo != null)
		{
			networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
			networkManager.StopHost();
		}

		RefreshRoomList();
	}
}
