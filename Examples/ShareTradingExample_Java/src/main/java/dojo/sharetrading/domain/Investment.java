package dojo.sharetrading.domain;

public class Investment {
    private String productCode;
    private int quantity;

    public Investment() {super();}

    public Investment(String ProductCode, int Quantity)
    {
        productCode = ProductCode;
        quantity = Quantity;
    }

    public int getQuantity() {
        return quantity;
    }

    public void setQuantity(int quantity) {
        this.quantity = quantity;
    }

    public String getProductCode() {
        return productCode;
    }

    public void setProductCode(String productCode) {
        this.productCode = productCode;
    }
}
