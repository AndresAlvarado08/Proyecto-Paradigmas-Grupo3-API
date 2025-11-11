public class PurchaseDetailDTO
{
    public int Purchase_Detail_Id { get; set; }
    public DateOnly Purchase_Date { get; set; }
    public int User_Id { get; set; }

    public List<DetalleCompraDTO> Detalle_Compra { get; set; }
}

public class DetalleCompraDTO
{
    public int Purchase_Id { get; set; }
    public int Card_Id { get; set; }
    public int Product_Id { get; set; }
    public decimal Product_Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
}
