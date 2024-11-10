using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.Constants
{
    public class ApiMessage
    {
        // Thông điệp lỗi và thành công cho Product
        public const string ProductNotFound = "No product found with the specified ID";
        public const string ProductRetrievedSuccessfully = "Product retrieved successfully";
        public const string ProductCreatedSuccessfully = "Product created successfully";
        public const string ProductUpdatedSuccessfully = "Product updated successfully";
        public const string ProductDeletedSuccessfully = "Product deleted successfully";
        public const string ProductNotMatch = "No products match";

        public const string ErrorMsg = "An error occurred while processing the request.";
    }
}
