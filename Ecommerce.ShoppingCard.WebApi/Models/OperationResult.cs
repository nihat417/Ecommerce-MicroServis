﻿namespace Ecommerce.ShoppingCard.WebApi.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public object? Data { get; set; }
    }
}
