//
//  Controls the torch
//

using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class Nights2Torch : MonoBehaviour 
{
    public string TorchOnBool = "on";
    public string TorchHasMagicBool = "has_magic";
    [Space(10)]
    public Renderer TorchColorRend;
    public string TorchColorProp = "_Color";

    [Space(10)]

    //physical torch stuff
    public int XBeeComPort = 2; //com port number to open
    public float XBeeTransmitInterval = 50.0f; //in milliseconds, how often we send

    private SerialPort _xbeeCom = null;
    private float _lastXBeeWrite = 0.0f;


    private Animator _animator = null;
    private Color _offFlameColor = Color.red;

	 void Start () 
    {
        _animator = gameObject.GetComponent<Animator>();
        SetAnimatorBool(TorchOnBool, false);

        //save off default color to shove on torch when its off
        if (TorchColorRend != null)
            _offFlameColor = TorchColorRend.material.GetColor(TorchColorProp);
	     
        //start connection over XBee to physical torch
        _xbeeCom = new SerialPort("COM" + XBeeComPort, 9600, Parity.None, 8, StopBits.One);
        OpenXBeeConnection();
    }

    void OpenXBeeConnection()
    {
       if (_xbeeCom != null)
       {
          if (_xbeeCom.IsOpen)
          {
             _xbeeCom.Close();
             Debug.Log("Closing XBee port, because it was already open!");
          }
          else
          {
             try
             {
                _xbeeCom.Open();  // opens the connection
                _xbeeCom.ReadTimeout = 50;  // sets the timeout value before reporting error
                Debug.Log("XBee Port Opened!");
             }
             catch (System.Exception e) 
             {
                Debug.LogWarning("Error opening XBee port: " + e.Message);
             }
          }
       }
       else
       {
          if (_xbeeCom.IsOpen)
             Debug.Log("XBee port is already open");
          else
             Debug.Log("XBee port == null");
       }
    }

    void OnApplicationQuit()
    {
       if(_xbeeCom != null)
          _xbeeCom.Close();
    }

    void SetAnimatorBool(string boolName, bool val)
    {
        if (_animator != null)
            _animator.SetBool(boolName, val);
    }

    bool IsFlameOn()
    {
        return (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon) || (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearBeacon);
    }

    bool ShowMagic()
    {
       return Nights2Mgr.Instance.TorchHasMagic() && IsFlameOn();
    }
	
	void Update () 
    {
        //update animator with status of torch
        SetAnimatorBool(TorchOnBool, IsFlameOn());

        SetAnimatorBool(TorchHasMagicBool, ShowMagic());

        if (TorchColorRend != null)
        {
            Color nextCandleColor = (Nights2Mgr.Instance.NextBeacon() != null) ? Nights2Mgr.Instance.NextBeacon().CandleColor : _offFlameColor;
            TorchColorRend.material.SetColor(TorchColorProp, IsFlameOn() ? nextCandleColor : _offFlameColor);
        }

	    //make sure we are parented to the right thing
        if (Nights2CamMgr.Instance != null)
        {
           Transform desiredParent = ((Nights2CamMgr.Instance.GetTorchParent() != null) && Nights2CamMgr.Instance.GetTorchParent().gameObject.activeInHierarchy) ? Nights2CamMgr.Instance.GetTorchParent() : null;
           if (transform.parent != desiredParent)
            {
                transform.parent = desiredParent;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }

        UpdateXBeeState();
	}

   void UpdateXBeeState()
   {
      if ((_xbeeCom == null) || !_xbeeCom.IsOpen)
      {
         return;
      }

      //has it been long enough?
      float elapsed = Time.time - _lastXBeeWrite;
      if (elapsed * 1000.0f < XBeeTransmitInterval)
      {
         return;
      }
      _lastXBeeWrite = Time.time;

      if (IsFlameOn())
      {
         int numCandlesOn = Nights2Mgr.Instance.NumCandlesLit();

         //letters A through H say that magic is on, for a particular part of the 8 step progression
         if (ShowMagic())
         {
            char magic = (char)((int)'A' + numCandlesOn);
            //Debug.Log("XBEE: " + magic.ToString());
            _xbeeCom.Write(magic.ToString());
         }
         //letters 1 through 9 say torch is on (without magic), for a particular part of the 8 step progression
         else
         {
            char magic = (char)((int)'1' + numCandlesOn);
            //Debug.Log("XBEE: " + magic.ToString());
            _xbeeCom.Write(magic.ToString());
         }
      }
      else //torch off
      {
         //Debug.Log("XBEE: 0" );
         _xbeeCom.Write("0");
      }
   }
}
