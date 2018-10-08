package dojo.sharetrading.domain;

public class SellRequest {

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

    public int getMinUnitPrice() {
        return minUnitPrice;
    }

    public void setMinUnitPrice(int maxUnitPrice) {
        this.minUnitPrice = maxUnitPrice;
    }

    private int quantity;
    private int minUnitPrice;

    public SellRequest() {
        super();
    }

    public SellRequest(String AccountNumber, String ProductCode, int Quantity, int MinUnitPrice)
    {
        accountNumber = AccountNumber;
        productCode = ProductCode;
        quantity= Quantity;
        minUnitPrice = MinUnitPrice;
    }
}
