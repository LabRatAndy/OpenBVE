using System.Drawing;
using System;
using OpenBveApi.Colors;
using OpenBveApi.Graphics;
using OpenBveApi.Textures;
using OpenTK.Graphics.OpenGL;

namespace LibRender2.Texts
{
	public class OpenGlString : IDisposable
	{
		private readonly BaseRenderer renderer;
		private int VAO = 0;
		private int VBO = 0;

		internal OpenGlString(BaseRenderer renderer)
		{
			this.renderer = renderer;
		}

		/// <summary>Renders a string to the screen.</summary>
		/// <param name="font">The font to use.</param>
		/// <param name="text">The string to render.</param>
		/// <param name="location">The location.</param>
		/// <param name="alignment">The alignment.</param>
		/// <param name="color">The color.</param>
		/// <remarks>This function sets the OpenGL blend function to glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA).</remarks>
		public void Draw(OpenGlFont font, string text, Point location, TextAlignment alignment, Color128 color)
		{
			if (text == null || font == null)
			{
				return;
			}
			renderer.LastBoundTexture = null;
			/*
			 * Prepare the top-left coordinates for rendering, incorporating the
			 * orientation of the string in relation to the specified location.
			 * */
			int left;

			if ((alignment & TextAlignment.Left) == 0)
			{
				int width = 0;

				for (int i = 0; i < text.Length; i++)
				{
					Texture texture;
					OpenGlFontChar data;
					i += font.GetCharacterData(text, i, out texture, out data) - 1;
					width += data.TypographicSize.Width;
				}

				if ((alignment & TextAlignment.Right) != 0)
				{
					left = location.X - width;
				}
				else
				{
					left = location.X - width / 2;
				}
			}
			else
			{
				left = location.X;
			}

			int top;

			if ((alignment & TextAlignment.Top) == 0)
			{
				int height = 0;

				for (int i = 0; i < text.Length; i++)
				{
					Texture texture;
					OpenGlFontChar data;
					i += font.GetCharacterData(text, i, out texture, out data) - 1;

					if (data.TypographicSize.Height > height)
					{
						height = data.TypographicSize.Height;
					}
				}

				if ((alignment & TextAlignment.Bottom) != 0)
				{
					top = location.Y - height;
				}
				else
				{
					top = location.Y - height / 2;
				}
			}
			else
			{
				top = location.Y;
			}

			/*
			 * Render the string.
			 * */
			GL.Enable(EnableCap.Texture2D);

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
			

			for (int i = 0; i < text.Length; i++)
			{
				Texture texture;
				OpenGlFontChar data;
				i += font.GetCharacterData(text, i, out texture, out data) - 1;

				if (renderer.currentHost.LoadTexture(texture, OpenGlTextureWrapMode.ClampClamp))
				{
					GL.BindTexture(TextureTarget.Texture2D, texture.OpenGlTextures[(int)OpenGlTextureWrapMode.ClampClamp].Name);

					int x = left - (data.PhysicalSize.Width - data.TypographicSize.Width) / 2;
					int y = top - (data.PhysicalSize.Height - data.TypographicSize.Height) / 2;

					/*
					 * In the first pass, mask off the background with pure black.
					 * */
					GL.BlendFunc(BlendingFactor.Zero, BlendingFactor.OneMinusSrcColor);
					GL.Begin(PrimitiveType.Quads);
					GL.Color4(color.A, color.A, color.A, 1.0f);
					GL.TexCoord2(data.TextureCoordinates.Left, data.TextureCoordinates.Top);
					GL.Vertex2(x, y);
					GL.TexCoord2(data.TextureCoordinates.Right, data.TextureCoordinates.Top);
					GL.Vertex2(x + data.PhysicalSize.Width, y);
					GL.TexCoord2(data.TextureCoordinates.Right, data.TextureCoordinates.Bottom);
					GL.Vertex2(x + data.PhysicalSize.Width, y + data.PhysicalSize.Height);
					GL.TexCoord2(data.TextureCoordinates.Left, data.TextureCoordinates.Bottom);
					GL.Vertex2(x, y + data.PhysicalSize.Height);
					GL.End();

					/*
					 * In the second pass, add the character onto the background.
					 * */
					GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);
					GL.Begin(PrimitiveType.Quads);
					GL.Color4(color.R, color.G, color.B, color.A);
					GL.TexCoord2(data.TextureCoordinates.Left, data.TextureCoordinates.Top);
					GL.Vertex2(x, y);
					GL.TexCoord2(data.TextureCoordinates.Right, data.TextureCoordinates.Top);
					GL.Vertex2(x + data.PhysicalSize.Width, y);
					GL.TexCoord2(data.TextureCoordinates.Right, data.TextureCoordinates.Bottom);
					GL.Vertex2(x + data.PhysicalSize.Width, y + data.PhysicalSize.Height);
					GL.TexCoord2(data.TextureCoordinates.Left, data.TextureCoordinates.Bottom);
					GL.Vertex2(x, y + data.PhysicalSize.Height);
					GL.End();
				}

				left += data.TypographicSize.Width;
			}

