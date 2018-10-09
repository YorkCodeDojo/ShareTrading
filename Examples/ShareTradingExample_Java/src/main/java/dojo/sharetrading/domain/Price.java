package dojo.sharetrading.domain;

import java.util.Date;

public class Price {

    public Price() { super();}

    public Price(String ProductCode, int CurrentUnitCost, Date Time)
    {
        this.CurrentUnitCost = CurrentUnitCost;
        this.ProductCode = ProductCode;
        this.Time = Time;
    }

    private String ProductCode;

    public int getCurrentUnitCost() {
        return CurrentUnitCost;
    }

    public void setCurrentUnitCost(int currentUnitCost) {
        CurrentUnitCost = currentUnitCost;
    }

    public String getProductCode() {
        return ProductCode;
    }

    public void setProductCode(String productCode) {
        ProductCode = productCode;
    }

    public Date getTime() {
        return Time;
    }

    public void setTime(Date time) {
        Time = time;
    }

    private int CurrentUnitCost;
    private Date Time;
}
