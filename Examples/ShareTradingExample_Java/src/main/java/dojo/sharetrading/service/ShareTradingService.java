package dojo.sharetrading.service;

import dojo.sharetrading.domain.*;
import org.springframework.http.*;
import org.springframework.http.client.OkHttp3ClientHttpRequestFactory;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

@Service
public class ShareTradingService {

    private final String ROOT_URL = "https://sharetradingapi.azurewebsites.net";
    private OkHttp3ClientHttpRequestFactory requestFactory = new OkHttp3ClientHttpRequestFactory();
    private final RestTemplate restTemplate = new RestTemplate(requestFactory);

    public AccountDetails registerAccount(String accountName){

        HttpEntity<NewAccountRequest> request = new HttpEntity<>(new NewAccountRequest(accountName));
        AccountDetails accountDetails = restTemplate.postForObject(ROOT_URL + "/api/accounts", request, AccountDetails.class);

        return accountDetails;
    }

    public AccountDetails AccountDetails(String accountNumber){

        ResponseEntity<AccountDetails> resp  = restTemplate.getForEntity(ROOT_URL + "/api/Accounts/" +accountNumber, AccountDetails.class);
        return resp.getBody();
    }

    public Transaction[] transactions(String accountNumber){

        ResponseEntity<Transaction[]> resp  = restTemplate.getForEntity(ROOT_URL + "/api/Accounts/" +accountNumber+ "/Transactions", Transaction[].class);
        return resp.getBody();
    }

    public Product[] availableProducts(){

        ResponseEntity<Product[]> resp  = restTemplate.getForEntity(ROOT_URL + "/api/products", Product[].class);
        return resp.getBody();
    }

    public Price currentPrice(String productCode){

        ResponseEntity<Price> resp  = restTemplate.getForEntity(ROOT_URL + "/api/products/" + productCode, Price.class);
        return resp.getBody();
    }

    public Purchase buyShares(String accountNumber, String productCode, int quantity, int maxUnitPrice){

        HttpEntity<BuyRequest> request = new HttpEntity<>(new BuyRequest(accountNumber,productCode,quantity,maxUnitPrice));
        Purchase purchase = restTemplate.postForObject(ROOT_URL + "/api/Purchases", request, Purchase.class);

        return purchase;
    }

    public Sale sellShares(String accountNumber, String productCode, int quantity, int minUnitPrice){

        HttpEntity<SellRequest> request = new HttpEntity<>(new SellRequest(accountNumber,productCode,quantity,minUnitPrice));
        Sale sale = restTemplate.postForObject(ROOT_URL + "/api/Sales", request, Sale.class);

        return sale;
    }

}
