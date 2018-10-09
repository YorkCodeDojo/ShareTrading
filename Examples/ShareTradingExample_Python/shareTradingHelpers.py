""" APIs and helper methods """
import requests  #python -m pip install requests
import json

# The location of the game server
SERVER_URL = 'http://sharetradingapi.azurewebsites.net/'


# API Calls
def register_account(accountName):
    """ Call to register your team on the share trading server"""
    payload = {'accountName': accountName}
    response = requests.post(SERVER_URL + 'api/accounts', json=payload)
    if response.status_code != 200:
        response.raise_for_status()
    return response.json()


def account_details(accountNumber):
    """ The details of your account"""
    response = requests.get(SERVER_URL + 'api/accounts/' + accountNumber)
    if response.status_code != 200:
        response.raise_for_status()
    return response.json()

def transactions(accountNumber):
    """ The transaction made by your account"""
    response = requests.get(SERVER_URL + 'api/accounts/' + accountNumber + '/transactions')
    if response.status_code != 200:
        response.raise_for_status()
    return response.json()

def available_products():
    """ What is available"""
    response = requests.get(SERVER_URL + 'api/products')
    if response.status_code != 200:
        response.raise_for_status()
    return response.json()

def product_details(productCode):
    """ Current price of a product """
    response = requests.get(SERVER_URL + 'api/products/' + productCode)
    if response.status_code != 200:
        response.raise_for_status()
    return response.json()


def buy_shares(accountNumber, productCode, quantity, maxUnitPrice):
    """Make a purchase """
    payload = {'accountNumber': accountNumber,'productCode':productCode, 'quantity':quantity, 'maxUnitPrice':maxUnitPrice }
    response = requests.post(SERVER_URL + 'api/Purchases', json = payload)
    if response.status_code != 200:
        response.raise_for_status()
    return response.json()
 
def sell_shares(accountNumber, productCode, quantity, minUnitPrice):
    """Make a sale """
    payload = {'accountNumber': accountNumber,'productCode':productCode, 'quantity':quantity, 'minUnitPrice':minUnitPrice }
    response = requests.post(SERVER_URL + 'api/Sales', json = payload)
    if response.status_code != 200:
        response.raise_for_status()
    return response.json()
