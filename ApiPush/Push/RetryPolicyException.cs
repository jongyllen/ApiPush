using System;

namespace ApiPush.Push
{
    [Serializable]
    public class RetryPolicyException : Exception
    {
        public int RetryCount { get; private set; }

        public RetryPolicyException(int _retryCount, Exception ex)
        {
            RetryCount = _retryCount;
        }

    }
}