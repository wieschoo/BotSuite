namespace BotSuite.Imaging
{
	/// <summary>
	///     The convolution matrix.
	/// </summary>
	public class ConvolutionMatrix
	{
		/// <summary>
		///     The matrix size.
		/// </summary>
		public int MatrixSize = 3;

		/// <summary>
		///     The matrix.
		/// </summary>
		public double[,] Matrix;

		/// <summary>
		///     The factor.
		/// </summary>
		public double Factor = 1;

		/// <summary>
		///     The offset.
		/// </summary>
		public double Offset = 1;

		/// <summary>
		///     Initializes a new instance of the <see cref="ConvolutionMatrix" /> class.
		/// </summary>
		/// <param name="size">
		///     The size.
		/// </param>
		public ConvolutionMatrix(int size)
		{
			this.MatrixSize = 3;
			this.Matrix = new double[size, size];
		}

		/// <summary>
		///     The set all.
		/// </summary>
		/// <param name="value">
		///     The value.
		/// </param>
		public void SetAll(double value)
		{
			for (int i = 0; i < this.MatrixSize; i++)
			{
				for (int j = 0; j < this.MatrixSize; j++)
				{
					this.Matrix[i, j] = value;
				}
			}
		}
	}
}