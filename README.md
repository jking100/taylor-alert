# Taylor API Documentation

<BASE_URL>: [tayloremailerfunction20250218182058.azurewebsites.net](https://tayloremailerfunction20250218182058.azurewebsites.net)

## Authentication
All endpoints except the root require authentication using the `x-functions-key` header. Set the value of this header to your API key.

## Endpoints

### Root Endpoint

- URL: `/`
- Method: ANY 
- Description: Returns an HTML page. Can be used to verify the API is live.
- Request Body: None
- Response: HTML page

### Emailer Endpoint

- URL: `/api/emailer`  
- Method: POST
- Description: Sends a basic email notification to your personal Gmail. Accepts an optional message parameter to append to the stock notification email.
- Request Headers: 
  - `content-type`: Must be set to `application/json`
  - `x-functions-key`: Set to your API key  
- Request Body (optional): 
  - Format: JSON
  - Parameters:
    - `msg` (string, optional): Additional message to append to the email  
- Response: If successful, HTTP response with copy of the full message sent, otherwise error message 
- Side effects: Succesful execution sends an email to your personal Gmail with subject line "Notification - Script Execution Finished"

## Example cURL Commands cuz I'm nice

Send a notification email with an additional message:

```
curl -X POST `
  '<BASE_URL>/api/emailer' `
  -H 'content-type: application/json' `
  -H 'x-functions-key: <KEY>' `  
  -d '{"msg":"Additional message content"}'
```

Send a notification email with the stock message only:

```  
curl -X POST `
  '<BASE_URL>/api/emailer' `
  -H 'content-type: application/json' `
  -H 'x-functions-key: <KEY>'
```

### Proof it works since I know you're going to give me a hard time cuz you'll type the connection incorrectly or something

```
PS C:\Users\Joe> curl -X POST `
>>   '<BASE_URL>/api/emailer' `
>>   -H 'content-type: application/json' `
>>   -H 'x-functions-key: <KEY>'
<h1>Script is done</h1><p>choke or no choke? you will know soon</p><hr><h3></h3>

PS C:\Users\Joe> curl -X POST `
>>   '<BASE_URL>/api/emailer' `
>>   -H 'content-type: application/json' `
>>   -H 'x-functions-key: <KEY>' `
>>   -d '{"msg":"Additional message content"}'
<h1>Script is done</h1><p>choke or no choke? you will know soon</p><hr><h3>Additional message content</h3>
```
