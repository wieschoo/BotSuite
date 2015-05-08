using System;
using System.Collections;


namespace BotSuite.MachineLearning.NeuralNetwork.LearningAlgorithms
{
    /// <summary>
    /// stockastic gradient backpropagation learning algorithm
    /// </summary>
    /// <remarks>
    /// 
    /// Neuron j in L-1                   
    ///  o ---------> Signal f(W*S_j) 
    /// Neuron i in L 
    ///  o ---------> Signal f(W*S_i) 
    /// Neuron k in L+1
    /// 
    /// 
    /// for i
    /// 
    /// t_i(n+1)     = t_i(n)     - \alpha*EO_i*\gamma     * ( T_i(n)   - T_i(n-1))
    /// w_{i,j}(n+1) = w_{i,j}(n) + \alpha*S_j*EO_i+\gamma * (w_[i,j}(n)-w_{i,j}(n-1))
    /// 
    /// 
    ///		with :
    ///				EO_i = f'(WSi) * (\IE Ouput of i - S_i)        for output layer
    ///				EO_i = f'(WSi) * \sum_k  w_{k,i} EO_k  )       for others
    /// </remarks>
    [Serializable]
    public class BackPropagationLearningAlgorithm : LearningAlgorithm
    {

        #region protected variables
        /// <summary>
        /// parameters of learning algorithm
        /// </summary>
        protected float _alpha, _gamma;
        /// <summary>
        /// errors in iteration
        /// </summary>
        protected float[] _error_vector;
        

        #endregion

        #region public properties
        /// <summary>
        /// set or get learning rate (high values fast progress but no convergence, low values -> slow learning)
        /// </summary>
        public float LearningRate
        {
            get { return _alpha; }
            set { _alpha = (value <= 0.0f) ?  _alpha : value; }
        }
        /// <summary>
        /// set or get the rumelhart coefficient
        /// </summary>
        public float RumelhartCoefficient
        {
            get { return _gamma; }
            set { _gamma = (value > 0) ? value : _gamma; }
        }

        #endregion

        #region constructor
        /// <summary>
        /// default BackPropagation (alpha = 0.5 ; gamma = 0.2)
        /// </summary>
        /// <param name="ANN">network to train</param>
        public BackPropagationLearningAlgorithm(NeuralNetwork ANN): base(ANN)
        {
            LearningRate = 0.5f;
            RumelhartCoefficient = 0.2f;
        }

        #endregion

        #region learning
        /// <summary>
        /// backpropagation learning algorithm
        /// </summary>
        /// <param name="inputs">the input</param>
        /// <param name="expected_outputs">the expected output</param>
        public override void Learn(float[][] inputs, float[][] expected_outputs)
        {
            base.Learn(inputs, expected_outputs);
            float[] NeuronOutput;
            float MeanSquareError2;
            CurrentEpoch = 0;
            // for each epoch
            do
            {
                // currently no error
                MeanSquareError = 0f;
                _error_vector = new float[ANN.NumberOfOutputs];
                // run through set of training data
                for (int i = 0; i < InputTrainingData.Length; i++)
                {
                    MeanSquareError2 = 0f;
                    NeuronOutput = ANN.Output(inputs[i]);
                    for (int j = 0; j < NeuronOutput.Length; j++)
                    {
                        _error_vector[j] = OutputTrainingData[i][j] - NeuronOutput[j];
                        // update current error
                        MeanSquareError2 += _error_vector[j] * _error_vector[j];
                    }
                    MeanSquareError2 /= 2f;
                    MeanSquareError += MeanSquareError2;
                    CalculateUsefulVariable(i);
                    // update weight
                    SetWeight(i);
                }
                CurrentEpoch++;
                // we aren't dead
                Pulse();
                // custom check if we have to run further
                if (Convergence()) break;
            }
            while (CurrentEpoch < _maximum_of_epochs && MeanSquareError > _error_treshold);

        }
        /// <summary>
        /// to speed up the process of weight updating we use this variable
        /// </summary>
        /// <param name="i"></param>
        protected void CalculateUsefulVariable(int i)
        {
            float s_k;
            int l = ANN.NumberOfLayers - 1;
            // last layer
            for (int j = 0; j < ANN[l].NeuronsInLayer; j++)
                ANN[l][j].UsefulVariable = ANN[l][j].Derivative * _error_vector[j];
            // other layers before
            for (l--; l >= 0; l--)
            {
                for (int j = 0; j < ANN[l].NeuronsInLayer; j++)
                {
                    s_k = 0f;
                    for (int k = 0; k < ANN[l + 1].NeuronsInLayer; k++)
                        s_k += ANN[l + 1][k].UsefulVariable * ANN[l + 1][k][j];
                    ANN[l][j].UsefulVariable = ANN[l][j].Derivative * s_k;
                }
            }
        }
        /// <summary>
        /// update the weight (see delta rule and chain rule for derivatives) and update affecting neurons
        /// </summary>
        /// <param name="NewWeight">new weight to set</param>
        protected void SetWeight(int NewWeight)
        {
            float[] lin;
            for (int j = 0; j < ANN.NumberOfLayers; j++)
            {
                if (j == 0) lin = InputTrainingData[NewWeight];
                else lin = ANN[j - 1].GetLastOuput;
                for (int n = 0; n < ANN[j].NeuronsInLayer; n++)
                {
                    for (int k = 0; k < ANN[j][n].NumberOfNeurons; k++)
                        ANN[j][n][k] += _alpha * lin[k] * ANN[j][n].UsefulVariable + _gamma * (ANN[j][n][k] - ANN[j][n].Last_Weight[k]);
                    ANN[j][n].Threshold -= _alpha * ANN[j][n].UsefulVariable + _gamma * (ANN[j][n].Threshold - ANN[j][n].Last_Threshold);
                }
            }
        }

        #endregion

        

    }
}
