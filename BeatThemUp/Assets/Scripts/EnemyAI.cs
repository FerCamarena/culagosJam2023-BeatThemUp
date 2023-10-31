using UnityEngine;
public class EnemyAI : MonoBehaviour {
    //Method called before first frame
    private void Start() {
    }
    //Method called each frame
    private void Update() {
    }
    //Method called each fixed time
    private void FixedUpdate() {
    }
    //Method called for harming
    public void Damage(float damageAmount) {
        Debug.Log(damageAmount);
    }
}
