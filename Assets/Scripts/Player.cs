using Photon.Pun;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable {
    public PhotonView photonView;
    public TextMeshPro nicknameText;
    public MeshRenderer capsule;
    public MeshRenderer cylinder;
    public Material mine;
    public Material other;
    public CharacterController characterController;

    public Vector3 currentPosition;
    public Quaternion currentRotation;
    public float speed = 5;

    private float vertical = 0;
    private float horizontal = 0;

    public void Awake() {
        nicknameText.text = photonView.IsMine ? PhotonNetwork.NickName : photonView.Owner.NickName;
        nicknameText.color = photonView.IsMine ? Color.green : Color.red;
        capsule.material = photonView.IsMine ? mine : other;
        cylinder.material = photonView.IsMine ? mine : other;
        characterController = GetComponent<CharacterController>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else {
            currentPosition = (Vector3) stream.ReceiveNext();
            currentRotation = (Quaternion) stream.ReceiveNext();
        }
    }

    public void Update() {
        if (photonView.IsMine) {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

            var movingDirection = (Vector3.forward * vertical) + (Vector3.right * horizontal);
            characterController.Move(movingDirection * Time.deltaTime * speed);

            if (vertical == 0 || horizontal == 0) {
                return;
            }
            
            transform.forward = movingDirection;    
        }
        else {
            transform.position = Vector3.Lerp(transform.position, currentPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, Time.deltaTime * 10);
        }
    }
}
