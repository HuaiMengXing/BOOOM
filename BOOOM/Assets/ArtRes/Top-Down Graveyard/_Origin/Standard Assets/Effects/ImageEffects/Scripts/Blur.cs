﻿/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Blur/Blur")]
    public class Blur : MonoBehaviour
    {
        /// Blur iterations - larger number means more blur.
        public int iterations = 3;

        /// Blur spread for each iteration. Lower values
        /// give better looking blur, but require more iterations to
        /// get large blurs. Value is usually between 0.5 and 1.0.
        public float blurSpread = 0.6f;


        // --------------------------------------------------------
        // The blur iteration shader.
        // Basically it just takes 4 texture samples and averages them.
        // By applying it repeatedly and spreading out sample locations
        // we get a Gaussian blur approximation.

        public Shader blurShader = null;

        static Material m_Material = null;
        protected Material material {
            get {
                if (m_Material == null) {
                    m_Material = new Material(blurShader);
                    m_Material.hideFlags = HideFlags.DontSave;
                }
                return m_Material;
            }
        }

        protected void OnDisable() {
            if ( m_Material ) {
                DestroyImmediate( m_Material );
            }
        }

        // --------------------------------------------------------

        [Obsolete]
        protected void Start()
        {
            // Disable if we don't support image effects
            if (!SystemInfo.supportsImageEffects) {
                enabled = false;
                return;
            }
            // Disable if the shader can't run on the users graphics card
            if (!blurShader || !material.shader.isSupported) {
                enabled = false;
                return;
            }
        }

        // Performs one blur iteration.
        public void FourTapCone (RenderTexture source, RenderTexture dest, int iteration)
        {
            float off = 0.5f + iteration*blurSpread;
            Graphics.BlitMultiTap (source, dest, material,
                                   new Vector2(-off, -off),
                                   new Vector2(-off,  off),
                                   new Vector2( off,  off),
                                   new Vector2( off, -off)
                );
        }

        // Downsamples the texture to a quarter resolution.
        private void DownSample4x (RenderTexture source, RenderTexture dest)
        {
            float off = 1.0f;
            Graphics.BlitMultiTap (source, dest, material,
                                   new Vector2(-off, -off),
                                   new Vector2(-off,  off),
                                   new Vector2( off,  off),
                                   new Vector2( off, -off)
                );
        }

        // Called by the camera to apply the image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination) {
            int rtW = source.width/4;
            int rtH = source.height/4;
            RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);

            // Copy source to the 4x4 smaller texture.
            DownSample4x (source, buffer);

            // Blur the small texture
            for(int i = 0; i < iterations; i++)
            {
                RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
                FourTapCone (buffer, buffer2, i);
                RenderTexture.ReleaseTemporary(buffer);
                buffer = buffer2;
            }
            Graphics.Blit(buffer, destination);

            RenderTexture.ReleaseTemporary(buffer);
        }
    }
}
