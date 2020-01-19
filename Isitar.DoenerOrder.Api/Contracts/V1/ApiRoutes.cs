namespace Isitar.DoenerOrder.Api.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Suppliers
        {
            private const string SupplierBase = Base + "/suppliers";
            public const string GetAll = SupplierBase;
            public const string Get = SupplierBase + "/{supplierId}";

            public const string Create = SupplierBase;
            public const string Update = SupplierBase + "/{supplierId}";
            public const string Delete = SupplierBase + "/{supplierId}";


            private const string SupplierProductsBase = SupplierBase + "/{supplierId}/products";
            public const string GetAllProducts = SupplierProductsBase;
            public const string GetProduct = SupplierProductsBase + "/{productId}";
            
            public const string CreateProduct = SupplierProductsBase;
            public const string UpdateProduct = SupplierProductsBase + "/{productId}";
            public const string DeleteProduct = SupplierProductsBase + "/{productId}";

            private const string ProductIngredientsBase = SupplierProductsBase + "/{productId}/ingredients";
            public const string GetAllProductIngredients = ProductIngredientsBase;
            public const string GetProductIngredients = ProductIngredientsBase + "/{ingredientId}";
            
            public const string AddProductIngredients = ProductIngredientsBase;
            public const string UpdateProductIngredients = ProductIngredientsBase + "/{ingredientId}";
            public const string DeleteProductIngredients = ProductIngredientsBase + "/{ingredientId}";
        }
        
        public static class Ingredients
        {
            private const string IngredientsBase = Base + "/ingredients";
            public const string GetAll = IngredientsBase;
            public const string Get = IngredientsBase + "/{supplierId}";

            public const string Create = IngredientsBase;
            public const string Update = IngredientsBase + "/{supplierId}";
            public const string Delete = IngredientsBase + "/{supplierId}";
        }
        
        public static class BulkOrders
        {
            private const string BulkOrdersBase = Base + "/bulk-orders";
            public const string GetAll = BulkOrdersBase;
            public const string Get = BulkOrdersBase + "/{bulkOrderId}";

            public const string Create = BulkOrdersBase;
            public const string Update = BulkOrdersBase + "/{bulkOrderId}";
            public const string Delete = BulkOrdersBase + "/{bulkOrderId}";

            private const string OrdersBase = BulkOrdersBase + "/{bulkOrderId}/orders";
            public const string GetAllOrders = OrdersBase;
            public const string GetOrder = OrdersBase + "/{orderId}";
            
            public const string CreateOrder = OrdersBase;
            public const string UpdateOrder = OrdersBase + "/{orderId}";
            public const string DeleteOrder = OrdersBase + "/{orderId}";

            private const string OrderLinesBase = OrdersBase + "/{orderId}/order-lines";
            public const string GetAllOrderLines = OrderLinesBase;
            public const string GetOrderLine = OrderLinesBase + "/{lineId}";
            
            public const string CreateOrderLine = OrderLinesBase;
            public const string UpdateOrderLine = OrderLinesBase + "/{lineId}";
            public const string DeleteOrderLine = OrderLinesBase + "/{lineId}";
        }
        
        public static class Auth
        {
            public const string Login = Base + "/auth/authenticate";
            
            public const string Register = Base + "/auth/register";
            
            public const string Refresh = Base + "/auth/refresh";
        }
    }
}