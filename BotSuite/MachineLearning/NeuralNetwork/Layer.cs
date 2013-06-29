using System;
using BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions;
namespace BotSuite.MachineLearning.NeuralNetwork
{
    
    /// <summary>
    /// organize the neurons
    /// </summary> 
    [Serializable]
    public class Layer
    {    

        #region protected properties
        /// <summary>
        /// number of neurons in layers
        /// </summary>
        protected int _neurons_in_layer;
        /// <summary>
        /// number of inputs
        /// </summary>
        protected int _inputs_of_layer;
        /// <summary>
        /// internal array of neurons
        /// </summary>
        protected Neuron[] _neurons;
        /// <summary>
        /// the output ofthe layer
        /// </summary>
        protected float[] _output;
        #endregion

        #region public properties
        /// <summary>
        /// set activation function for all neurons in layer
        /// </summary>
        public iActivationFunction ActivationFunction
        {
            set{
                foreach (Neuron n in _neurons)
                    n.F = value;
            }

        }
        /// <summary>
        /// get number of neurons in current layer
        /// </summary>
        public int NeuronsInLayer
        {
            get { return _neurons_in_layer; }
        }
        /// <summary>
        /// get number of inputs in current layer
        /// </summary>
        public int InputsOfLayer
        {
            get { return _inputs_of_layer; }
        }
        /// <summary>
        /// pointer to specific neuron
        /// </summary>
        /// <param name="NeuronIdx">index of neuron</param>
        /// <returns></returns>
        public Neuron this[int NeuronIdx]
        {
            get { return _neurons[NeuronIdx]; }
        }
        /// <summary>
        /// get last output of the neurons in this layer
        /// </summary>
        public float[] GetLastOuput
        {
            get { return _output; }
        }
        #endregion

        #region build layers
        /// <summary>
        /// initialise a layer containing x inputs and n neurons
        /// </summary>
        /// <param name="NumberOfInputs">how many inputs do we have</param>
        /// <param name="NumberOfNeurons">how many neurons should this layer contain</param>
        /// <param name="f">activation function foreach neuron in this layer</param>
        public Layer(int NumberOfInputs, int NumberOfNeurons, iActivationFunction f=null)
        {
            _neurons_in_layer = NumberOfInputs;
            _inputs_of_layer = NumberOfNeurons;
            this._neurons = new Neuron[_neurons_in_layer];
            _output = new float[_neurons_in_layer];
            for (int i = 0; i < NumberOfInputs; i++)
                this._neurons[i] = new Neuron(NumberOfNeurons, f);
        }
        #endregion

        #region FUNCTION TO INITIALISE
        /// <summary>
        /// set random weights to each neuron
        /// </summary>
        public void ShuffleWeight()
        {
            foreach (Neuron n in _neurons)
                n.ShuffleWeight();
        }
        /// <summary>
        /// set random thresholds to each neuron
        /// </summary>
        public void ShuffleThreshold()
        {
            foreach (Neuron n in _neurons)
                n.ShuffleThreshold();
        }
        /// <summary>
        /// everything is random in this layer
        /// </summary>
        public void ShuffleBoth()
        {
            ShuffleThreshold();
            ShuffleWeight();
            
        }
        /// <summary>
        /// Set the randomization interval for all neurons
        /// </summary>
        /// <param name="min">the minimum value</param>
        /// <param name="max">the maximum value</param>
        public void SetIntervalForPRNG(float min, float max)
        {
            foreach (Neuron n in _neurons)
            {
                n.Randomization_LeftIntervall = min;
                n.Randomization_RightIntervall = max;
                
            }
        }
        #endregion

        #region gain output
        /// <summary>
        /// get the signal from each neuron as a vector
        /// </summary>
        /// <param name="input">input vector</param>
        /// <returns>vector of neuron output</returns>
        public float[] Signal(float[] input)
        {
            if (input.Length != _inputs_of_layer)
                throw new Exception("LAYER : Wrong input vector size, unable to compute output value");
            for (int i = 0; i < _neurons_in_layer; i++)
                _output[i] = _neurons[i].Signal(input);
            return _output;
        }

        #endregion
    }
}
