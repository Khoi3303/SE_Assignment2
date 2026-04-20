namespace WebCoreMVC_Exer4.Models
{
    public class CartItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double UnitAmount { get; set; }
        public double SubTotal => Quantity * UnitAmount;
    }
}