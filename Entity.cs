using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NotDownwell.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotDownwell
{
    public abstract class Entity//座標などを保持する共通のクラス
    {
        public Vector2 position;//物体左上の座標 spritebatchのpositionが左上なのでそれに準拠する形で実装
        public Vector2 Center//中心
        {
            get => position + Size / 2f;
            set => position = value - Size / 2f;
        }
        public Vector2 oldPosition;//前フレームの位置
        public Vector2 velocity;//速度
        public Vector2 oldVelocity;//前フレームの速度
        public int width;
        public int heigth;
        public bool collideX;//左右の壁などに衝突した際
        public bool collideY;
        public HitBox hitBox;//衝突判定
        //描画
        public int frameCounter;//フレーム用カウンター
        public int frame;//現在のフレーム(0~)
        public int maxFrame = 1;//フレーム数
        public Vector2 Size
        {
            get => new Vector2(width, heigth);
            set
            {
                width = (int)value.X;
                heigth = (int)value.Y;
            }
        }
        public float rotation;
        public float scale = 1f;
        /// <summary>
        /// 1 => 右 -1 => 左 
        /// </summary>
        public int direction = 1;
        public Entity()
        {
        }
        public virtual void Update()
        {

        }
        /// <summary>
        /// エリアの左右との衝突確認、速度調整
        /// </summary>
        public virtual void CheckAreaCollide()
        {
            if (position.X <= GameScene.BorderL)
            {
                collideX = true;
                velocity.X = 0f;
                position.X = GameScene.BorderL;
            }
            if (position.X + width >= GameScene.BorderR)
            {
                collideX = true;
                velocity.X = 0f;
                position.X = GameScene.BorderR - width;
            }
        }
        public virtual bool CheckCollision(Entity target) => hitBox.Intersect(target.hitBox);
        public virtual void Kill()
        {

        }
        /// <summary>
        /// エンティティのフレーム周りのアップデートを行う
        /// </summary>
        public virtual void FrameUpdate()
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
