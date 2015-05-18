// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------



[UnityEngine.RequireComponent(typeof(UnityEngine.Animator))]
public class BarrierAnimation : UnityEngine.MonoBehaviour {
    
    private UnityEngine.Animator animator;
    
    public bool Disappearing {
        get {
            return this.animator.GetBool(Hash.Disappearing);
        }
        set {
            this.animator.SetBool(Hash.Disappearing, value);
        }
    }
    
    public bool Frozen {
        get {
            return this.animator.GetBool(Hash.Frozen);
        }
        set {
            this.animator.SetBool(Hash.Frozen, value);
        }
    }
    
    public bool CoolingDown {
        get {
            return this.animator.GetBool(Hash.CoolingDown);
        }
        set {
            this.animator.SetBool(Hash.CoolingDown, value);
        }
    }
    
    public bool Hitting {
        get {
            return false;
        }
        set {
            this.animator.SetTrigger(Hash.Hitting);
        }
    }
    
    private void Awake() {
        this.animator = this.GetComponent <UnityEngine.Animator>();
    }
    
    private class Name {
        
        public const string Disappearing = "Disappearing";
        
        public const string Frozen = "Frozen";
        
        public const string CoolingDown = "CoolingDown";
        
        public const string Hitting = "Hitting";
    }
    
    private class Hash {
        
        public static int Disappearing = UnityEngine.Animator.StringToHash(Name.Disappearing);
        
        public static int Frozen = UnityEngine.Animator.StringToHash(Name.Frozen);
        
        public static int CoolingDown = UnityEngine.Animator.StringToHash(Name.CoolingDown);
        
        public static int Hitting = UnityEngine.Animator.StringToHash(Name.Hitting);
    }
}
