namespace Isitar.DoenerOrder.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Suppliers
        {
            public const string SupplierBase = Base + "/suppliers";
            public const string GetAll = SupplierBase;
            public const string Get = SupplierBase + "/{supplierId}";

            public const string Create = SupplierBase;
            public const string Update = SupplierBase + "/{supplierId}";
            public const string Delete = SupplierBase + "/{supplierId}";



            public const string SupplierProductsBase = SupplierBase + "/{supplierId}/products";
            public const string GetAllProducts = SupplierProductsBase;
            public const string GetProduct = SupplierProductsBase + "/{productId}";
            
            public const string AddProduct = SupplierProductsBase;
            public const string UpdateProduct = SupplierProductsBase + "/{productId}";
            public const string DeleteProduct = SupplierProductsBase + "/{productId}";
        }
        
        public static class Auth
        {
            public const string Login = Base + "/auth/authenticate";
            
            public const string Register = Base + "/auth/register";
            
            public const string Refresh = Base + "/auth/refresh";
        }
    }
}