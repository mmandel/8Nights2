//
// Activate animator triggers in response to changes to the Nights2Mgr's state
//


using UnityEngine;
using System.Collections;

public class Nights2AnimTrigger : MonoBehaviour 
{
    public AnimTriggerEntry[] Triggers = new AnimTriggerEntry[0];

    [System.Serializable]
    public class AnimTriggerEntry
    {
        public Nights2Mgr.Nights2State StateTrigger = Nights2Mgr.Nights2State.GettingReady;
        [Space(10)]
        public string AnimTriggerName = "";
    }


    private Animator _animator = null;

    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();

        //subscribe to state changed events
        if (Nights2Mgr.Instance != null)
            Nights2Mgr.Instance.OnStateChanged += OnNights2StateChanged;
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

}
