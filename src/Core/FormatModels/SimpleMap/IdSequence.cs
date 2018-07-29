// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using System.Linq.Expressions;

namespace SectorDirector.Core.FormatModels.SimpleMap
{
    public sealed class IdSequence<TId> where TId : struct
    {
        private readonly ConstructorDelegate _idConstructor;
        private int _current = 0;

        public IdSequence()
        {
            _idConstructor = CreateConstructor(typeof(TId), typeof(int));
        }

        public TId GetNext() => (TId)_idConstructor(_current++);

        
        // This stuff below is because there is no generic constraint for a constructor signature.
        // All of the ID structs have a constructor that takes an int.
        // This uses Expression Trees to get access to that constructor so we can call it.
        // https://stackoverflow.com/questions/840261/passing-arguments-to-c-sharp-generic-new-of-templated-type#840299

        // this delegate is just, so you don't have to pass an object array. _(params)_
        private delegate object ConstructorDelegate(params object[] args);

        private static ConstructorDelegate CreateConstructor(Type type, params Type[] parameters)
        {
            // Get the constructor info for these parameters
            var constructorInfo = type.GetConstructor(parameters);

            // define a object[] parameter
            var paramExpr = Expression.Parameter(typeof(Object[]));

            // To feed the constructor with the right parameters, we need to generate an array 
            // of parameters that will be read from the initialize object array argument.
            var constructorParameters = parameters.Select((paramType, index) =>
                // convert the object[index] to the right constructor parameter type.
                Expression.Convert(
                    // read a value from the object[index]
                    Expression.ArrayAccess(
                        paramExpr,
                        Expression.Constant(index)),
                    paramType)).ToArray();

            // just call the constructor.
            var body = Expression.New(constructorInfo, constructorParameters);

            var constructor = Expression.Lambda<ConstructorDelegate>(body, paramExpr);
            return constructor.Compile();
        }
    }
}