using UnityEngine;
using UnityEngine.UI;
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
			if(networkManager.matchMaker == null)
			{
				networkManager.StartMatchMaker();
			}

			RefreshRoomList();
	}

	public void RefreshRoomList()
	{
			ClearRoomList();
			networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 1, OnMatchList);
			status.text = "Loading...";
	}

	public void OnMatchList (bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		status.text = "";
		if (matchList == null)
		{
			status.text = "Unable to connect to server";
			return;
		}


//		foreach(MatchDesc match in matchList.matches)
//		{
//			GameObject roomListItemGameObject = Instantiate (roomListItemPrefab);
//			roomListItemGameObject.transform.SetParent(roomListParent);
//
//			RoomListItem _roomListItem = roomListItemGameObject.GetComponent<RoomListItem>();
//			if(_roomListItem != null)
//			{
//				_roomListItem.Setup(match);
//			}
//
//			roomList.Add(roomListItemGameObject);
//		}
//
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

}
