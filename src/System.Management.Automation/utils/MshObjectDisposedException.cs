// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Runtime.Serialization;

namespace System.Management.Automation
{
    /// <summary>
    /// This is a wrapper for exception class
    /// <see cref="System.ObjectDisposedException"/>
    /// which provides additional information via
    /// <see cref="System.Management.Automation.IContainsErrorRecord"/>.
    /// </summary>
    /// <remarks>
    /// Instances of this exception class are usually generated by the
    /// PowerShell Engine.  It is unusual for code outside the PowerShell Engine
    /// to create an instance of this class.
    /// </remarks>
    public class PSObjectDisposedException
            : ObjectDisposedException, IContainsErrorRecord
    {
        #region ctor
        /// <summary>
        /// Initializes a new instance of the PSObjectDisposedException class.
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns>Constructed object.</returns>
        /// <remarks>
        /// Per MSDN, the parameter is objectName and not message.
        /// I confirm this experimentally as well.
        /// Also note that there is no parameterless constructor.
        /// </remarks>
        public PSObjectDisposedException(string objectName)
            : base(objectName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PSObjectDisposedException class.
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="message"></param>
        /// <returns>Constructed object.</returns>
        public PSObjectDisposedException(string objectName, string message)
                : base(objectName, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PSObjectDisposedException class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns>Constructed object.</returns>
        public PSObjectDisposedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #region Serialization
        /// <summary>
        /// Initializes a new instance of the PSObjectDisposedException class
        /// using data serialized via
        /// <see cref="System.Runtime.Serialization.ISerializable"/>
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        /// <returns>Constructed object.</returns>
        [Obsolete("Legacy serialization support is deprecated since .NET 8", DiagnosticId = "SYSLIB0051")]
        protected PSObjectDisposedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            throw new NotSupportedException();
        }

        #endregion Serialization
        #endregion ctor

        /// <summary>
        /// Additional information about the error.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// Note that ErrorRecord.Exception is
        /// <see cref="System.Management.Automation.ParentContainsErrorRecordException"/>.
        /// </remarks>
        public ErrorRecord ErrorRecord
        {
            get
            {
                _errorRecord ??= new ErrorRecord(
                    new ParentContainsErrorRecordException(this),
                    _errorId,
                    ErrorCategory.InvalidOperation,
                    null);

                return _errorRecord;
            }
        }

        private ErrorRecord _errorRecord;
        private readonly string _errorId = "ObjectDisposed";
    }
}
