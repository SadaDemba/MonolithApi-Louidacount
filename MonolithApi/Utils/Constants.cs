namespace MonolithApi.Utils
{
    public class Constants
    {
        /// <summary>
        /// Exception messages for not found objects
        /// </summary>
        public const string ADDRESS_NOT_FOUND = "Adress not found !!!";
        public const string PRODUCT_NOT_FOUND = "Produit not found !!!";
        public const string PRODUCT_TYPE_NOT_FOUND = "Product type not found !!!";
        public const string SHOP_NOT_FOUND = "Shop not found !!!";
        public const string REDUCTION_NOT_FOUND = "Reduction not found !!!";
        public const string PRODUCT_REDUCTION_NOT_FOUND = "Product reduction not found !!!";


        /// <summary>
        /// Exception messages for object which already exists
        /// </summary>
        public const string ADDRESS_EXIST = "Address already exists !!!";
        public const string PRODUCT_EXIST = "Product already exists !!!";
        public const string PRODUCT_TYPE_EXIST = "Product type already exists !!!";
        public const string SHOP_EXIST = "Shop name already exists !!!";
        public const string REDUCTION_EXIST = "Reduction already exists for this!!!";

        /// <summary>
        /// Other Exception messages
        /// </summary>
        public const string INVALID_ID = "Id invalide !!!";
        public const string INVALID_VALUES = "Invalid values filled !!!";
        public const string DEPENDENCY_ERROR = "Dependency error: Delete dependent objects first !!!";
        public const string DEPENDENCY_ERROR_BIS = "Deactivate product reduction first !!!";
        public const string ACTION_FORBIDDEN = "Action forbidden for this user !!!";
        public const string CURRENT_STATE = "Already in this state";
    }
}
