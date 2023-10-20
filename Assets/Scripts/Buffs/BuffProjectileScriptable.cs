using UnityEngine;

[CreateAssetMenu(fileName = "BuffProjectile", menuName = "ScriptableObjects/BuffProjectile", order = 1)]
public class BuffProjectileScriptable : ScriptableObject
{
    public Sprite buffSprite;
    public Vector2 colliderSize;

    public float speed = 1;
    public float playerSpeed = 1;

    public bool onceDestroy = true;
}
