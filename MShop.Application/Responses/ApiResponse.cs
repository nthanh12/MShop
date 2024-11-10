using MShop.Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.Responses
{
    public class ApiResponse<T>
    {
        public EnumErrCode ErrCode { get; set; }
        public T Data { get; set; }
        public string ErrDescription { get; set; }
        public string ErrDetail { get; set; }
        public int TotalDataRecord { get; set; }

        public ApiResponse()
        {
            ErrCode = EnumErrCode.Error;
            ErrDescription = string.Empty;
            ErrDetail = string.Empty;
            TotalDataRecord = 0;
        }
    }
}
