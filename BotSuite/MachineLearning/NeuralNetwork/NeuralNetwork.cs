using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions;
using BotSuite.MachineLearning.NeuralNetwork.LearningAlgorithms;

namespace BotSuite.MachineLearning.NeuralNetwork
{
    
    /// <summary>
    /// represent an artificial feed forward neural network
    /// </summary>
    [Serializable]
    public class NeuralNetwork
    {

        #region protected properties
        /// <summary>
        /// store the layers
        /// </summary>
        protected Layer[] NetworkLayers;
        /// <summary>
        /// number of input neurons
        /// </summary>
        protected int ProtectedNumberOfInputs;
        /// <summary>
        /// the learning algorithmn to improve weights
        /// </summary>
        protected LearningAlgorithm TeacherAlgorithm;

        #endregion

        #region public properties
        /// <summary>
        /// get number of input neurons
        /// </summary>
        public int NumberOfInputs
        {
            get { return ProtectedNumberOfInputs; }
        }
        /// <summary>
        /// get number of output neurons
        /// </summary>
        public int NumberOfOutputs
        {
            get { return NetworkLayers[NumberOfLayers - 1].NeuronsInLayer; }
        }
        /// <summary>
        /// get number of hidden layers
        /// </summary>
        public int NumberOfLayers
        {
            get { return NetworkLayers.Length; }
        }
        /// <summary>
        /// the learning algorithm
        /// </summary>
        public LearningAlgorithm Teacher
        {
            get { return TeacherAlgorithm; }
            set { TeacherAlgorithm = (value != null) ? value : TeacherAlgorithm; }
        }
        /// <summary>
        /// pointer to hidden layer n
        /// </summary>
        /// <param name="n">index n</param>
        /// <returns></returns>
        public Layer this[int n]
        {
            get { return NetworkLayers[n]; }
        }
        /// <summary>
        /// activation function the each neuron in each layer
        /// </summary>
        public iActivationFunction f
        {
            set
            {
                foreach (Layer l in NetworkLayers)
                    l.ActivationFunction = value;
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// initialise neural network
        /// </summary>
        /// <param name="NumberOfInputs">the number of input vectors</param>
        /// <param name="LayerVectorDescription">description of hidden layers</param>
        /// <param name="ActivationFunction">the activation function (default sigmoid)</param>
        /// <param name="Learner">Learning algorithm (default: backpropagation)</param>
        public NeuralNetwork(int NumberOfInputs, int[] LayerVectorDescription, iActivationFunction ActivationFunction=null, LearningAlgorithm Learner=null)
        {
            if (LayerVectorDescription.Length < 1)
                throw new Exception("NeuralNetwork -> cannot be built, since there are no neurons");
            if (NumberOfInputs < 1)
                throw new Exception("NeuralNetwork -> cannot be built, since it need at least 1 input");

            ProtectedNumberOfInputs = NumberOfInputs;
            if (Learner != null)
                TeacherAlgorithm = Learner;
            else
                TeacherAlgorithm = new BackPropagationLearningAlgorithm(this);
            
            NetworkLayers = new Layer[LayerVectorDescription.Length];
            NetworkLayers[0] = new Layer(LayerVectorDescription[0], ProtectedNumberOfInputs);
            for (int i = 1; i < LayerVectorDescription.Length; i++)
                NetworkLayers[i] = new Layer(LayerVectorDescription[i], LayerVectorDescription[i - 1], ActivationFunction);
        }
        #endregion

        #region helpers
        /// <summary>
        /// set ranom weights to each connection
        /// </summary>
        public void ShuffleWeight()
        {
            foreach (Layer l in NetworkLayers)
                l.ShuffleWeight();
        }
        /// <summary>
        /// set random threshold to each neuron
        /// </summary>
        public void ShuffleThreshold()
        {
            foreach (Layer l in NetworkLayers)
                l.ShuffleThreshold();
        }
        /// <summary>
        /// everything is random
        /// </summary>
        public void ShuffleBoth()
        {
            foreach (Layer l in NetworkLayers)
                l.ShuffleBoth();
        }
        /// <summary>
        /// set interval for random numbers
        /// </summary>
        /// <param name="min">left interval ending</param>
        /// <param name="max">right interval ending</param>
        public void SetIntervalForPRNG(float min, float max)
        {
            foreach (Layer l in NetworkLayers)
                l.SetIntervalForPRNG(min, max);
        } 
        #endregion

        #region file i/o
        /// <summary>
        /// store the network as a binary file
        /// </summary>
        /// <code>
        /// NeuralNetwork ANN = new NeuralNetwork(...);
        /// Ann.Store("savednetwork.bin");
        /// </code>
        /// <param name="file">file name</param>
        public void Store(string file)
        {
            IFormatter binFmt = new BinaryFormatter();
            Stream s = File.Open(file, FileMode.Create);
            binFmt.Serialize(s, this);
            s.Close();
        }
        /// <summary>
        /// load the neural network from binary file
        /// </summary>
        /// <code>
        /// NeuralNetwork ANN = NeuralNetwork.Load("savednetwork.bin");
        /// </code>
        /// <param name="file"></param>
        /// <returns></returns>
        public static NeuralNetwork Load(string file)
        {
            NeuralNetwork result;
            try
            {
                IFormatter binFmt = new BinaryFormatter();
                Stream s = File.Open(file, FileMode.Open);
                result = (NeuralNetwork)binFmt.Deserialize(s);
                s.Close();
            }
            catch (Exception e)
            {
                throw new Exception("NeuralNetwork : Unable to load file " + file + " : " + e);
            }
            return result;
        } 
        #endregion

        #region wrapper
        /// <summary>
        /// wrapper for direct learning
        /// </summary>
        /// <param name="inputs">input training data</param>
        /// <param name="expected_outputs">the expected output of training data</param>
        public void Learn(float[][] inputs, float[][] expected_outputs)
        {
            Teacher.Learn(inputs, expected_outputs);
        }
        /// <summary>
        /// calculates the output for specific input
        /// </summary>
        /// <param name="input">input vector</param>
        /// <returns>output vector</returns>
        public float[] Output(float[] input)
        {
            if (input.Length != ProtectedNumberOfInputs)
                throw new Exception("PERCEPTRON : Wrong input vector size, unable to compute output value");
            float[] result;
            result = NetworkLayers[0].Signal(input);
            for (int i = 1; i < NumberOfLayers; i++)
                result = NetworkLayers[i].Signal(result);
            return result;
        }
        /// <summary>
        /// calculates the prediction error (good for cross validation)
        /// </summary>
        /// <param name="inputs">set of training data/ test data</param>
        /// <param name="expected_outputs">the expected output</param>
        /// <returns></returns>
        public float PredictionError(float[][] inputs, float[][] expected_outputs)
        {
            float MSE = 0.0f;
            for (int i = 0; i < inputs.Length;i++ )
            {
                float[] result = Output(inputs[i]);
                for (int j = 0; j < result.Length;j++ )
                    MSE += (result[j] - expected_outputs[i][j]) * (result[j] - expected_outputs[i][j]);
            }
            return MSE;
        }
        #endregion
      
    }
}
