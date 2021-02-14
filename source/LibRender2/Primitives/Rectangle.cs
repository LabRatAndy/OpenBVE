using System.Drawing;
using OpenBveApi.Colors;
using OpenBveApi.Textures;
using OpenTK.Graphics.OpenGL;
using System;

namespace LibRender2.Primitives
{
	public class Rectangle
	{
		private readonly BaseRenderer renderer;
		private int VAO = -1;
		private int VBO = -1;

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
		private void DrawImmediate(Texture texture, PointF point, SizeF size, Color128? color = null)
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
		public void Draw(Texture texture, PointF point, SizeF size, Color128? color = null)
		{
			if (renderer.currentOptions.IsUseNewRenderer)
			{
				DrawWithShader(texture, point, size, color);
			}
			else
			{
				DrawImmediate(texture, point, size, color);
			}
		}
		private void DrawWithShader(Texture texture, PointF point, SizeF size, Color128? colour = null)
		{
			renderer.RectangleShader.Activate();
			renderer.RectangleShader.SetRectangleViewMatrix(renderer.CurrentViewMatrix);
			renderer.RectangleShader.SetRectangleProjectionMatrix(renderer.CurrentProjectionMatrix);
			float[] vertexdata;
			//set up vao and VBO if needed
			if (VAO == -1)
			{
				if (VBO != -1) GL.DeleteBuffer(VBO);
				GL.GenVertexArrays(1, out VAO);
				GL.BindVertexArray(VAO);
				GL.GenBuffers(1, out VBO);
				GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
				GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 24, (IntPtr)null, BufferUsageHint.DynamicDraw);
				GL.EnableVertexAttribArray(0);
				GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, sizeof(float) * 24, 0);
				GL.BindVertexArray(0);
			}
			// setup the vertexdata
			vertexdata = new float[]
			{
				// triangle 1
				point.X,point.Y,0.0f,0.0f,
				point.X+size.Width,point.Y,1.0f,0.0f,
				point.X+size.Width,point.Y+size.Height,1.0f,1.0f,
				//triangle 2
				point.X+size.Width,point.Y+size.Height,1.0f,1.0f,
				point.X,point.Y+size.Height,0.0f,1.0f,
				point.X,point.Y,0.0f,0.0f
			};
			GL.BindVertexArray(VAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, sizeof(float) * 24, vertexdata);
			// pass colours, texture, projection matrix etc to shader
			if (texture == null || !renderer.currentHost.LoadTexture(texture, OpenGlTextureWrapMode.ClampClamp))
			{
				GL.Disable(EnableCap.Texture2D);
				renderer.RectangleShader.SetRectangleHasTexture(false);
				if (colour.HasValue)
				{
					renderer.RectangleShader.SetRectangleHasColour(true);
					renderer.RectangleShader.SetRectangleColour((Color128)colour);
				}
				else
				{
					renderer.RectangleShader.SetRectangleHasColour(false);
				}
			}
			else
			{
				GL.Enable(EnableCap.Texture2D);
				GL.ActiveTexture(TextureUnit.Texture0);
				GL.BindTexture(TextureTarget.Texture2D, texture.OpenGlTextures[(int)OpenGlTextureWrapMode.ClampClamp].Name);
				renderer.RectangleShader.SetRectangleHasTexture(true);
				renderer.RectangleShader.SetRectangleTexture(0);
				if (colour.HasValue)
				{
					renderer.RectangleShader.SetRectangleHasColour(true);
					renderer.RectangleShader.SetRectangleColour((Color128)colour);
				}
				else
				{
					renderer.RectangleShader.SetRectangleHasColour(false);
				}
			}
			//draw the rectangle
			GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
			GL.Disable(EnableCap.Texture2D);
			GL.BindVertexArray(0);
			renderer.RectangleShader.Deactivate();
		}
	}
}
