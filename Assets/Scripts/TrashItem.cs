using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public TrashDataSO trashInfo;

    public void Collect()
    {
        
        GameManager.AddTrash(trashInfo);  
        
        Destroy(gameObject);
    }
}