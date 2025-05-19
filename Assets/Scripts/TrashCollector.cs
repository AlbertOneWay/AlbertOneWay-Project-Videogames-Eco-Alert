using UnityEngine;
using VolumetricLines;

public class TrashCollector : MonoBehaviour
{
    [Header("Layer de la basura")]
    public LayerMask trashLayer;

    [Header("Distancia de rayo")]
    public float maxDistance = 10f;

    [Header("Velocidad de atracción")]
    public float pullSpeed = 10f;

    [Header("Prefab del VFX de rayo")]
    public GameObject beamPrefab;

    [Header("Origen del rayo (mano o pecho del jugador)")]
    public Transform beamOrigin;

    private TrashItem heldTrash;
    private Rigidbody heldRb;
    private GameObject currentBeam;
    private VolumetricLineBehavior beamLine;

    private void OnEnable()
    {
        TrashItem.OnTrashDestroyed += HandleTrashDestroyed;
    }

    private void OnDisable()
    {
        TrashItem.OnTrashDestroyed -= HandleTrashDestroyed;
    }

    private void HandleTrashDestroyed(TrashItem destroyed)
    {
        if (heldTrash == destroyed)
        {
            CleanupHeldTrash(); // ¡limpia bien si desaparece fuera del script!
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (heldTrash == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, trashLayer))
                {
                    TrashItem trash = hit.collider.GetComponent<TrashItem>();
                    if (trash != null)
                    {
                        heldTrash = trash;
                        heldRb = trash.GetComponent<Rigidbody>();

                        if (heldRb != null)
                        {
                            heldRb.useGravity = false;
                            heldRb.isKinematic = true;
                        }

                        currentBeam = Instantiate(beamPrefab);
                        beamLine = currentBeam.GetComponent<VolumetricLineBehavior>();
                    }
                }
            }
            else
            {
                Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * 2f;
                if (heldTrash != null)
                {
                    heldTrash.transform.position = Vector3.Lerp(heldTrash.transform.position, targetPos, Time.deltaTime * pullSpeed);
                }

                if (beamLine != null && beamOrigin != null)
                {
                    beamLine.StartPos = beamOrigin.position;
                    beamLine.EndPos = heldTrash.transform.position;
                }
            }
        }
        else if (heldTrash != null)
        {
            ReleaseTrash();
        }
    }

    private void ReleaseTrash()
    {
        if (heldRb != null)
        {
            heldRb.useGravity = true;
            heldRb.isKinematic = false;
        }

        CleanupHeldTrash();
    }

    private void CleanupHeldTrash()
    {
        heldTrash = null;
        heldRb = null;

        if (currentBeam != null)
        {
            Destroy(currentBeam);
            currentBeam = null;
        }

        beamLine = null;
    }
}
