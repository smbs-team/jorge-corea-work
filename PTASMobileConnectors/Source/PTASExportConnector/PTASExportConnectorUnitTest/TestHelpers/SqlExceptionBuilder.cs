using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;


namespace PTASExportConnectorUnitTest
{
    class SqlExceptionBuilder
    {
        private int errorNumber;
        private string errorMessage;

        public SqlException Build()
        {
            SqlError error = this.CreateError();
            SqlErrorCollection errorCollection = this.CreateErrorCollection(error);
            SqlException exception = this.CreateException(errorCollection);

            return exception;
        }

        public SqlExceptionBuilder WithErrorNumber(int number)
        {
            this.errorNumber = number;
            return this;
        }

        public SqlExceptionBuilder WithErrorMessage(string message)
        {
            this.errorMessage = message;
            return this;
        }

        private SqlError CreateError()
        {
            var ctors = typeof(SqlError).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            var firstSqlErrorCtor = ctors.FirstOrDefault(
                ctor =>
                ctor.GetParameters().Count() == 8);
            SqlError error = firstSqlErrorCtor.Invoke(
                new object[]
                {
                this.errorNumber,
                new byte(),
                new byte(),
                string.Empty,
                this.errorMessage,
                string.Empty,
                new int(),
                new Exception(),
                }) as SqlError;

            return error;
        }

        private SqlErrorCollection CreateErrorCollection(SqlError error)
        {
            var sqlErrorCollectionCtor = typeof(SqlErrorCollection).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];
            SqlErrorCollection errorCollection = sqlErrorCollectionCtor.Invoke(new object[] { }) as SqlErrorCollection;

            typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(errorCollection, new object[] { error });

            return errorCollection;
        }

        private SqlException CreateException(SqlErrorCollection errorCollection)
        {
            var ctor = typeof(SqlException).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];
            SqlException sqlException = ctor.Invoke(
                new object[]
                { 
                this.errorMessage,
                errorCollection,
                null,
                Guid.NewGuid()
                }) as SqlException;

            return sqlException;
        }
    }
}
