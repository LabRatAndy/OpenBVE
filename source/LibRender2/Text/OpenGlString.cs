using System.Drawing;
using OpenBveApi.Colors;
using OpenBveApi.Graphics;
using OpenBveApi.Textures;
using OpenTK.Graphics.OpenGL;

namespace LibRender2.Texts
{
	public class OpenGlString
	{
		private readonly BaseRenderer renderer;

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
			if (renderer.currentOptions.IsUseNewRenderer == true)
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
				return;
			}
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
		public void DrawWithShader(OpenGlFont font, string text, Point location, TextAlignment alignment, Color128 color)
		{
			// position data calculated as per draw function 
			renderer.LastBoundTexture = null;
			int Left;
			if ((alignment & TextAlignment.Left) == 0)
			{
				int width = GetWidth(text, font);
				if ((alignment & TextAlignment.Right) != 0)
				{
					Left = location.X - width;
				}
				else
				{
					Left = location.X - width / 2;
				}
			}
			else
			{
				Left = location.X;
			}

			int Top;
			if ((alignment & TextAlignment.Top) == 0)
			{
				int height = GetHeight(text, font);
				if ((alignment & TextAlignment.Bottom) != 0)
				{
					Top = location.Y - height;
				}
				else
				{
					Top = location.Y - height / 2;
				}
			}
			else
			{
				Top = location.Y;
			}
			// set up VAO and VBO up to use with the text shader
			int VAO;
			int VBO;
			GL.GenVertexArrays(1, out VAO);
			GL.GenBuffers(1, out VBO);
			GL.BindVertexArray(VAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			//reserve enough VBO space 6 x 4 floats should be enough for a 2D quad
			GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * 4, System.IntPtr.Zero , BufferUsageHint.DynamicDraw);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			//render the text
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			renderer.TextShader.Activate();
			renderer.TextShader.SetTextColour(color);
			renderer.TextShader.SetTexture(0);
			GL.BindVertexArray(VAO);
			// iterate through the text
			for (int n = 0; n < text.Length; n++)
			{
				Texture texture;
				OpenGlFontChar data;
				n += font.GetCharacterData(text, n, out texture, out data) - n;
				if (renderer.currentHost.LoadTexture(texture, OpenGlTextureWrapMode.ClampClamp))
				{
					GL.BindTexture(TextureTarget.Texture2D, texture.OpenGlTextures[(int)OpenGlTextureWrapMode.ClampClamp].Name);
					int x = Left - (data.TypographicSize.Width - data.PhysicalSize.Width) / 2;
					int y = Top - (data.TypographicSize.Height - data.PhysicalSize.Height) / 2;
					float[] textVerticies = new float[6 * 4]
					{
						x,y+data.PhysicalSize.Height,data.TextureCoordinates.Left,data.TextureCoordinates.Top,
						x,y,data.TextureCoordinates.Left,data.TextureCoordinates.Bottom,
						x+data.PhysicalSize.Width,y,data.TextureCoordinates.Right,data.TextureCoordinates.Bottom,
						x,y+data.PhysicalSize.Height,data.TextureCoordinates.Left,data.TextureCoordinates.Top,
						x+data.PhysicalSize.Width,y,data.TextureCoordinates.Right,data.TextureCoordinates.Bottom,
						x+data.PhysicalSize.Width,y+data.PhysicalSize.Height,data.TextureCoordinates.Right,data.TextureCoordinates.Top
					};
					GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
					GL.BufferSubData(BufferTarget.ArrayBuffer, new System.IntPtr(0), new System.IntPtr(6 * 4 * sizeof(float)), textVerticies);
					GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
					Left += data.TypographicSize.Width;
				}
			}
			renderer.RestoreBlendFunc();
		}
		private int GetWidth(string text, OpenGlFont font)
		{
			int width = 0;
			for (int i = 0; i < text.Length; i++)
			{
				Texture texture;
				OpenGlFontChar data;
				i += font.GetCharacterData(text, i, out texture, out data) - i;
				width += data.TypographicSize.Width;
			}
			return width;
		}
		private int GetHeight(string text, OpenGlFont font)
		{
			int height = 0;
			for (int i = 0; i < text.Length; i++)
			{
				Texture texture;
				OpenGlFontChar data;
				i += font.GetCharacterData(text, i, out texture, out data) - i;
				if (data.TypographicSize.Height > height)
				{
					height = data.TypographicSize.Height;
				}
			}
			return height;
		}
	}
		
}
