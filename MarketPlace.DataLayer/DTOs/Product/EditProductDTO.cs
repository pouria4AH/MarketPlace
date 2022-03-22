namespace MarketPlace.DataLayer.DTOs.Product
{
    public class EditProductDTO : CreateProductDTO
    {
        public long Id { get; set; }
        public string ImageName { get; set; }
    }

    public enum EditProductResult
    {
        NotFound,
        NotForUser,
        Success
    }
}
