# Trade and Sell

## Table of Contents
1. [Introduction](#introduction)
2. [Problem Statement](#problem-statement)
3. [Solution](#solution)
4. [Development Tools and Technologies](#development-tools-and-technologies)
5. [Instructions to Execute the Code](#instructions-to-execute-the-code)
6. [Outstanding Issues and Future Plans](#outstanding-issues-and-future-plans)
7. [References](#references)

## Introduction
The "Trade and Sell" web application is designed to help college and university students trade items with each other. The platform allows students to view item details, check prices, and communicate with sellers or buyers. It also enables students to find items locally, avoiding shipping issues. This application aims to provide a convenient and secure way for students to trade and sell items such as used books, electronics, and furniture.

## Problem Statement
Many students face budget constraints and cannot afford to buy new books, electronics, or furniture. Although there are online forums for trading items, they are not widely known and lack formal trading rules, leading to failed trades. The "Trade and Sell" app addresses this gap by offering a dedicated platform where students can trade items confidently and conveniently.

## Solution
The application offers a range of features to facilitate the trading and selling of items among students:

### Key Features:
- **Authentication System:** Users can register and log in securely.
- **View Items:** Users can browse and search for items available for trade or sale.
- **Post Items:** Users can post items for trade or sale, including details like pictures, condition, and price.
- **Trade Items:** Users can propose trades and negotiate terms.
- **Messaging System:** Users can communicate with each other through messages.
- **Admin Panel:** Administrators can manage users, items, and monitor site usage.

## Development Tools and Technologies
The "Trade and Sell" web application was developed using the following tools and technologies:

### Development Tools:
- **Visual Studio Community 2019:** Used as the primary development environment.
- **GitHub:** For version control and collaboration.

### Technologies and Frameworks:
- **ASP.NET Core:** The primary framework for building the web application.
- **HTML5 / CSS3 with Bootstrap:** For front-end development.
- **JavaScript with jQuery:** For client-side scripting.
- **Microsoft SQL Server:** For database management.
- **Azure Server Hosting:** For deploying the web application.

### Packages and Libraries:
- **Stripe Payment System:** Integrated for handling payments (deprecated in this version).

## Instructions to Execute the Code
To set up and run the "Trade and Sell" application, follow these steps:

1. **Download and Install Visual Studio:**
   - Run the Visual Studio Installer.
   - Select "ASP.NET and web development" and ".NET desktop development" and click install.

2. **Clone the Repository:**
   - Clone the project repository from GitHub using the following URL:
     ```
     https://github.com/aidenliw/TradeAndSell
     ```

3. **Open the Project:**
   - Open the `TradeAndSell` project in Visual Studio.
   - Change the configuration of the project to release mode.
   - Build the solution if necessary.

4. **Setup Database:**
   - Configure the connection string in the `appsettings.json` file to point to your SQL Server instance.

5. **Run the Project:**
   - Run the project, and the application will be accessible through the local server.

## Outstanding Issues and Future Plans
### Outstanding Issues:
- **Password Reset:** The functionality for password resets via email is currently not available due to issues with API keys. Approval from the IT department is pending.
- **Deprecated Features:** Some features like Stripe payment and reporting bad sellers were deprecated to simplify the application.

### Future Plans:
- **Mobile Accessibility:** Enhance the user interface to support mobile devices.
- **Analytics:** Provide administrators with analytical data on user activity, such as the number of transactions and most traded items.
- **Improved User Experience:** Simplify the registration and login process, and improve the performance of large data uploads.

## References
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Bootstrap Documentation](https://getbootstrap.com/docs/4.5/getting-started/introduction/)
- [Stripe API Documentation](https://stripe.com/docs/api)
- [Azure Documentation](https://docs.microsoft.com/en-us/azure/)
- [GitHub Repository](https://github.com/aidenliw/TradeAndSell)
- [Live Project URL](https://tradeandsell.azurewebsites.net/)
- [Video Demonstration on YouTube](https://www.youtube.com/watch?v=bVzwyTVLXFM)
