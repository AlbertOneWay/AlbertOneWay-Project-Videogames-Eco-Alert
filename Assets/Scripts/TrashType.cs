
using UnityEngine;
public enum TrashType
{
    Organico,
    Plastico,
    Papel,
    Vidrio,
    Metal,
    Otro
}

public enum TrashCategory
{
    Aprovechables,     // Blanco
    Organicos,         // Verde
    NoAprovechables    // Negro
}


[CreateAssetMenu(menuName = "Basura/Nueva basura")]
public class TrashDataSO : ScriptableObject
{
    public string trashName;
    public Sprite trashIcon;
    public TrashCategory category;
}