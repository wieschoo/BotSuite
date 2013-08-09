using System;
using BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions;
namespace BotSuite.MachineLearning.NeuralNetwork
{
    
    /// <summary>
    /// this class represents a neuron
    /// </summary>
    /// <remarks>
    /// output = f( threshold* \sum_{j=1}^{N_Inputs} s_{t-1,j}w_j ) 
    ///</remarks>
    [Serializable]
    public class Neuron
    {

        #region protected properties
        /// <summary>
        /// intern instance of a PseudoRandomNumberGenerator
        /// </summary>
        protected static Random prng = new Random();
        /// <summary>
        /// intern interval for randomisation
        /// </summary>
        protected float _intervall_min = -1.0f, _intervall_max = 1.0f;
        /// <summary>
        /// intern weights of inputs
        /// </summary>
        protected float[] _weight,_remember_weight;
        /// <summary>
        /// intern theshold variable
        /// </summary>
        protected float _threshold = 0f,_remember_threshold = 0f;
        /// <summary>
        /// intern handle of the activation function
        /// </summary>
        protected iActivationFunction _f = null;
        /// <summary>
        /// the output
        /// </summary>
        protected float _neuron_output = 0f;
        /// <summary>
        /// values for faster computation
        /// </summary>
        protected float _synapsevalue_with_threshold = 0f,_usefull_variable; 
        #endregion

        #region public properties
        /// <summary>
        /// returns number of neurons which affect this neuron
        /// </summary>
        public int NumberOfNeurons
        {
            get { return _weight.Length; }
        }
        /// <summary>
        /// pointer to all neurons that affect this neuron
        /// </summary>
        /// <param name="NeuronIdx"></param>
        /// <returns></returns>
        public float this[int NeuronIdx]
        {
            get { return _weight[NeuronIdx]; }
            set { _remember_weight[NeuronIdx] = _weight[NeuronIdx]; _weight[NeuronIdx] = value; }
        }
        /// <summary>
        /// thresholding
        /// </summary>
        public float Threshold
        {
            get { return _threshold; }
            set { _remember_threshold = _threshold; _threshold = value; }
        }
        /// <summary>
        /// remember last threshold
        /// </summary>
        public float Last_Threshold
        {
            get { return _remember_threshold; }
        }
        /// <summary>
        /// get the output of this neuron
        /// </summary>
        public float Output
        {
            get { return _neuron_output; }
        }
        /// <summary>
        /// get gradient value for stochastic gradient descent
        /// </summary>
        public float Derivative
        {
            get { return _f.Derivative(_synapsevalue_with_threshold); }
        }
        /// <summary>
        /// this is useful for calculations
        /// </summary>
        public float LastSumOfInputsWithTreshold
        {
            get { return _synapsevalue_with_threshold; }
        }
        /// <summary>
        /// set or get the activationfunction
        /// </summary>
        public iActivationFunction F
        {
            get { return _f; }
            set { _f = value; }
        }
        /// <summary>
        /// simplify the calculation
        /// </summary>
        public float UsefulVariable
        {
            get { return _usefull_variable; }
            set { _usefull_variable = value; }
        }
        /// <summary>
        /// remember last weights
        /// </summary>
        public float[] Last_Weight
        {
            get { return _remember_weight; }
        }
        /// <summary>
        /// Get or set the minimum value for randomisation of weights and threshold
        /// </summary>
        public float Randomization_LeftIntervall
        {
            get { return _intervall_min; }
            set { _intervall_min = value; }
        }
        /// <summary>
        /// get or set the right end of interval value for randomization
        /// </summary>
        public float Randomization_RightIntervall
        {
            get { return _intervall_max; }
            set { _intervall_max = value; }
        } 
        #endregion

        #region constructor
        /// <summary>
        /// initialise the neuron
        /// </summary>
        /// <param name="NumberOfInputs">number of inputs in layer before</param>
        /// <param name="ActivationFunction">activation function for current neuron (default: sigmoid function)</param>
        public Neuron(int NumberOfInputs, iActivationFunction ActivationFunction=null)
        {
            _remember_weight = new float[NumberOfInputs];
            _weight = new float[NumberOfInputs];
            if(ActivationFunction!=null)
                _f = ActivationFunction;
            else
                _f = new SigmoidActivationFunction();
        }
        #endregion

        #region initialise neurons
        /// <summary>
        /// set random weights
        /// </summary>
        public void ShuffleWeight()
        {
            for (int i = 0; i < NumberOfNeurons; i++)
            {
                _remember_weight[i] = 0f;
                _weight[i] = _intervall_min + (((float)(prng.Next(1000))) / 1000f) * (_intervall_max - _intervall_min);
                
            }
        }
        /// <summary>
        /// set random threshold
        /// </summary>
        public void ShuffleThreshold()
        {
            _threshold = _intervall_min + (((float)(prng.Next(1000))) / 1000f) * (_intervall_max - _intervall_min);
        }
        /// <summary>
        /// set all random
        /// </summary>
        public void ShuffleBoth()
        {
            ShuffleThreshold();
            ShuffleWeight();
            
        } 
        #endregion

        #region gain output
        /// <summary>
        /// get the signal from this neuron
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public float Signal(float[] input)
        {
            if (input.Length != NumberOfNeurons)
                throw new Exception("Network -> neuron-> wrong size of input vector " + input.Length.ToString() + " should be " + NumberOfNeurons.ToString());

            _synapsevalue_with_threshold = 0;

            for (int i = 0; i < NumberOfNeurons; i++)
                _synapsevalue_with_threshold += _weight[i] * input[i];
            _synapsevalue_with_threshold -= _threshold;

            if (_f != null)
                _neuron_output = _f.Function(_synapsevalue_with_threshold);
            else
                _neuron_output = _synapsevalue_with_threshold;

            return _neuron_output;
        } 
        #endregion

    }
}
