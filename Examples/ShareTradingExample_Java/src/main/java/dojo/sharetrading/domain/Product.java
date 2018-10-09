package dojo.sharetrading.domain;

public class Product {
    private String productCode;
    private String description;

    public Product()
    {
        super();
    }

    public Product(String ProductCode, String Description)
    {
        productCode = ProductCode;
        description = Description;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getProductCode() {
        return productCode;
    }

    public void setProductCode(String productCode) {
        this.productCode = productCode;
    }
}
