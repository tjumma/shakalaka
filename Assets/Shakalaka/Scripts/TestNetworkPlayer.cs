using Unity.Netcode;
using UnityEngine;

namespace Shakalaka
{
    public class TestNetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed;

        private Vector3 _serverMoveInput;

        private void Update()
        {
            var moveInput = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) moveInput.z = +1f;
            if (Input.GetKey(KeyCode.D)) moveInput.x = +1f;
            if (Input.GetKey(KeyCode.S)) moveInput.z = -1f;
            if (Input.GetKey(KeyCode.A)) moveInput.x = -1f;

            if (IsServer)
            {
                if (IsLocalPlayer)
                    _serverMoveInput = moveInput;
                Move(_serverMoveInput);
            }
            else if(IsClient && IsLocalPlayer)
                MoveServerRpc(moveInput);
        }

        private void Move(Vector3 moveInput)
        {
            transform.position += moveInput * (moveSpeed * Time.deltaTime);
        }

        [ServerRpc]
        private void MoveServerRpc(Vector3 moveInput)
        {
            _serverMoveInput = moveInput;
        }

        private void Start()
        {
            Debug.Log("PlayerNetwork Start");
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log("PlayerNetwork OnNetworkSpawn");
            NetworkManager.Singleton.NetworkTickSystem.Tick += OnTick;
        }
        
        public override void OnNetworkDespawn()
        {
            NetworkManager.Singleton.NetworkTickSystem.Tick -= OnTick;
        }

        private void OnTick()
        {
            // Debug.Log("Tick");
        }
    }
}