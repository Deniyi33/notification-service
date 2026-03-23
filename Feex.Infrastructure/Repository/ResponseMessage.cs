using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feex.Infrastructure.Repository
{
    public class ResponseDescription
    {
        public static string DuplicateMessage = "Details already exist";
        public static string SuccessMessage = "success";
        public static string NoRecordFound = "no record found";
        public static string InternalErrorMessage = "Unexpected error encountered! ";
        public static string GeneralErrorMessage = "Sorry, this request could not be completed";
    }
}
