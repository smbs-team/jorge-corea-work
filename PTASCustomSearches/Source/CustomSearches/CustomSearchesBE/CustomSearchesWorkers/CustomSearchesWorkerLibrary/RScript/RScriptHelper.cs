using RDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSearchesWorkerLibrary.RScript
{
    /// <summary>
    /// R Scripting worker class
    /// </summary>
    public static class RScriptHelper
    {
        static REngine engine;


        public static string EvaluateAsString(string script)
        {
            SymbolicExpression resultObj = Evaluate(script);
            CharacterVector resultVector = resultObj.AsCharacter();
            string[] result = { "" };

            if (resultVector != null)
            {
                result = resultVector.ToArray();
            }

            return result[0];
        }

        public static double[] EvaluateAsNumericVector(string script)
        {
            SymbolicExpression resultObj = Evaluate(script);
            NumericVector resultVector = resultObj.AsNumeric();
            double[] toReturn = resultVector.ToArray();

            return toReturn;        
        }

        /// <summary>
        /// Executes the r script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns>Results from the script.</returns>
        public static SymbolicExpression Evaluate(string script)
        {
            REngine.SetEnvironmentVariables(); // <-- May be omitted; next line would call it.
            REngine engine = RScriptHelper.GetEngine();

            // A somewhat contrived but customary Hello World:
            return  engine.Evaluate(script);
        }

        private static REngine GetEngine()
        {
            if (engine == null)
            {
                engine = REngine.GetInstance();
            }

            return engine;
        }

        public static void Dispose()
        {
            engine.Dispose();
        }
    }
}
