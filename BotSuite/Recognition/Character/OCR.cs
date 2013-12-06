// -----------------------------------------------------------------------
//  <copyright file="OCR.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.Recognition.Character
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;

	using BotSuite.Imaging;

	using global::BotSuite.MachineLearning.NeuralNetwork;

	/// <summary>
	///     class to recognize characters by a neural network
	/// </summary>
	[Serializable]
	public class OCR
	{
		/// <summary>
		///     instance of neural network
		/// </summary>
		protected NeuralNetwork WorkingNeuralNetwork;

		/// <summary>
		///     receptors to detect character
		/// </summary>
		protected MagicMatchSticks ImageSense;

		/// <summary>
		///     flag of network initialisation
		/// </summary>
		protected bool NetworkWasInitialised = false;

		/// <summary>
		///     possible characters that can be recognized
		/// </summary>
		protected List<char> CharactersToRecognize;

		/// <summary>
		///     Initializes a new instance of the <see cref="OCR" /> class.
		///     get a simple class to do OCR
		/// </summary>
		/// <param name="numberOfSense">
		///     number of receptors
		/// </param>
		/// <returns>
		///     instance of class
		/// </returns>
		public OCR(int numberOfSense = 17)
		{
			this.ImageSense = new MagicMatchSticks();
			this.ImageSense.Generate(numberOfSense);
		}

		/// <summary>
		///     start a new trainingsession to learn the new imagedatas
		/// </summary>
		/// <param name="trainingImageData">
		///     data (images) to learn
		/// </param>
		public void StartTrainingSession(Dictionary<char, List<ImageData>> trainingImageData)
		{
			if (!this.NetworkWasInitialised)
			{
				this.CharactersToRecognize = new List<char>(trainingImageData.Keys);
				this.WorkingNeuralNetwork = new NeuralNetwork(
					this.ImageSense.Num(),
					new[] { this.ImageSense.Num(), 10, 10, this.CharactersToRecognize.Count });
				this.WorkingNeuralNetwork.ShuffleBoth();
				this.NetworkWasInitialised = true;
			}

			const float BadCharacter = -0.5f; // the expected output for a not matching character
			const float GoodCharacter = 0.5f; // the expected output for a matching character

			// precalculate the expected output
			float[] expectedOutput = new float[this.CharactersToRecognize.Count];
			for (int j = 0; j < this.CharactersToRecognize.Count; j++)
			{
				expectedOutput[j] = BadCharacter;
			}

			// remember last character to calculate the expected output faster
			int lastCharacter = 0;

			int totalLearningData = this.CharactersToRecognize.Sum(t => trainingImageData[t].Count);

			// raw learning - data
			float[][] x = new float[totalLearningData][];
			float[][] y = new float[totalLearningData][];

			int learningdataPointer = 0;
			int numberOfInputs = this.ImageSense.Num();
			int numberOfOutputs = this.CharactersToRecognize.Count;

			for (int currentCharacter = 0; currentCharacter < this.CharactersToRecognize.Count; currentCharacter++)
			{
				expectedOutput[lastCharacter] = BadCharacter;
				expectedOutput[currentCharacter] = GoodCharacter;
				lastCharacter = currentCharacter;
				if (trainingImageData[this.CharactersToRecognize[currentCharacter]].Count != 0)
				{
					for (int imgs = 0; imgs < trainingImageData[this.CharactersToRecognize[currentCharacter]].Count; imgs++)
					{
						float[] currentState =
							this.ImageSense.GetMagicMatchSticksState(trainingImageData[this.CharactersToRecognize[currentCharacter]][imgs]);
						x[learningdataPointer] = new float[numberOfInputs];
						y[learningdataPointer] = new float[numberOfOutputs];
						for (int a = 0; a < numberOfInputs; a++)
						{
							x[learningdataPointer][a] = currentState[a];
						}
						for (int a = 0; a < numberOfOutputs; a++)
						{
							y[learningdataPointer][a] = expectedOutput[a];
						}
						learningdataPointer++;
					}
				}
			}

			this.WorkingNeuralNetwork.Teacher.MaximumOfEpochs = 1000;
			this.WorkingNeuralNetwork.Learn(x, y);
		}

		/// <summary>
		///     calculate the prediction error of a textsuite
		/// </summary>
		/// <param name="testSuite">
		///     Training data as list of pictures(imagedata) and key (character)
		/// </param>
		/// <returns>
		///     The <see cref="float" />.
		/// </returns>
		public float PredictionError(Dictionary<char, List<ImageData>> testSuite)
		{
			int datasetSize = 0;
			int occuredErrors = 0;

			foreach (char currentCharacter in this.CharactersToRecognize)
			{
				if (testSuite.ContainsKey(currentCharacter))
				{
					if (testSuite[currentCharacter].Count != 0)
					{
						for (int imgs = 0; imgs < testSuite[currentCharacter].Count; imgs++)
						{
							datasetSize++;
							if (currentCharacter != this.Recognize(testSuite[currentCharacter][imgs]))
							{
								occuredErrors++;
							}
						}
					}
				}
			}

			return (float)occuredErrors / datasetSize;
		}

		/// <summary>
		///     get the pattern of the magic sticks, which the network learns
		/// </summary>
		/// <param name="img">
		///     image of character
		/// </param>
		/// <returns>
		///     pattern of magic sticks as array of floats (0.0f, 1.0f)
		/// </returns>
		public float[] GetMagicSticksPattern(ImageData img)
		{
			return this.ImageSense.GetMagicMatchSticksState(img);
		}

		/// <summary>
		///     get the output of the neural network
		/// </summary>
		/// <param name="input">
		/// </param>
		/// <returns>
		///     network output as array of float
		/// </returns>
		public float[] GetNetworkOutput(float[] input)
		{
			return this.WorkingNeuralNetwork.Output(input);
		}

		/// <summary>
		///     recognize a character in image
		/// </summary>
		/// <param name="img">
		///     image of character
		/// </param>
		/// <returns>
		///     detected character (this is possible wrong, if the training wasn't successfull)
		/// </returns>
		public char Recognize(ImageData img)
		{
			float[] output = this.WorkingNeuralNetwork.Output(this.ImageSense.GetMagicMatchSticksState(img));
			int indexAtMax = output.ToList().IndexOf(output.Max());
			return this.CharactersToRecognize[indexAtMax];
		}

		/// <summary>
		///     save the OCR data in a binary formated file
		/// </summary>
		/// <param name="file">
		///     the target file path
		/// </param>
		public void Store(string file)
		{
			IFormatter binFmt = new BinaryFormatter();
			Stream s = File.Open(file, FileMode.Create);
			binFmt.Serialize(s, this);
			s.Close();
		}

		/// <summary>
		///     load a OCR data file from a binary formated file
		/// </summary>
		/// <param name="file">
		///     the neural network file file
		/// </param>
		/// <returns>
		///     The <see cref="OCR" />.
		/// </returns>
		public static OCR Load(string file)
		{
			OCR result;
			try
			{
				IFormatter binFmt = new BinaryFormatter();
				Stream s = File.Open(file, FileMode.Open);
				result = (OCR)binFmt.Deserialize(s);
				s.Close();
			}
			catch (Exception e)
			{
				throw new Exception("OCR : Unable to load file " + file + " : " + e);
			}

			return result;
		}
	}
}