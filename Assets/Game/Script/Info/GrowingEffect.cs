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
public class GrowingEffect : UnityEngine.MonoBehaviour {
    
    private UnityEngine.Animator animator;
    
    public bool Grow {
        get {
            return false;
        }
        set {
            this.animator.SetTrigger(Hash.Grow);
        }
    }
    
    private void Awake() {
        this.animator = this.GetComponent <UnityEngine.Animator>();
    }
    
    private class Name {
        
        public const string Grow = "Grow";
    }
    
    private class Hash {
        
        public static int Grow = UnityEngine.Animator.StringToHash(Name.Grow);
    }
}
