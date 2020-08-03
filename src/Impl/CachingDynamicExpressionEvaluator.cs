using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Deeva.CodeRefiner;

namespace Deeva.Impl
{
    public class CachingDynamicExpressionEvaluator<TOut, TIn, TIn2> : GenericDynamicExpressionEvaluator<TOut, TIn, TIn2>
    {
        private readonly MD5 _md5Hasher;

        /// <summary>
        /// stores an MD5 hash of the class and the compilation object
        /// </summary>
        private readonly Dictionary<string, CompilerResults> _compiledExpressions;

        private readonly object _concurrencyLock = new object();

        public CachingDynamicExpressionEvaluator(params ICodeRefiner[] refinements)
            : base(refinements)
        {
            _md5Hasher = System.Security.Cryptography.MD5.Create();
            _compiledExpressions = new Dictionary<string, CompilerResults>();
        }

        internal override CompilerResults CompileClass(string classDefinition)
        {
            lock (_concurrencyLock)
            {
                var md5Hash = _md5Hasher.ComputeHash(Encoding.ASCII.GetBytes(classDefinition));
                var smd5Hash = Encoding.ASCII.GetString(md5Hash);

                if (!_compiledExpressions.ContainsKey(smd5Hash))
                {
                    _compiledExpressions.Add(smd5Hash, base.CompileClass(classDefinition));
                }

                return _compiledExpressions[smd5Hash];
            }
        }

        public void ClearCache()
        {
            _compiledExpressions.Clear();
        }
    }

    public class CachingDynamicExpressionEvaluator<TOut, TIn> : GenericDynamicExpressionEvaluator<TOut,TIn>
    {
        private readonly MD5 _md5Hasher;

        /// <summary>
        /// stores an MD5 hash of the class and the compilation object
        /// </summary>
        private readonly Dictionary<string, CompilerResults> _compiledExpressions;

        private readonly object _concurrencyLock = new object();

        public CachingDynamicExpressionEvaluator(params ICodeRefiner[] refinements)
            : base(refinements)
        {
            _md5Hasher = System.Security.Cryptography.MD5.Create();
            _compiledExpressions = new Dictionary<string, CompilerResults>();
        }

        internal override CompilerResults CompileClass(string classDefinition)
        {
            lock (_concurrencyLock)
            {
                var md5Hash = _md5Hasher.ComputeHash(Encoding.ASCII.GetBytes(classDefinition));
                var smd5Hash = Encoding.ASCII.GetString(md5Hash);

                if (!_compiledExpressions.ContainsKey(smd5Hash))
                {
                    _compiledExpressions.Add(smd5Hash, base.CompileClass(classDefinition));
                }

                return _compiledExpressions[smd5Hash];
            }
        }

        public void ClearCache()
        {
            _compiledExpressions.Clear();
        }
    }
}