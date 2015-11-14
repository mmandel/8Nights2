//
// Activate animator triggers in response to changes to the Nights2Mgr's state
//


using UnityEngine;
using System.Collections;

public class Nights2AnimTrigger : MonoBehaviour 
{
    [Tooltip("Passes the # of candles lit so far to this integer animator param")]
    public string NumCandlesLitParam = "candles_lit";

    [Space(10)]

    [Tooltip("Set this trigger on the animator when the player walks through an entrance portal")]
    public string PassedEntrancePortalTrigger = "entrance_portal";

    [Tooltip("Set this trigger on the animator when the player walks through an exit portal")]
    public string PassedExitPortalTrigger = "exit_portal";

    public string InAltWorld1Bool = "alt_world1";
    public string InAltWorld2Bool = "alt_world2";

    [Space(10)]

    public AnimTriggerEntry[] Triggers = new AnimTriggerEntry[0];

    [System.Serializable]
    public class AnimTriggerEntry
    {
        public Nights2Mgr.Nights2State StateTrigger = Nights2Mgr.Nights2State.GettingReady;
        [Space(10)]
        public string AnimTriggerName = "";
    }


    private Animator _animator = null;
    private bool _hasCandlesLitParam = false;
    private bool _hasEntrancePortalParam = false;
    private bool _hasExitPortalParam = false;
    private bool _hasAltWorld1Param = false;
    private bool _hasAltWorld2Param = false;

    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();

        //subscribe to state changed events
        if (Nights2Mgr.Instance != null)
            Nights2Mgr.Instance.OnStateChanged += OnNights2StateChanged;

        //subscript to portal state changed events
        if (Nights2TorchPlayer.Instance != null)
            Nights2TorchPlayer.Instance.OnPortalStateChanged += OnPortalStateChanged;

        //see if some params exist
        _hasCandlesLitParam = Nights2Utl.AnimatorHasParam(_animator, NumCandlesLitParam);
        _hasEntrancePortalParam = Nights2Utl.AnimatorHasParam(_animator, PassedEntrancePortalTrigger);
        _hasExitPortalParam = Nights2Utl.AnimatorHasParam(_animator, PassedExitPortalTrigger);

        _hasAltWorld1Param = Nights2Utl.AnimatorHasParam(_animator, InAltWorld1Bool);
        _hasAltWorld2Param = Nights2Utl.AnimatorHasParam(_animator, InAltWorld2Bool);
    }

    void Update()
    {
        if (_animator == null)
            return;

        //drive num portals passed param
        if (_hasCandlesLitParam && (NumCandlesLitParam.Length > 0))
        {
            _animator.SetInteger(NumCandlesLitParam, Nights2Mgr.Instance.NumCandlesLit());
        }

        if (_hasAltWorld1Param)
            _animator.SetBool(InAltWorld1Bool, Nights2Mgr.Instance.InAltWorld1());
        if (_hasAltWorld2Param)
            _animator.SetBool(InAltWorld2Bool, Nights2Mgr.Instance.InAltWorld2());
    }

    void OnDestroy()
    {
        //unsubscribe
        if (Nights2Mgr.Instance != null)
            Nights2Mgr.Instance.OnStateChanged -= OnNights2StateChanged;
    }

    void OnNights2StateChanged(object sender, Nights2Mgr.StateChangedEventArgs e)
    {
        if (_animator == null)
            return;

        for (int i = 0; i < Triggers.Length; i++)
        {
            if (Triggers[i].StateTrigger == e.NewState)
            {
                if(Triggers[i].AnimTriggerName.Length > 0)
                    _animator.SetTrigger(Triggers[i].AnimTriggerName);
            }
        }
    }

    void OnPortalStateChanged(object sender, Nights2TorchPlayer.PortalChangedEventArgs e)
    {
        if (_animator == null)
            return;

        //went through entrance portal
        if (_hasEntrancePortalParam && (e.NewState == Nights2TorchPlayer.PortalState.ThroughEntrancePortal))
        {
            _animator.SetTrigger(PassedEntrancePortalTrigger);
        }

        //went through exit portal
        if (_hasExitPortalParam && (e.NewState == Nights2TorchPlayer.PortalState.ThroughExitPortal))
        {
            _animator.SetTrigger(PassedExitPortalTrigger);
        }
    }
}
