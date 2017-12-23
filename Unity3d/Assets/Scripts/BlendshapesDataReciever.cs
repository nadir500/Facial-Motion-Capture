using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.Text.RegularExpressions;
public class BlendshapesDataReciever : MonoBehaviour {

	// Use this for initialization

	Thread receiveThread;
	UdpClient client;
	public int port;
	public LandmarkCoordinates[] landmarks;
	//info
    private int x,y;
	private int ntemp;
	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = "";

	void Start () {
		init();
		//ExtractResults();
	}
public void ExtractResults(string socket_string_input)
{
	 string str= "[[100 100]\n[200 10]\n[10 30]\n[20 20]]";
	
	 string[] the_coords= new string[68*2]; //landmarks x,y
	 String regex = @"[\[\]']+";
	 Regex regex_newline = new Regex("(\r\n|\r|\n)");   // remove the '+'

	string output1= Regex.Replace(socket_string_input,regex,"");

    string output2= regex_newline.Replace(output1, ",");
    string result = Regex.Replace(output2, @"(?<=\d)\p{Zs}(?=\d)", ",");

	string[] extracted_result= Regex.Split(result,",");
	landmarks= new LandmarkCoordinates[(extracted_result.Length/2)];
    //Debug.Log("landmarks length "+  landmarks.Length + " extracted result length " + extracted_result.Length);

	for (int i = 0; i < extracted_result.Length; i++)
	{
		ntemp = i+1;
		if (ntemp >= 0 && ntemp <= 68)
		{
		Int32.TryParse(extracted_result[i], out x);
		//Debug.Log(x);
		//x=float.Parse(extracted_result[i]);
		//Debug.Log(x);
	    Int32.TryParse(extracted_result[ntemp], out y);

		//y=float.Parse(extracted_result[ntemp]);
		//Debug.Log(y);
		landmarks[i]= new LandmarkCoordinates(x,y);
		}

	}
	//	Debug.Log(landmarks[0].x+ " , "  +landmarks[0].y );
//	return landmarks;
	 //landmark array returned to make its move on blendshapes :D 
}
	void OnGUI(){
		Rect  rectObj=new Rect (40,10,200,400);
		
		GUIStyle  style  = new GUIStyle ();
		
		style .alignment  = TextAnchor.UpperLeft;
		
		GUI .Box (rectObj,"# UDPReceive\n127.0.0.1 "+port +" #\n"
		          
		          //+ "shell> nc -u 127.0.0.1 : "+port +" \n"
		          
		          + "\nLast Packet: \n"+ lastReceivedUDPPacket
		          
		          //+ "\n\nAll Messages: \n"+allReceivedUDPPackets
		          
		          ,style );

	}

	private void init(){
		print ("UPDSend.init()");

		port = 5065;

		print ("Sending to 127.0.0.1 : " + port);

		receiveThread = new Thread (new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();

	}

	private void ReceiveData(){
		client = new UdpClient (port);
		while (true) {
			try{

				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
				byte[] data = client.Receive(ref anyIP);
				string text = Encoding.UTF8.GetString(data);
				
				//Debug.Log(text);
				// ExtractResults(text);
				//Debug.Log(landmarks[0].x+ " , "  +landmarks[1].y );
				lastReceivedUDPPacket=text;
				allReceivedUDPPackets=allReceivedUDPPackets+text;
				ExtractResults(lastReceivedUDPPacket);
				//xPos = float.Parse(text);
			//	xPos *= 0.021818f;
			}catch(Exception e){
				print (e.ToString());
			}
		}
	}

	public string getLatestUDPPacket(){
		allReceivedUDPPackets = "";
		return lastReceivedUDPPacket;
	}
	
	// Update is called once per frame
	void Update () {
	//	hero.transform.position = new Vector3(xPos-6.0f,-3,0);
	}

	void OnApplicationQuit(){
		if (receiveThread != null) {
			receiveThread.Abort();
			Debug.Log(receiveThread.IsAlive); //must be false
		}
	}
}
