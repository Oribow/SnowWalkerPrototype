using UnityEngine;
/*
Author: Oribow
*/
namespace Combat
{
    public class BasicDeathHandler : MonoBehaviour
    {
        public IHealth healthComponent;
        public bool destroyObject = true;
        public float destroyAfterTime;
        public Animator animator;
        public GameObject root;
        public UnityEngine.Events.UnityEvent onDeath;

        void Start()
        {
            healthComponent.OnDeath += HealthComponent_OnDeath;
        }

        private void HealthComponent_OnDeath(object sender, System.EventArgs e)
        {
            if (animator != null)
                animator.SetTrigger("Die");
            onDeath.Invoke();
            if (destroyObject)
                Destroy(root, destroyAfterTime);
        }
    }
}
