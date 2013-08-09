using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotSuite.MachineLearning.NeuralNetwork.LearningMonitors;

namespace BotSuite.MachineLearning.NeuralNetwork.LearningAlgorithms
{
    /// <summary>
    /// The abstract class describing a learning
    /// algorithm for a neural network
    /// </summary>
    [Serializable]
    public abstract class LearningAlgorithm
    {

        #region PROTECTED FIELDS
        /// <summary>
        /// intern error threshold
        /// </summary>
        protected float _error_treshold;
        /// <summary>
        /// the maximum of epochs in gradient descent
        /// </summary>
        protected int _maximum_of_epochs;
        /// <summary>
        /// the input training data
        /// </summary>
        protected float[][] InputTrainingData;
        /// <summary>
        /// the corresponding expected output for training data
        /// </summary>
        protected float[][] OutputTrainingData;
        /// <summary>
        /// intern variable for extern monitoring
        /// </summary>
        protected iLearningMonitor _monitor;
        #endregion

        #region public properties
        /// <summary>
        /// the neural network to learn
        /// </summary>
        public NeuralNetwork ANN
        {
            get;
            protected set;
        }
        /// <summary>
        /// the mean square error in training data
        /// </summary>
        public float MeanSquareError
        {
            get;
            protected set;
        }
        /// <summary>
        /// access to the error threshold
        /// </summary>
        public float ErrorTreshold
        {
            get { return _error_treshold; }
            set { _error_treshold = (value > 0) ? value : _error_treshold; }
        }
        
        /// <summary>
        /// extern monitor to get information about progress in learning
        /// </summary>
        public iLearningMonitor LearningMonitor
        {
            get { return _monitor; }
            set { _monitor = value; }
        }
        /// <summary>
        /// the current epoch
        /// </summary>
        public int CurrentEpoch
        {
            get;
            protected set;
        }
        /// <summary>
        /// get or set maximum of epoch
        /// </summary>
        public int MaximumOfEpochs
        {
            get { return _maximum_of_epochs; }
            set { _maximum_of_epochs = (value > 0) ? value : _maximum_of_epochs; }
        }

        #endregion

        #region constructor

        /// <param name="N">ANN to train</param>
        public LearningAlgorithm(NeuralNetwork N)
        {
            ANN = N;
            ErrorTreshold = 0.005f;
            MaximumOfEpochs = 10000;
            CurrentEpoch = 0;
            MeanSquareError = -1.0f;
        }
        /// <summary>
        /// base function to learn data
        /// </summary>
        /// <param name="inputs">input data for training</param>
        /// <param name="expected_outputs">th corresponding output data</param>
        public virtual void Learn(float[][] inputs, float[][] expected_outputs)
        {
            if (expected_outputs.Length < 1)
                throw new Exception("LearningAlgorithm -> missing OutputTrainingData");
            if (inputs.Length < 1)
                throw new Exception("LearningAlgorithm -> missing InputTrainingData");
            if (inputs.Length != expected_outputs.Length)
                throw new Exception("LearningAlgorithme -> length of input and output doesn't match ");

            InputTrainingData = inputs;
            OutputTrainingData = expected_outputs;
        }

        #endregion
        /// <summary>
        /// custom check for convergence
        /// </summary>
        /// <returns></returns>
        protected bool Convergence()
        {
            // custom check, if the learning phase should be stopped
            return (_monitor != null) ? _monitor.Convergence() : false;
        }
        /// <summary>
        /// pulse in every epoch of stochastic gradient descent
        /// </summary>
        protected void Pulse()
        {
            // send current progress to learn monitor
            if (_monitor != null) _monitor.Pulse(CurrentEpoch, MeanSquareError);
        }
    }
}