			renderer.RestoreBlendFunc();
			GL.Disable(EnableCap.Texture2D);

			GL.PopMatrix();

			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
		}

		/// <summary>Renders a string to the screen.</summary>
		/// <param name="font">The font to use.</param>
		/// <param name="text">The string to render.</param>
		/// <param name="location">The location.</param>
		/// <param name="alignment">The alignment.</param>
		/// <param name="color">The color.</param>
		/// <param name="shadow">Whether to draw a shadow.</param>
		/// <remarks>This function sets the OpenGL blend function to glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA).</remarks>
		public void Draw(OpenGlFont font, string text, Point location, TextAlignment alignment, Color128 color, bool shadow)
		{
			if (renderer.currentOptions.IsUseNewRenderer)
			{
				if (shadow)
				{
					DrawWithShader(font, text, new Point(location.X - 1, location.Y + 1), alignment, new Color128(0.0f, 0.0f, 0.0f, 0.5f * color.A));
					DrawWithShader(font, text, location, alignment, color);
				}
				else
				{
					DrawWithShader(font, text, location, alignment, color);
				}
			}
			else
			{
				if (shadow)
				{
					Draw(font, text, new Point(location.X - 1, location.Y + 1), alignment, new Color128(0.0f, 0.0f, 0.0f, 0.5f * color.A));
					Draw(font, text, location, alignment, color);
				}
				else
				{
					Draw(font, text, location, alignment, color);
				}
			}
		}
		private void DrawWithShader(OpenGlFont font, string text, Point location, TextAlignment alignment, Color128 colour)
		{
			if (font == null || text == null)
			{
				return;
			}
			renderer.LastBoundTexture = null;
			//get top and left coords for drawing as per draw method above
			int left;
			if ((alignment & TextAlignment.Left) == 0)
			{
				int width = 0;
				for (int i = 0; i < text.Length; i++)
				{
					Texture texture;
					OpenGlFontChar data;
					i += font.GetCharacterData(text, i, out texture, out data) - 1;
					width += data.TypographicSize.Width;
				}
				if ((alignment & TextAlignment.Right) != 0)
				{
					left = location.X - width;
				}
				else
				{
					left = location.X - width / 2;
				}
			}
			else
			{
				left = location.X;
			}
			int top;
			if ((alignment & TextAlignment.Top) == 0)
			{
				int height = 0;
				for (int i = 0; i < text.Length; i++)
				{
					Texture texture;
					OpenGlFontChar data;
					i += font.GetCharacterData(text, i, out texture, out data) - 1;
					if (height < data.TypographicSize.Height)
					{
						height = data.TypographicSize.Height;
					}
				}
				if ((alignment & TextAlignment.Bottom) != 0)
				{
					top = location.Y - height;
				}
				else
				{
					top = location.Y - height / 2;
				}
			}
			else
			{
				top = location.Y;
			}
			//render text using learningopengl.com method
			//create vao if needed
			float[] vertices;
			if (VAO == 0)
			{
				if (VBO != 0) GL.DeleteBuffers(1, ref VBO);
				GL.GenVertexArrays(1, out VAO);
				GL.BindVertexArray(VAO);
				GL.GenBuffers(1, out VBO);
				GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
				// setup the memory needed to draw the letters and make sure it is setup to be dynamic draw as will be changed  for every letter.
				GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * (6 * 4), (IntPtr)null, BufferUsageHint.DynamicDraw);
				GL.EnableVertexAttribArray(0);
				GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
				GL.BindVertexArray(0);
			}
			//activate the text shader and set up the blending functions and pass the colour , projection uniforms to the shader
			renderer.TextShader.Activate();
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			OpenTK.Matrix4 orthographicprojection;
			OpenTK.Matrix4.CreateOrthographic(renderer.Screen.Width, renderer.Screen.Height, 0.001f, 600.0f, out orthographicprojection);
			renderer.TextShader.SetTextProjectionMatrix(orthographicprojection);
			renderer.TextShader.SetTextColour(colour);

