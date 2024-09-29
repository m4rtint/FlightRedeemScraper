# Cathay Pacific Flight Availability Scraper

## Overview

This project is designed to scrape flight availability for redeem tickets specifically for Cathay Pacific. The application helps users check the availability of redeemable flight tickets quickly and efficiently. To ensure the security of our application, we avoid storing sensitive information like API keys directly in the source code or repository. Instead, we use environment variables to manage these credentials.

## Prerequisites

- Windows operating system
- PowerShell or Windows Terminal installed
- Administrator privileges to set environment variables

## Setting Up MailGun API Key

### 1. Open PowerShell as Administrator

1. Press `Win + X` and select `Windows PowerShell (Admin)` or `Windows Terminal (Admin)` if installed.

### 2. Set the Environment Variable

1. In the PowerShell window, execute the following command to set your MailGun API key as a user environment variable. Replace `'your-api-key-here'` with your actual MailGun API key.

    ```powershell
    [System.Environment]::SetEnvironmentVariable('MailGun-ApiKey', 'your-api-key-here', 'User')
    ```

### 3. Verify the Environment Variable

1. To ensure the environment variable is set correctly, you can retrieve it using the following command:

    ```powershell
    $apiKey = [System.Environment]::GetEnvironmentVariable('MailGun-ApiKey', 'User')
    Write-Output $apiKey
    ```

   If the environment variable is set correctly, your API key should be printed in the PowerShell window.

## Why Use Environment Variables?

Storing API keys directly in the source code can lead to security vulnerabilities, such as accidental exposure through version control systems. By storing API keys in environment variables, we can keep our credentials secure and separate from our application logic. This approach helps in maintaining best practices for security and configuration management.

## Accessing the Environment Variable in Your Application

Once you have set the `MailGun-ApiKey` environment variable, you can access it in your Windows Forms application using the following C# code:

```csharp
string apiKey = Environment.GetEnvironmentVariable("MailGun-ApiKey", EnvironmentVariableTarget.User);
