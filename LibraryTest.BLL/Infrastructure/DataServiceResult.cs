using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manlike.BLL.Infrastructure
{
    public class DataServiceResult
    {
        public DataServiceResult(IList<string> errors)
        {
            Succeeded = false;
            Errors = errors;
        }
        public DataServiceResult(bool success, object data)
        {
            Succeeded = success;
            ResultData = data;
        }
        public DataServiceResult(bool success, object data, IList<string> errors)
        {
            Succeeded = success;
            ResultData = data;
            Errors = errors;
        }

        public object ResultData { get; }
        public bool Succeeded { get; }
        public IList<string> Errors { get; }

        public static DataServiceResult Success() => Success(null);

        public static DataServiceResult Success(object data) => new DataServiceResult(true, data);

        public static DataServiceResult Failed(params string[] errors) => Failed(errors.ToList());

        public static DataServiceResult Failed(IList<string> errors) => new DataServiceResult(errors);

        public static DataServiceResult Failed(object data, params string[] errors) => new DataServiceResult(false, data, errors);

        public static DataServiceResult Failed(IList<IdentityError> errors) => new DataServiceResult(errors.Select(x => x.Description).ToList());

        public static DataServiceResult Failed(object data, IList<string> errors) => new DataServiceResult(false, data, errors);
    }
}
