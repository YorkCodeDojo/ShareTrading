package dojo.sharetrading.domain;

public class BuyRequest {

    private String accountNumber;
    private String productCode;

    public String getAccountNumber() {
        return accountNumber;
    }

    public void setAccountNumber(String accountNumber) {
        this.accountNumber = accountNumber;
    }

    public String getProductCode() {
        return productCode;
    }

    public void setProductCode(String productCode) {
        this.productCode = productCode;
    }

    public int getQuantity() {
        return quantity;
    }

    public void setQuantity(int quantity) {
        this.quantity = quantity;
    }

    public int getMaxUnitPrice() {
        return maxUnitPrice;
    }

    public void setMaxUnitPrice(int maxUnitPrice) {
        this.maxUnitPrice = maxUnitPrice;
    }

    private int quantity;
    private int maxUnitPrice;

    public BuyRequest() {
        super();
    }

    public BuyRequest(String AccountNumber, String ProductCode, int Quantity, int MaxUnitPrice)
    {
        accountNumber = AccountNumber;
        productCode = ProductCode;
        quantity= Quantity;
        maxUnitPrice = MaxUnitPrice;
    }
}
