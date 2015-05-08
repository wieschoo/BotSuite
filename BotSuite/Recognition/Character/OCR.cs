using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BotSuite.ImageLibrary;
using BotSuite.MachineLearning.NeuralNetwork;


namespace BotSuite.Recognition.Character
{
    
    
    /// <summary>
    /// class to recognize characters by a neural network
    /// </summary>
    [Serializable]
    public class OCR
    {
        #region protected properties
        /// <summary>
        /// instance of neural network
        /// </summary>
        protected NeuralNetwork WorkingNeuralNetwork;
        /// <summary>
        /// receptors to detect character
        /// </summary>
        protected MagicMatchSticks ImageSense;
        /// <summary>
        /// flag of network initialisation
        /// </summary>
        protected bool NetworkWasInitialised = false;
        /// <summary>
        /// possible characters that can be recognized
        /// </summary>
        protected List<Char> CharactersToRecognize; 
        #endregion

        #region construct
        /// <summary>
        /// get a simple class to do OCR
        /// </summary>
        /// <param name="NumberOfSense">number of receptors</param>
        /// <returns>instance of class</returns>
        public OCR(int NumberOfSense = 17)
        {
            ImageSense = new MagicMatchSticks();
            ImageSense.Generate(NumberOfSense);

        } 
        #endregion

        #region learn the recognition
        /// <summary>
        /// start a new trainingsession to learn the new imagedatas
        /// </summary>
        /// <param name="TrainingImageData">data (images) to learn</param>
        public void StartTrainingSession(Dictionary<Char, List<ImageData>> TrainingImageData)
        {
            if (!NetworkWasInitialised)
            {
                CharactersToRecognize = new List<Char>(TrainingImageData.Keys);
                WorkingNeuralNetwork = new NeuralNetwork(ImageSense.Num(), new int[] { ImageSense.Num(), 10, 10, CharactersToRecognize.Count });
                WorkingNeuralNetwork.ShuffleBoth();
                NetworkWasInitialised = true;
            }


            // we need random here
            Random rand = new Random();

            const float BAD_CHARACTER = -0.5f;      // the expected output for a not matching character
            const float GOOD_CHARACTER = 0.5f;      // the expected output for a matching character

            // precalculate the expected output
            float[] ExpectedOutput = new float[CharactersToRecognize.Count];
            for (int j = 0; j < CharactersToRecognize.Count; j++)
                ExpectedOutput[j] = BAD_CHARACTER;

            // remember last character to calculate the expected output faster
            int LastCharacter = 0;

            int TotalLearningData = 0;
            for (int chars = 0; chars < CharactersToRecognize.Count; chars++)
                TotalLearningData += TrainingImageData[CharactersToRecognize[chars]].Count;

            // raw learning - data
            float[][] X = new float[TotalLearningData][];
            float[][] Y = new float[TotalLearningData][];

            int LearningdataPointer = 0;
            int NumberOfInputs = ImageSense.Num();
            int NumberOfOutputs = CharactersToRecognize.Count;

            for (int CurrentCharacter = 0; CurrentCharacter < CharactersToRecognize.Count; CurrentCharacter++)
            {

                ExpectedOutput[LastCharacter] = BAD_CHARACTER;
                ExpectedOutput[CurrentCharacter] = GOOD_CHARACTER;
                LastCharacter = CurrentCharacter;
                if (TrainingImageData[CharactersToRecognize[CurrentCharacter]].Count != 0)
                {
                    for (int imgs = 0; imgs < TrainingImageData[CharactersToRecognize[CurrentCharacter]].Count; imgs++)
                    {
                        float[] CurrentState = ImageSense.GetMagicMatchSticksState(TrainingImageData[CharactersToRecognize[CurrentCharacter]][imgs]);
                        X[LearningdataPointer] = new float[NumberOfInputs];
                        Y[LearningdataPointer] = new float[NumberOfOutputs];
                        for (int a = 0; a < NumberOfInputs; a++)
                            X[LearningdataPointer][a] = CurrentState[a];
                        for (int a = 0; a < NumberOfOutputs; a++)
                            Y[LearningdataPointer][a] = ExpectedOutput[a];
                        LearningdataPointer++;
                    }
                }

            }
            WorkingNeuralNetwork.Teacher.MaximumOfEpochs = 1000;
            WorkingNeuralNetwork.Learn(X, Y);
        }

        /// <summary>
        /// calculate the prediction error of a textsuite
        /// </summary>
        /// <param name="TestSuite">Training data as list of pictures(imagedata) and key (character)</param>
        /// <returns></returns>
        public float PredictionError(Dictionary<Char, List<ImageData>> TestSuite)
        {
            int DatasetSize = 0;
            int OccuredErrors = 0;

            for (int chars = 0; chars < CharactersToRecognize.Count; chars++)
            {
                Char CurrentCharacter = CharactersToRecognize[chars];
                if (TestSuite.ContainsKey(CurrentCharacter))
                    if (TestSuite[CurrentCharacter].Count != 0)
                        for (int imgs = 0; imgs < TestSuite[CharactersToRecognize[chars]].Count; imgs++)
                        {
                            DatasetSize++;
                            if (CurrentCharacter != Recognize(TestSuite[CharactersToRecognize[chars]][imgs]))
                                OccuredErrors++;
                        }
            }
            return (float)OccuredErrors / DatasetSize;
        }

        /// <summary>
        /// get the pattern of the magic sticks, which the network learns
        /// </summary>
        /// <param name="Img">image of character</param>
        /// <returns>pattern of magic sticks as array of floats (0.0f, 1.0f)</returns>
        public float[] GetMagicSticksPattern(ImageData Img)
        {
            return ImageSense.GetMagicMatchSticksState(Img);
        }

        /// <summary>
        /// get the output of the neural network
        /// </summary>
        /// <param name="input"></param>
        /// <returns>network output as array of float</returns>
        public float[] GetNetworkOutput(float[] input)
        {
            return WorkingNeuralNetwork.Output(input);
        } 
        #endregion

        #region apply the network to an unkown image
        /// <summary>
        /// recognize a character in image
        /// </summary>
        /// <param name="Img">image of character</param>
        /// <returns>detected character (this is possible wrong, if the training wasn't successfull)</returns>
        public Char Recognize(ImageData Img)
        {
            float[] Output = WorkingNeuralNetwork.Output(ImageSense.GetMagicMatchSticksState(Img));
            var indexAtMax = Output.ToList().IndexOf(Output.Max());
            return CharactersToRecognize[indexAtMax];
        } 
        #endregion

        #region file io
        /// <summary>
        /// save the OCR data in a binary formated file
        /// </summary>
        /// <param name="file">the target file path</param>
        public void Store(string file)
        {
            IFormatter binFmt = new BinaryFormatter();
            Stream s = File.Open(file, FileMode.Create);
            binFmt.Serialize(s, this);
            s.Close();
        }
        /// <summary>
        /// load a OCR data file from a binary formated file
        /// </summary>
        /// <param name="file">the neural network file file</param>
        /// <returns></returns>
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
        #endregion
    
    }
}
