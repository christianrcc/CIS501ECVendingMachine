# Customer_Mode Vending Machine

A simple **WPF vending machine application** built in C#.  
This project simulates a real vending machine where users can:

- Insert coins and bills
- Select products (Pepsi, Coke, Funyuns, Cheetos, Twix, Snickers)
- See running total and remaining balance
- Apply a **5% discount** when spending over $20
- Complete transactions, cancel, or request a refund
- See live updates of product quantities and disable items that are sold out or unaffordable

---

## 📸 Features

- **Interactive UI**: Product grid with six `Stock` tiles (name, quantity, Add button)
- **Money Handling**: Buttons for Quarters, Dimes, Nickels, $1, $5, $10
- **Dynamic Cart**: Shows all selected items and their total cost
- **Stock Management**: Quantity updates in real time and disables out-of-stock products
- **Smart Button States**:
  - Finish only enables when balance covers total
  - Refund button disables if no money inserted
  - Add buttons disable if you can't afford that item or if stock is empty
- **Discounts**: 5% discount is automatically applied when spending over $20 and displayed under Total Cost

---

## 🛠 Project Structure

- **UserInterface.xaml / UserInterface.xaml.cs**  
  Main window and logic for UI, money insertion, cart management, and button state control.

- **Stock.xaml / Stock.xaml.cs**  
  UserControl representing a single product tile. Displays product name, quantity, and Add button.

- **Product Classes (`Pepsi`, `Coke`, `Funyuns`, `Cheetos`, `Twix`, `Snickers`)**  
  Each implements `IProduct` with `Name`, `Price`, and `ProductType`.

- **Enums**  
  - `Coins`: `FIVE_CENT`, `TEN_CENT`, `TWENTYFIVE_CENT`
  - `Bills`: `ONE_DOLLAR`, `FIVE_DOLLAR`, `TEN_DOLLAR`

---

## 🚀 Running the Application

1. Open the solution in **Visual Studio**.
2. Build the solution (`Ctrl+Shift+B`).
3. Run (`F5`).
4. Insert money using the coin/bill buttons and select items.

---

## 💰 Discount Logic

- **When Total > $20:**  
  - A 5% discount is applied automatically.
  - The `DiscountText` label appears under Total Cost.
  - `Finish` button uses discounted price to check if balance is enough.
- **When Total ≤ $20:**  
  - Total is full price.
  - DiscountText is hidden.

This logic is handled in `UserInterface.xaml.cs` inside `RecomputeTotals()`.

---

## 🔧 Future Improvements

- Display original total with strikethrough for clearer savings.
- Support giving change in coins/bills rather than just refunding total.
- Add more product types dynamically via a configuration file.

---

## 🖼 Example Screenshot

*(Optional: add a screenshot of your running app here)*

---

## 👨‍💻 Author

Built for **Customer_Mode** project — a simple example of WPF data binding, event handling, and state management in C#.

