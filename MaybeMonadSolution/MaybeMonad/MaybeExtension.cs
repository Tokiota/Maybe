namespace MaybeMonad
{
    using System;

    public static class MaybeExtension
    {
        #region Public Methods and Operators

        public static Maybe<TReturnType> Select<TInput, TReturnType>(this Maybe<TInput> input,
                                                                     Func<TInput, TReturnType> selectEvaluator)
        {
            return input.HasValue
                       ? selectEvaluator(input.Value).ToMaybe()
                       : Maybe<TReturnType>.Nothing;
        }

        public static Maybe<TReturnType> SelectOrDefault<TInput, TReturnType>(this Maybe<TInput> input
                                                                              ,
                                                                              Func<TInput, TReturnType> selectEvaluator
                                                                              ,
                                                                              Func<TReturnType> selectDefault)
        {
            Maybe<TReturnType> result = input.Select(selectEvaluator);

            return result.HasValue
                       ? result
                       : selectDefault().ToMaybe();
        }

        public static Maybe<T> ToMaybe<T>(this T valor)
        {
            return typeof(T).IsValueType
                       ? new Maybe<T>(valor, true)
                       : new Maybe<T>(valor, valor != null);
                              //? new Maybe<T>(valor, false)
                              //: new Maybe<T>(valor, true);
        }

        

        #endregion
    }
}