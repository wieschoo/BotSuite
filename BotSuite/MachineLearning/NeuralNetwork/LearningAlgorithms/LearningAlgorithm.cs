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
        protected float _error_treshold;
        protected int _maximum_of_iterations;
        protected float[][] InputTrainingData;
        protected float[][] OutputTrainingData;

        #endregion

        #region PUBLIC ACCES TO LEARNING ALGORITHM STATE
        public NeuralNetwork ANN
        {
            get;
            protected set;
        }
        public float MeanSquareError
        {
            get;
            protected set;
        }
        public float ErrorTreshold
        {
            get { return _error_treshold; }
            set { _error_treshold = (value > 0) ? value : _error_treshold; }
        }
        iLearningMonitor _monitor;
        public iLearningMonitor LearningMonitor
        {
            get { return _monitor; }
            set { _monitor = value; }
        }
        public int CurrentIteration
        {
            get;
            protected set;
        }
        public int MaximumOfIteration
        {
            get { return _maximum_of_iterations; }
            set { _maximum_of_iterations = (value > 0) ? value : _maximum_of_iterations; }
        }

        #endregion

        #region constructor

        /// <param name="N">ANN to train</param>
        public LearningAlgorithm(NeuralNetwork N)
        {
            ANN = N;
            ErrorTreshold = 0.005f;
            MaximumOfIteration = 10000;
            CurrentIteration = 0;
            MeanSquareError = -1.0f;
        }
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

        protected bool Convergence()
        {
            // custom check, if the learning phase should be stopped
            return (_monitor != null) ? _monitor.Convergence() : false;
        }

        protected void Pulse()
        {
            // send current progress to learn monitor
            if (_monitor != null) _monitor.Pulse(CurrentIteration, MeanSquareError);
        }
    }
}
