using Core.Utilities.Results;

namespace Core.Utilities.Business
{
    public class BusinessRunner
    {
        public static IResult CheckDifferent(ResultType[] resultTypes, params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                foreach (var resultType in resultTypes)
                {
                    if (resultType != logic.ResultType)
                    {
                        return logic;
                    }
                }
            }

            return null;
        }

        public static IResult CheckAny(ResultType[] resultTypes, params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                foreach (var resultType in resultTypes)
                {
                    if (resultType == logic.ResultType)
                    {
                        return logic;
                    }
                }
            }

            return null;
        }
    }
}