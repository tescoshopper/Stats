using System;
using System.IO;
using NUnit.Framework;

namespace Stats
{
    /// <summary>
    /// Singleton containing functions to compute statical (arithmetic mean) data
    /// from a matrix of real numbers.
    /// </summary>
    public sealed class Averages
    {
        #region "Constructor"

        private Averages() { }

        #endregion

        #region "Public members"

        public static Averages Functions => functions;

        /// <summary>
        /// Calculate arithmetic mean from a matrix of real numbers.
        /// </summary>
        /// <param name="matrix">unbounded decimal array.</param>
        /// <returns>Decimal.</returns>
        public decimal CalculateMean(decimal[] matrix)
        {
            int matrixLength = matrix.Length;
            decimal matrixValue = 0;

            for (int i = 0; i < matrixLength; i++)
            {
                matrixValue += matrix[i];
            }

            return matrixValue / matrixLength;
        }

        /// <summary>
        /// Calculate standard deviation from a matrix of real numbers.
        /// </summary>
        /// <param name="matrix">Unbounded decimal array.</param>
        /// <returns>Decimal.</returns>
        public decimal CalculateStandardDeviation(decimal[] matrix)
        {
            int matrixLength = matrix.Length;
            decimal matrixMean = CalculateMean(matrix);
            decimal matrixStandardDeviation;
            decimal matrixCumulative = 0;

            for (int i = 0; i < matrixLength; i++)
            {
                matrixStandardDeviation = matrix[i] - matrixMean;
                matrixCumulative += matrixStandardDeviation * matrixStandardDeviation;
            }

            return CalculateSquareRoot(matrixCumulative / matrixLength);
        }

        /// <summary>
        /// Calculate frequencies from a matrix of real numbers.
        /// </summary>
        /// <param name="matrix">Unbounded decimal array.</param>
        /// <returns>Unbounded integer array.</returns>
        public int[] CalculateFrequencies(decimal[] matrix)
        {
            int[] matrixFrequencies = new int[10];

            for (int i = 0; i < matrix.Length; i++)
            {
                matrixFrequencies[(int)(matrix[i] / 10)] += 1;
            }

            return matrixFrequencies;
        }

        #endregion

        #region "Private members"

        private static readonly Averages functions = new Averages();

        /// <summary>
        /// Calculate square root of a positive real number using the Babylonian technique.
        /// </summary>
        /// <param name="squareNumber">Decimal.</param>
        /// <returns>Decimal.</returns>
        private decimal CalculateSquareRoot(decimal squareNumber)
        {
            const decimal approximation = 0.000001M;
            decimal squareRoot = squareNumber;
            decimal dividend = 1;

            while ((squareRoot - dividend) > approximation)
            {
                squareRoot = (squareRoot + dividend) / 2;
                dividend = squareNumber / squareRoot;
            }

            return squareRoot;
        }

        #endregion

    }

    /// <summary>
    /// Unit tests for Averages class functions.
    /// </summary>
    [TestFixture]
    public class TestAverages
    {
        /// <summary>
        /// Unit test CalculateMean function.
        /// </summary>
        [Test]
        public void TestCalculateMean()
        {
            decimal meanAverage = Averages.Functions.CalculateMean(new decimal[] { 0, 5, 19, 27, 56, 57, 59, 89, 98, 99 });
            Assert.That(decimal.Round(meanAverage, 2), Is.EqualTo(50.90));
        }

        /// <summary>
        /// Unit test CalculateStandardDeviation function.
        /// </summary>
        [Test]
        public void TestCalculateStandardDeviation()
        {
            decimal standardDeviation = Averages.Functions.CalculateStandardDeviation(new decimal[] { 0, 5, 19, 27, 56, 57, 59, 89, 98, 99 });
            Assert.That(decimal.Round(standardDeviation, 2), Is.EqualTo(35.21));
        }

        /// <summary>
        /// Unit test CalculateFrequencies function.
        /// </summary>
        [Test]
        public void TestCalculateFrequencies()
        {
            int[] expectedFrequencies =  { 2, 1, 1, 0, 0, 3, 0, 0, 1, 2 };
            int[] actualFrequencies = Averages.Functions.CalculateFrequencies(new decimal[] { 0, 5, 19, 27, 56, 57, 59, 89, 98, 99 });
            
            Assert.That(actualFrequencies.Length, Is.EqualTo(10));

            for (int i = 0; i < actualFrequencies.Length; i++)
            {
                Assert.That(actualFrequencies[i], Is.EqualTo(expectedFrequencies[i]));
            }
        }
    }

        class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter full path of a one-line CSV file containing numeric only >=0 values <100");
                return 1;
            }

            // Handle all errors both expected and unexpected at this time.
            // Note this is a test harness only.
            try
            {
                decimal[] matrix = Array.ConvertAll<string, decimal>(File.ReadAllText(args[0]).Split(','), Convert.ToDecimal);

                Console.WriteLine();
                Console.WriteLine($"Matrix mean average: {Averages.Functions.CalculateMean(matrix)}");
                Console.WriteLine();
                Console.WriteLine($"Matrix standard deviation: {Averages.Functions.CalculateStandardDeviation(matrix)}");
                Console.WriteLine();
                Console.WriteLine("Matrix frequencies:");

                int[] matrixFrequencies = Averages.Functions.CalculateFrequencies(matrix);
                for (int i = 0; i < matrixFrequencies.Length; i++)
                {
                    Console.WriteLine($"Class: {i + 1}  Frequency: {matrixFrequencies[i]}");
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Operation aborted with the following error:");
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}
