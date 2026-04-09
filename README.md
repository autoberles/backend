# 🚗 Autóbérlés Backend API

## 📌 Projekt bemutatása

Ez a projekt egy autóbérlő rendszer backend API-ja, amely lehetővé teszi a felhasználók számára autók böngészését és bérlését, míg az adminisztrátorok teljes körűen kezelhetik a rendszer adatait.

A rendszer ASP.NET Core alapokon készült .NET 8.0 verzióval, Entity Framework Core használatával, MySQL adatbázissal.

### Főbb funkciók:

* 🔐 JWT alapú autentikáció és autorizáció
* 👤 Felhasználókezelés (admin, agent, customer szerepkörök)
* 🚗 CRUD műveletek autókkal, felhasználókkal, telephelyekkel
* 📅 Bérlések kezelése
* 📸 Képfeltöltés autókhoz (admin)
* 📧 Email értesítés sikeres regisztráció esetén
* 🔑 Elfelejtett jelszó visszaállítása emailben küldött kóddal
* 🧾 PDF küldése emailben sikeres bérlést követően

---

## ⚙️ Telepítés és futtatás

### 🔧 Követelmények

A projekt futtatásához az alábbiak szükségesek:

* .NET 8 SDK (a projekt .NET 8.0 framework-öt használ)
* MySQL szerver (futó állapotban)
* Visual Studio

🔍 A telepített .NET verzió ellenőrzéséhez (Visual Studio Terminalban):

dotnet --list-sdks

---

### 📥 Telepítés lépései

1. Repository klónozása a githubról:

git clone https://github.com/autoberles/backend.git

2. Adatbázis létrehozása:

database.sql fájl futtatása dbForge segítségével

3. Projekt futtatása:

Visual Studioban, http kiválasztásával

---

## 👥 Demó felhasználók

| Szerepkör   | Email              | Jelszó     |
| ----------- | -----------------  | ---------  |
| 🛡️ Admin    | tothpista@admin.hu | Tothpista1 |
| 💼 Agent    | kislili@agent.com  | Kislili1   |

A customer felhasználók nincsenek előre seedelve.
Customerként fel lehet regisztrálni, így mindenki saját jelszót állít be.

---

## 🧪 Tesztelés

A projektben lévő végpontokhoz tesztek is készültek.

Futtatás (Visual Studio Terminalban):

cd autoberles_tests
dotnet test --logger "console;verbosity=detailed"

---

## 👨‍💻 A projekt készítői

| Név              | Szerep                    |
| ---------------- | ------------------------- |
| Marquetant Zalán | Frontend fejlesztő        |
| Márton Kristóf   | Backend fejlesztő         |
| Szabó Domonkos   | Mobilalkalmazás fejlesztő |

---
