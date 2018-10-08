package dojo.sharetrading.domain;

public class NewAccountRequest {
    private String accountName;

    public NewAccountRequest(String accountName) {
        this.accountName = accountName;
    }

    public String getAccountName() {
        return accountName;
    }
}