			//iterate the string and renderer the text
			GL.BindVertexArray(VAO);
			for (int n = 0; n < text.Length; n++)
			{
				Texture texture;
				OpenGlFontChar data;
				n += font.GetCharacterData(text, n, out texture, out data) - 1;
				if (renderer.currentHost.LoadTexture(texture, OpenGlTextureWrapMode.ClampClamp))
				{
					GL.BindTexture(TextureTarget.Texture2D, texture.OpenGlTextures[(int)OpenGlTextureWrapMode.ClampClamp].Name);
					renderer.TextShader.SetFontTexture(texture.OpenGlTextures[(int)OpenGlTextureWrapMode.ClampClamp].Name);
					float x = (float)left - (data.PhysicalSize.Width - data.TypographicSize.Width) / 2;
					float y = (float)top - (data.PhysicalSize.Height - data.TypographicSize.Height) / 2;
					vertices = new float[]
					{
						//triangle 1
						x,y,data.TextureCoordinates.Left,data.TextureCoordinates.Top, // vertex 1
						x+data.PhysicalSize.Width,y,data.TextureCoordinates.Right,data.TextureCoordinates.Top,	//vertex 2
						x+data.PhysicalSize.Width,y+data.PhysicalSize.Height,data.TextureCoordinates.Right,data.TextureCoordinates.Bottom, // vertex 3
						//triangle 2
						x+data.PhysicalSize.Width,y,data.TextureCoordinates.Right,data.TextureCoordinates.Top, //vertex 2
						x+data.PhysicalSize.Width,y+data.PhysicalSize.Height,data.TextureCoordinates.Right,data.TextureCoordinates.Bottom, //vertex 3
						x,y+data.PhysicalSize.Height,data.TextureCoordinates.Left,data.TextureCoordinates.Bottom
					};
					//send the vertex data to the vbo
					GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, sizeof(float) * 24, vertices);
					GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
				}
				left += data.TypographicSize.Width;
			}
			GL.BindVertexArray(0);
			GL.Disable(EnableCap.Texture2D);
			renderer.TextShader.Deactivate();
			renderer.RestoreBlendFunc();
		}
		public void Dispose()
		{
			if (VBO != 0) GL.DeleteBuffer(VBO);
			if (VAO != 0) GL.DeleteVertexArray(VAO);
			GC.SuppressFinalize(this);
		}
		~OpenGlString()
		{
			if (VBO != 0) GL.DeleteBuffer(VBO);
			if (VAO != 0) GL.DeleteVertexArray(VAO);
		}

	}
}
