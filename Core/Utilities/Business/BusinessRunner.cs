using Core.Utilities.Results;

namespace Core.Utilities.Business
{
    public class BusinessRunner
    {
        public static IResult CheckAnyDifferent(ResultType resultType, params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (resultType != logic.ResultType)
                {
                    return logic;
                }
            }

            return null;
        }

        public static IResult CheckAny(ResultType resultType, params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (resultType == logic.ResultType)
                {
                    return logic;
                }
            }

            return null;
        }
    }
}