package dojo.sharetrading.domain;

import java.util.Date;

public class Transaction {

    public Transaction() {super();}

    public String getID() {
        return ID;
    }

    public void setID(String ID) {
        this.ID = ID;
    }

    public String getAccountNumber() {
        return AccountNumber;
    }

    public void setAccountNumber(String accountNumber) {
        AccountNumber = accountNumber;
    }

    public int getQuantity() {
        return Quantity;
    }

    public void setQuantity(int quantity) {
        Quantity = quantity;
    }

    public Date getTime() {
        return Time;
    }

    public void setTime(Date time) {
        Time = time;
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

    public String getProductCode() {
        return ProductCode;
    }

    public void setProductCode(String productCode) {
        ProductCode = productCode;
    }

    public Transaction(String ID, String AccountNumber, int Quantity, Date Time, int UnitPrice, int TotalValue, String ProductCode)
    {
        this.ID = ID;
        this.AccountNumber = AccountNumber;
        this.Quantity = Quantity;
        this.Time = Time;
        this.UnitPrice = UnitPrice;
        this.TotalValue = TotalValue;
        this.ProductCode = ProductCode;

    }

    private String ID;
    private String AccountNumber;
    private int Quantity;
    private Date Time;
    private int UnitPrice;
    private int TotalValue;
    private String ProductCode;

}
