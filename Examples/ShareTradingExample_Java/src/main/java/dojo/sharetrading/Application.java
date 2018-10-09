package dojo.sharetrading;

import dojo.sharetrading.domain.*;
import dojo.sharetrading.service.ShareTradingService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.event.ApplicationReadyEvent;
import org.springframework.context.event.EventListener;

import java.io.IOException;

@EnableAutoConfiguration
@SpringBootApplication
public class Application {

    Logger log = LoggerFactory.getLogger(Application.class);

    @Autowired
    ShareTradingService tradingService;

    @EventListener(ApplicationReadyEvent.class)
    public void playGame() throws IOException {
        log.info("Application Started");

        Product[] products = tradingService.availableProducts();
        Price price = tradingService.currentPrice("ProductA");
        AccountDetails accountDetails = tradingService.registerAccount("A java client");
        Purchase purchase = tradingService.buyShares(accountDetails.getAccountNumber(), "ProductA", 10,200);
        Sale sale = tradingService.sellShares(accountDetails.getAccountNumber(), "ProductA", 10,20);
        Transaction[] transactions = tradingService.transactions(accountDetails.getAccountNumber());
        accountDetails = tradingService.AccountDetails(accountDetails.getAccountNumber());
    }
    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }

}
