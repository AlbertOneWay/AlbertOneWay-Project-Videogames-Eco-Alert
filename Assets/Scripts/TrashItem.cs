using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public TrashDataSO trashInfo;

    public void Collect()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddTrash(trashInfo);  
        }
        Destroy(gameObject);
    }
}