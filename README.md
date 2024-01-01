# Firebase Authentication Asp.Net Core

## Overview

This project focuses on handling authentication via Firebase from the server side. It utilizes Firebase UI for generating initial access and refresh tokens. Alternatively, you have the option to use the Firebase JavaScript SDK for the same purpose.

Once the token is generated on the client side, it is sent to the server, which then exchanges it for a fresh access token and an encrypted access token. Additionally, the server clears the IndexedDB where Firebase stores FirebaseIdToken and refreshToken for enhanced security.

## Key Features

- **Firebase Integration**: Utilizes Firebase for server-side authentication.
- **Firebase UI**: Generates initial access and refresh tokens using Firebase UI.(Feel free to use FirebaseJs for the same purpose)
- **Secure Token Exchange**: Server exchanges the FirebaseIdToken and RefreshToken for an encrypted access token and same refresh token.
- **IndexedDB Clearance**: Clears IndexedDB after token exchange for improved privacy.
- **Server Side Token Refresh**: After initial token generation all the subsequent token refresh is handled by server not firebase ui.

## Getting Started

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Installation

1. Clone the repository: `git clone https://github.com/TusharSwarupTandon/FirebaseAuthenticationDotNetCore.git`
2. Navigate to the project folder: `cd FirebaseAuthenticationDotNetCore.Website`
3. Restore dependencies: `dotnet restore`
4. Configure Firebase:
   - Set credentials in `appsettings.json`.

### Usage

1. Run the application: `dotnet run`
2. Access the app in your browser: `http://localhost:7070`

## Configuration

### Project Settings

Ensure to configure your project credentials in the `appsettings.json` file.

```json
{
    "CookieConfig" : {
        "AuthCookieEncryptionKey"  : ""
    },

    "FirebaseAuthConfig" : {
        "ApiKey": "",
        "AuthDomain": "",
        "ProjectId": "",
        "StorageBucket": "",
        "MessagingSenderId": "",
        "AppId": "",
        "MeasurementId": "",
        "ValidAuthority": ""
    },

    "FirebaseServiceAccountConfig" : {
        "Type": "",
        "ProjectId": "",
        "PrivateKeyId": "",
        "PrivateKey": "",
        "ClientEmail": "",
        "ClientId": "",
        "AuthUri": "",
        "TokenUri": "",
        "AuthProviderX509CertUrl": "",
        "ClientX509CertUrl": "",
        "UniverseDomain": ""
    }
}
```
- `CookieConfig:AuthEncryptionKey`: This key is used to encrypt and decrypt JWT when sent from the server.
- `FirebaseAuthConfig`: You can download this from your project's Firebase console by navigating to Project Settings -> General -> Your Apps -> Config.
- `FirebaseServiceAccountConfig`: You need to download this from your Firebase project by going to Project Settings -> Service Accounts -> Generate New Private Key
