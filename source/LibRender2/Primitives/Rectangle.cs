﻿using System.Drawing;
using OpenBveApi.Colors;
using OpenBveApi.Textures;
using OpenTK.Graphics.OpenGL;

namespace LibRender2.Primitives
{
	public class Rectangle
	{
		private readonly BaseRenderer renderer;

		internal Rectangle(BaseRenderer renderer)
		{
			this.renderer = renderer;
		}

		/// <summary>Renders an overlay texture</summary>
		/// <param name="texture">The texture</param>
		/// <param name="left">The left co-ordinate</param>
		/// <param name="top">The top co-ordinate</param>
		/// <param name="right">The right co-ordinate</param>
		/// <param name="bottom">The bottom co-ordinate</param>
		public void RenderOverlayTexture(Texture texture, double left, double top, double right, double bottom)
		{
			Draw(texture, new PointF((float)left, (float)top), new SizeF((float)(right - left), (float)(bottom - top)));
		}

		/// <summary>Renders a solid color rectangular overlay</summary>
		/// <param name="left">The left co-ordinate</param>
		/// <param name="top">The top co-ordinate</param>
		/// <param name="right">The right co-ordinate</param>
		/// <param name="bottom">The bottom co-ordinate</param>
		public void RenderOverlaySolid(double left, double top, double right, double bottom)
		{
			Draw(null, new PointF((float)left, (float)top), new SizeF((float)(right - left), (float)(bottom - top)));
		}

		/// <summary>Draws a simple 2D rectangle.</summary>
		/// <param name="texture">The texture, or a null reference.</param>
		/// <param name="point">The top-left coordinates in pixels.</param>
		/// <param name="size">The size in pixels.</param>
		/// <param name="color">The color, or a null reference.</param>
		public void Draw(Texture texture, PointF point, SizeF size, Color128? color = null)
		{
			renderer.LastBoundTexture = null;
			// TODO: Remove Nullable<T> from color once RenderOverlayTexture and RenderOverlaySolid are fully replaced.
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			unsafe
			{
				fixed (double* matrixPointer = &renderer.CurrentProjectionMatrix.Row0.X)
				{
					GL.LoadMatrix(matrixPointer);
				}
				GL.MatrixMode(MatrixMode.Modelview);
				GL.PushMatrix();
				fixed (double* matrixPointer = &renderer.CurrentViewMatrix.Row0.X)
				{
					GL.LoadMatrix(matrixPointer);
				}

			}
			if (texture == null || !renderer.currentHost.LoadTexture(texture, OpenGlTextureWrapMode.ClampClamp))
			{
				GL.Disable(EnableCap.Texture2D);

				if (color.HasValue)
				{
					GL.Color4(color.Value.R, color.Value.G, color.Value.B, color.Value.A);
				}

				GL.Begin(PrimitiveType.Quads);
				GL.Vertex2(point.X, point.Y);
				GL.Vertex2(point.X + size.Width, point.Y);
				GL.Vertex2(point.X + size.Width, point.Y + size.Height);
				GL.Vertex2(point.X, point.Y + size.Height);
				GL.End();
			}
			else
			{
				GL.Enable(EnableCap.Texture2D);
				GL.BindTexture(TextureTarget.Texture2D, texture.OpenGlTextures[(int)OpenGlTextureWrapMode.ClampClamp].Name);

				if (color.HasValue)
				{
					GL.Color4(color.Value.R, color.Value.G, color.Value.B, color.Value.A);
				}

				GL.Begin(PrimitiveType.Quads);
				GL.TexCoord2(0.0f, 0.0f);
				GL.Vertex2(point.X, point.Y);
				GL.TexCoord2(1.0f, 0.0f);
				GL.Vertex2(point.X + size.Width, point.Y);
				GL.TexCoord2(1.0f, 1.0f);
				GL.Vertex2(point.X + size.Width, point.Y + size.Height);
				GL.TexCoord2(0.0f, 1.0f);
				GL.Vertex2(point.X, point.Y + size.Height);
				GL.End();
				GL.Disable(EnableCap.Texture2D);
			}

			GL.PopMatrix();

			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
		}
		public void DrawWithShader(Texture texture, PointF point, SizeF size, Color128? colour = null)
		{
			renderer.LastBoundTexture = null;
			//todo sort out projection matrix 
			//todo add the shader using and fbo binding commands
			//create the vertex data
			float[] vertices = new float[16]
			{
				point.X, point.Y, 0.0f, 0.0f,
				point.X + size.Width, point.Y, 1.0f, 0.0f,
				point.X + size.Width, point.Y + size.Height, 1.0f, 1.0f,
				point.X, point.Y + size.Height, 0.0f, 1.0f
			};
			// create the vbo and vao
			int vbo, vao;
			GL.GenVertexArrays(1, out vao);
			GL.GenBuffers(1, out vbo);
			GL.BindVertexArray(vao);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, 16 * sizeof(float), vertices, BufferUsageHint.StaticDraw);
			//set the attributes
			//coordinates 
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);
			//texcoords
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 2 * sizeof(float));
			GL.EnableVertexAttribArray(1);
			if (texture == null || !renderer.currentHost.LoadTexture(texture, OpenGlTextureWrapMode.ClampClamp))
			{
				if (colour.HasValue)
				{
					//todo sort out passing the colour to the shader ? via uniform or via vbo
				}
				//todo pass the uniforms
				//draw the vbo
				GL.DrawArrays(PrimitiveType.Quads, 0, );
			}
			else
			{
				GL.BindTexture(TextureTarget.Texture2D, texture.OpenGlTextures[(int)OpenGlTextureWrapMode.ClampClamp].Name);
				if (colour.HasValue)
				{
					//todo sort out passing the colour to the shader ? via uniform or via vbo
				}
				GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			}
			GL.BindVertexArray(0);
		}
	}
}
