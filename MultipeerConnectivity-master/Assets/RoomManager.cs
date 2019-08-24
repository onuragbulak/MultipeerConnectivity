using GooglePlayGames;
using GooglePlayGames.BasicApi.Nearby;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using NearbyDroids;
using System.IO;

public class RoomManager : MonoBehaviour

{   public bool isClient
    {
        get;set;

    }
    NearbyRoom room;
    bool connected;

	public string myEndpointId { get; set;}    
	public string myName {get; set;}
	private Action<NearbyRoom, bool> roomDiscoveredCallback;
    
    //private static RoomListener roomListener = new RoomListener();
    // private readonly IDiscoveryListener listener;
    //  public InputField roomName, username;
    //private string localPlayerName;
    public const string RoomNameKey = "roomname";
    public const string PlayerNameKey = "playername";
    public Text  textObject;
	NearbyPlayer globalNearbyPlayer = null ;

    //private bool joining;
    void Awake()
    {
    
        DontDestroyOnLoad(gameObject);
        
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (connected == true) {
          //  if (globalNearbyPlayer != null) { NearbyRoom.LookupRoomByEndpoint(myEndpointId).MessageHandler.Invoke(globalNearbyPlayer, Encoding.ASCII.GetBytes("asdasd"));


            //}

            //PlayGamesPlatform.Nearby.SendReliable(new List<string>{NearbyRoom.endPoints }, Encoding.ASCII.GetBytes("awodkaodkapdkawopdkapdkpoakdpawkdpwakdoadkapdkaw"));
            
       // }
//        PlayGamesPlatform.Nearby.SendReliable(new List<string> { NearbyRoom.endPoints }, Encoding.ASCII.GetBytes("awodkaodkapdkawopdkapdkpoakdpawkdpwakdoadkapdkaw"));




    }
    public bool onDiscovery
    {
        get; set;
    }
    public bool onAdvertise
    {
        get; set;
    }
    public bool onFound

    { get; set; }
    public IMessageListener messageListener { get; private set; }

    public void createRoom(string rName, string uName)
    {

        NearbyController nearbyController = new NearbyController();
        nearbyController.Advertise(rName);
        return;
    }
    
    public void onCreateRoom()
    {
		myEndpointId = UnityEngine.Random.Range(1000, 10000).ToString();
		myName = UnityEngine.Random.Range (1000, 10000).ToString ();

		NearbyPlayer nearbyPlayer = new NearbyPlayer(SystemInfo.deviceUniqueIdentifier,myEndpointId,myName);
		globalNearbyPlayer = nearbyPlayer; 
        NearbyRoom.StopAll();
        PlayGamesPlatform.InitializeNearby((client) =>
        {
            Debug.Log("Nearby connections initialized");
        });
        
        NearbyRoom nearbyRoom = new NearbyRoom("Most Popular Room!!");
        nearbyRoom.AutoJoin = true;
        nearbyRoom.AlwaysOpen = true;
        isClient = false; 
        nearbyRoom.OpenRoom();

        
       
        //List<string> appIdentifiers = new List<string>();
        //appIdentifiers.Add(PlayGamesPlatform.Nearby.GetAppBundleId());
        //PlayGamesPlatform.Nearby.StartAdvertising(
        //          "Awesome Game Host",  // User-friendly name
        //            appIdentifiers,  // App bundle Id for this game
        //           TimeSpan.FromSeconds(0),// 0 = advertise forever
        //           (AdvertisingResult result) =>
        //           {

        //               Debug.Log("OnAdvertisingResult: " + result);
        //           },
        //              (ConnectionRequest request) =>
        //              {

        //                  Debug.Log("Received connection request: " +
        //                   request.RemoteEndpoint.ServiceId + " " +
        //                   request.RemoteEndpoint.EndpointId + " " +
        //                   request.RemoteEndpoint.Name);

        //              }
        //           );

    }

    byte[] bytes = Encoding.ASCII.GetBytes("asdasd");
    

    public void OnEndpointFound(EndpointDetails discoveredEndpoint)
    {
        Debug.Log("Found Endpoint: " +
                  discoveredEndpoint.ServiceId + " " +
                  discoveredEndpoint.EndpointId + " " +
                  discoveredEndpoint.Name);
        // textObject.
        GameManager gameManager = new GameManager();
        gameManager.SendMessageToChat("Found Endpoint: " +
                  discoveredEndpoint.ServiceId + " " +
                  discoveredEndpoint.EndpointId + " " +
                  discoveredEndpoint.Name, Message.MessageType.info);
        PlayGamesPlatform.Nearby.SendConnectionRequest(
               "Local Game player",  // the user-friendly name
               discoveredEndpoint.EndpointId,   // the discovered endpoint	
               bytes,  // byte[] of data
               (response) => {
                   Debug.Log("response: " +
                                response.ResponseStatus);
               },
               (IMessageListener)messageListener);

    }
    public void StartMultiplayer()
    {

       


    }
    
    public void StartDiscovery()
    {

        isClient = true;
        Debug.Log("StartDiscovery called");
        onStartDiscovery();
    }


    internal void onStartDiscovery()
    {

        NearbyRoom.StopAll();

		myEndpointId = UnityEngine.Random.Range(1000, 10000).ToString();
		myName = UnityEngine.Random.Range (1000, 10000).ToString ();
		NearbyPlayer nearbyPlayer = new NearbyPlayer(SystemInfo.deviceUniqueIdentifier,myEndpointId,myName);
		globalNearbyPlayer = nearbyPlayer; 
        NearbyRoom.FindRooms(OnRoomFound); 
        
        //PlayGamesPlatform.InitializeNearby((client) =>
        //{
        //    Debug.Log("Nearby connections initialized");
        //});
        //Debug.Log("onStartDiscovery called"); 
        //PlayGamesPlatform.Nearby.StartDiscovery(
        //                PlayGamesPlatform.Nearby.GetServiceId(),
        //                TimeSpan.FromSeconds(0),
        //                listener);
    }
      
    internal void OnRoomFound(NearbyRoom room, bool available)
    {

        if(available)

        {
			
		
	

		
			myEndpointId = UnityEngine.Random.Range(1000, 10000).ToString();
			myName = UnityEngine.Random.Range (1000, 10000).ToString ();
			NearbyPlayer nearbyPlayer = new NearbyPlayer(SystemInfo.deviceUniqueIdentifier,myEndpointId,myName);
            
            room.JoinRoom(nearbyPlayer, Encoding.ASCII.GetBytes("asdasd"), OnRoomJoined);
           
            // GameObject obj = Instantiate() as GameObject;
        }
    }
    
    internal void OnRoomJoined (ConnectionResponse response)
    {
        Debug.Log("OnRoomJoined Called status: " + response.ResponseStatus);
        if (response.ResponseStatus == ConnectionResponse.Status.Accepted)
        {
            // if we are connected, stop looking for rooms.
            NearbyRoom.StopRoomDiscovery();

            // the first payload is sent with the response so we can initialize
            // the game scene.
            OnMessageReceived(room.Address, response.Payload);
            textObject.text = room.Address  + " :  " + response.Payload.ToString() + response.RemoteEndpointId;
             connected = true;
        }
    }

    internal void OnMessageReceived(NearbyPlayer sender, byte[] data)
    {

		Debug.Log ("New Messaggeee!!!! "); 
		Debug.Log (data.ToString ());

        UpdateGameStateFromData(data);
      

    }
    internal void UpdateGameStateFromData(byte[] data)
    {
        MemoryStream ms = new MemoryStream(data);
        
        
    }

}
