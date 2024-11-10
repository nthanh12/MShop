using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.Enums
{
    public enum EnumErrCode
    {
        Error = -1,
        Fail = 0,
        Success = 1,
        Empty = 2,
        NotYetLogin = 3,
        ExistMultiOfUnique = 4,
        DiffrentPass = 5,
        AlreadyExist = 6,
        InvalidEndDate = 7,
        FailUploadImage = 8,
        SuccessWithEmptyTokenFirebase = 9,
        PermissionDenied = 10,
        FailAddNotification = 11,
        DoesNotExist = 12,
        ValidateRequiment = 13,
        SuccessWithFailSomething = 14,
        NotHaveQuotaToRollback = 15
    }
}
