using UnityEngine;

[System.Serializable]
class PlayerData
{
    public PlayerData(int id, Vector3 pos, Vector3 scale)
    {
        Id = id;
        Pos = pos;
        Scale = scale;
    }
    public int Id { get; }
    public Vector3 Pos { get; }
    public Vector3 Scale { get; }
}
