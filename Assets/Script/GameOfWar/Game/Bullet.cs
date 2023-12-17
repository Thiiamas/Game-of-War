namespace GameOfWar.Game
{
    using UnityEngine;
    using GameOfWar.Characters;
    public class Bullet : MonoBehaviour
    {
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public float Speed { get; set; }

        public Bullet(Vector3 position, Vector3 direction, float speed)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
        }

        void Update()
        {
            Move();
        }

        public void Move()
        {
            Position += Direction * Speed * Time.deltaTime;
        }

        public void Seek(Character target)
        {
            Direction = target.transform.position - Position;
            Direction.Normalize();
        }
    }
}