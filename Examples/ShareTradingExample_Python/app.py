""" Example ShareTradingApp written in Python """
from shareTradingHelpers import (register_account, account_details, available_products, product_details, buy_shares, sell_shares, transactions)

# The (unique) name of your team
ACCOUNT_NAME = 'pythonExample'

def displayAccount(account):
    print("Account Number : " + account["accountNumber"])
    print("Account Name : " + account["accountName"])
    print("TotalFromTransactions : " + str(account["totalFromTransactions"]))
    print("OpeningCash : " + str(account["openingCash"]))
    print ("Portfolio:")
    for investment in account["portfolio"]:
        print(investment["productCode"] + " " + str(investment["quantity"]))

def displayProducts(products):
    for product in products:
        print(product["productCode"] + " " + str(product["description"]))

def displayTransactions(my_transactions):
    for transaction in my_transactions:
        print("Product Code : " + transaction["productCode"])
        print("Account Number : " + str(transaction["accountNumber"]))
        print("Time : " + str(transaction["time"]))
        print("TotalValue : " + str(transaction["totalValue"]))
        print("UnitPrice : " + str(transaction["unitPrice"]))
        print("ID : " + str(transaction["id"]))
        print("Quantity : " + str(transaction["quantity"]))        

def displayProduct(product):
    print("Product Code : " + product["productCode"])
    print("Price : " + str(product["currentUnitCost"]))
    print("Time : " + str(product["time"]))

def displayPurchase(purchase):
    print("Product Code : " + purchase["productCode"])
    print("Success : " + str(purchase["success"]))
    print("Message : " + str(purchase["message"]))
    print("TotalValue : " + str(purchase["totalValue"]))
    print("UnitPrice : " + str(purchase["unitPrice"]))
    print("TransactionID : " + str(purchase["transactionID"]))
    print("Quantity : " + str(purchase["quantity"]))

def displaySale(sale):
    print("Product Code : " + sale["productCode"])
    print("Success : " + str(sale["success"]))
    print("Message : " + str(sale["message"]))
    print("TotalValue : " + str(sale["totalValue"]))
    print("UnitPrice : " + str(sale["unitPrice"]))
    print("TransactionID : " + str(sale["transactionID"]))
    print("Quantity : " + str(sale["quantity"]))

def go():
    """ Main logic """

    newAccount = register_account(ACCOUNT_NAME)
    displayAccount(newAccount)
    accountNumber = newAccount["accountNumber"]

    products = available_products()
    displayProducts(products)

    product = product_details("ProductC")
    displayProduct(product)

    purchase = buy_shares(accountNumber, "ProductC", 10, 700)
    displayPurchase(purchase)

    sale = sell_shares(accountNumber, "ProductC", 10, 500)
    displaySale(sale)
    
    my_transactions = transactions(accountNumber)
    displayTransactions(my_transactions)

    amendedAccount = account_details(accountNumber)
    displayAccount(amendedAccount)

go()