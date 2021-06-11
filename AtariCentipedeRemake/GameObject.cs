using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtariCentipedeRemake
{
    class GameObject
    {
        private Texture2D texture;
        public Vector2 position;
        public int speed;
        public bool active;
        public float width;
        public float height;
        public bool horizontalDirection;

        public GameObject()
        {
            position = Vector2.Zero;
            speed = 32;
            active = true;
            width = 0.0F;
            height = 0.0F;
            horizontalDirection = true;
        }

        public GameObject(GameObject org)
        {
            this.texture = org.texture;
            this.position = org.position;
            this.speed = org.speed;
            this.active = org.active;
            this.width = org.width;
            this.height = org.height;
        }

        public void AddTexture(Texture2D texture)
        {
            this.texture = texture;
            width = texture.Width;
            height = texture.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }
    }
}
