using MShop.Application.DTOs;
using MShop.Application.Enums;
using MShop.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.SharedFunct
{
    public class Utils
    {
        public static ApiResponse<T> ActionCatch<T>(EnumErrCode errCode, string errDescription, string errDetail)
        {
            return new ApiResponse<T>
            {
                ErrCode = errCode,
                ErrDescription = errDescription,
                ErrDetail = errDetail
            };
        }
    }
}
