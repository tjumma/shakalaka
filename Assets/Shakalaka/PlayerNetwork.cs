using Unity.Netcode;
using UnityEngine;

namespace Shakalaka
{
    public class PlayerNetwork : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed;
        
        private void Update()
        {
            if (!IsOwner) return;
            
            var moveDir = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
            if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
            if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
            if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;

            transform.position += moveDir * (moveSpeed * Time.deltaTime);
        }
    }
}