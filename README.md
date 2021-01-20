# Stripe Chekout Session Demo

A demo shows the `Description` in a product will be ignored when using the checkout session ID created at the backend.

## .NET Core

Add Stripe's API keys and a price ID in `netcore/appsettings.json`:

```json
"Stripe": {
  "PublicKey": "/* Your Stripe Publishable API Key */",
  "SecretKey": "/* Your Stripe API Scret */",
  "PriceId": "/* Your Stripe Price ID */"
}
```

Run the project:

```bash
cd netcore
dotnet build
dotnet run
```

Go to [https://localhost:5001/](https://localhost:5001/) to generate a new checkout session.
After that, go to [https://localhost:5001/success/](https://localhost:5001/success/) to see the most recent charge.
