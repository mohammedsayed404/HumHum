using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared;

namespace Presentation.Mock;


public class MockCartController : Controller
{
    private readonly IServiceManager _serviceManager;

    public MockCartController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }


    public string Testing()
    {
        return $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Confirm Your Email</title>
                    <style>
                        body {{
                            font-family: 'Roboto', sans-serif;
                            line-height: 1.6;
                            color: #333;
                            background-color: #f4f4f4;
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            min-height: 100vh;
                            margin: 0;
                        }}
                        .container {{
                            background-color: #fff;
                            border-radius: 8px;
                            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                            padding: 40px;
                            width: 100%;
                            max-width: 400px;
                            text-align: center;
                        }}
                        h1 {{
                            color: #2c3e50;
                            margin-bottom: 20px;
                        }}
                        button {{
                            background-color: #3498db;
                            color: white;
                            border: none;
                            padding: 12px 20px;
                            border-radius: 4px;
                            cursor: pointer;
                            font-size: 16px;
                            transition: background-color 0.3s;
                        }}
                        button:hover {{
                            background-color: #2980b9;
                        }}
                        .message {{
                            margin-top: 20px;
                            font-weight: bold;
                        }}
                        .success {{
                            color: #2ecc71;
                        }}
                        .error {{
                            color: #e74c3c;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Confirm Your Email</h1>
                        <p>Click the button below to confirm your email address:</p>
                        <button id='confirmButton'>Confirm Email</button>
                        <p id='message' class='message'></p>
                    </div>

                    <script>
                        document.getElementById('confirmButton').addEventListener('click', async () => {{
                            const messageElement = document.getElementById('message');
                            try {{
                                const response = await fetch('/api/Auth/confirm-email', {{
                                    method: 'POST',
                                    headers: {{ 'Content-Type': 'application/json' }},
                                    body: JSON.stringify({{
                                        userId: '',
                                        token: ''
                                    }})
                                }});

                                const result = await response.json();

                                if (response.ok) {{
                                    messageElement.textContent = 'Email confirmed successfully!';
                                    messageElement.className = 'message success';
                                    document.getElementById('confirmButton').style.display = 'none';
                                }} else {{
                                    messageElement.textContent = result.message || 'An error occurred. Please try again.';
                                    messageElement.className = 'message error';
                                }}
                            }} catch (error) {{
                                console.error('Error:', error);
                                messageElement.textContent = 'An unexpected error occurred. Please try again later.';
                                messageElement.className = 'message error';
                            }}
                        }});
                    </script>
                </body>
                </html>
                        ";
    }

    ///          ICartService       ,  IOrderService[Auroize]
    //[min , plus , index , Delete ],   Checkout => Order Controller => Order Service ,

    ///Shop Controller  => Products , Restaurant  , Category , 
    /////Cart Shoping => 




    [HttpGet]
    public IActionResult Create() => View();


    [HttpPost]
    public IActionResult Create(CustomerCartDto cart)
    {

        var res = _serviceManager.CartService.UpdateCustomerCartAsync(cart);



        return RedirectToAction(nameof(ShowCart), new { id = cart.Id });

    }

    [HttpGet]
    public async Task<IActionResult> ShowCart(string id)
    {

        var res = await _serviceManager.CartService.GetCustomerCartAsync(id);



        return View(res);

    }

}
