package dojo.sharetrading.domain;

import java.net.ProxySelector;

public class Purchase {

    public Purchase()
    {
        super();
    }

    public Purchase(boolean Success, int Quantity, int UnitPrice, int TotalValue, String Message, String ProductCode, String TransactionID)
    {
        this.Success = Success;
        this.Quantity = Quantity;
        this.UnitPrice = UnitPrice;
        this.TotalValue = TotalValue;
        this.Message = Message;
        this.ProductCode = ProductCode;
        this.TransactionID = TransactionID;
    }

    private boolean Success;
    private int Quantity;
    private int UnitPrice;
    private int TotalValue;
    private String Message;
    private String ProductCode;

    public boolean isSuccess() {
        return Success;
    }

    public void setSuccess(boolean success) {
        Success = success;
    }

    public int getQuantity() {
        return Quantity;
    }

    public void setQuantity(int quantity) {
        Quantity = quantity;
    }

    public int getUnitPrice() {
        return UnitPrice;
    }

    public void setUnitPrice(int unitPrice) {
        UnitPrice = unitPrice;
    }

    public int getTotalValue() {
        return TotalValue;
    }

    public void setTotalValue(int totalValue) {
        TotalValue = totalValue;
    }

    public String getMessage() {
        return Message;
    }

    public void setMessage(String message) {
        Message = message;
    }

    public String getProductCode() {
        return ProductCode;
    }

    public void setProductCode(String productCode) {
        ProductCode = productCode;
    }

    public String getTransactionID() {
        return TransactionID;
    }

    public void setTransactionID(String transactionID) {
        TransactionID = transactionID;
    }

    private String TransactionID;



}
