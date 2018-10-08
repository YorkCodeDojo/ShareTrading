package dojo.sharetrading.domain;

import java.util.List;

public class AccountDetails {

    public AccountDetails(String accountNumber,
                          String accountName,
                          int TotalFromTransactions,
                          int OpeningCash,
                          List<Investment> Portfolio) {
        this.accountNumber = accountNumber;
        this.accountName = accountName;
        totalFromTransactions = TotalFromTransactions;
        openingCash = OpeningCash;
        portfolio = Portfolio;
    }

    public AccountDetails()
    {
        super();
    }

    String accountNumber;
    String accountName;
    private int totalFromTransactions;
    private int openingCash;
    private List<Investment> portfolio;

    public String getAccountNumber() {
        return accountNumber;
    }

    public void setAccountNumber(String accountNumber) {
        this.accountNumber = accountNumber;
    }

    public String getAccountName() {
        return accountName;
    }

    public void setAccountName(String accountName) {
        this.accountName = accountName;
    }

    public int getTotalFromTransactions() {
        return totalFromTransactions;
    }

    public void setTotalFromTransactions(int totalFromTransactions) {
        this.totalFromTransactions = totalFromTransactions;
    }

    public int getOpeningCash() {
        return openingCash;
    }

    public void setOpeningCash(int openingCash) {
        this.openingCash = openingCash;
    }

    public List<Investment> getPortfolio() {
        return this.portfolio;
    }

    public void setPortfolio(List<Investment> portfolio) {
        this.portfolio = portfolio;
    }

}
