# Password Vault - Admin Whitelist Feature

## Overview
This application implements a password vault system with an admin-controlled whitelist feature. Only whitelisted users can register and access the system.

## Features

### Admin Features
- **Grant User Access**: Add new users to the whitelist with ACE-XXXX format IDs
- **User Management**: View all whitelisted users and revoke access
- **Secure Access**: Role-based authentication for admin functions

### User Features
- **Registration**: Whitelisted users can register with ACE-XXXX format IDs
- **Login**: Secure authentication for registered users

## Database Schema

### whitelisted_users Table
- `id`: Primary key (auto-increment)
- `admin_id`: Foreign key to admins table (int)
- `user_id`: ACE-XXXX format user identifier (varchar)
- `created_at`: Timestamp when user was whitelisted
- `updated_at`: Timestamp of last update
- `access_revoked`: Boolean flag for revoked access

### admins Table
- `id`: Primary key (auto-increment)
- `username`: Admin username
- `password_hash`: Hashed admin password

### app_users Table
- `id`: Primary key (auto-increment)
- `user_id`: ACE-XXXX format user identifier
- `password_hash`: Hashed user password

## Setup Instructions

1. **Database Setup**
   - Run the SQL script in `Database/whitelisted_users.sql`
   - Ensure your connection string is configured in `appsettings.json`

2. **Admin Account Creation**
   - Create an admin user in the `admins` table
   - Use the AuthService to hash the password

3. **Application Configuration**
   - Update connection string in `appsettings.json`
   - Ensure all dependencies are installed

## Usage

### Admin Workflow
1. Login as admin
2. Click the drawer menu (top-left button)
3. Select "Grant User Access" to whitelist new users
4. Select "User Management" to view all whitelisted users
5. Revoke access if needed

### User Registration Workflow
1. Admin whitelists user with ACE-XXXX ID
2. User registers with whitelisted ACE ID
3. User can login and access the system

## Security Features
- Role-based authentication
- Password hashing
- Confirmation required for whitelist actions
- Access revocation capability
- Input validation for ACE ID format

## File Structure
```
Controllers/
  - AdminController.cs          # Admin-specific actions

Models/
  - AuthModels.cs               # View models and entities

Data/
  - WhitelistRepository.cs      # Whitelist data access
  - AdminRepository.cs          # Admin data access

Views/
  - Admin/                      # Admin-specific views
    - _WhitelistedUsersTable.cshtml  # Partial view for user table

wwwroot/js/
  - drawer.js                   # Drawer functionality with whitelist features
```

## API Endpoints

### Admin Controller
- `POST /Admin/WhitelistUser` - Add user to whitelist
- `GET /Admin/WhitelistedUsers` - View all whitelisted users
- `POST /Admin/RevokeAccess` - Revoke user access

## Validation Rules
- ACE ID must match pattern: `^ACE-\d{4}$`
- Confirmation required for whitelist actions
- Unique user IDs enforced at database level

## How It Works

1. **Admin Login** → Access to drawer menu
2. **Grant Access Tab** → Enter ACE-XXXX ID and confirm
3. **Database Storage** → User stored in `whitelisted_users` table with admin_id
4. **User Management Tab** → View all whitelisted users
5. **Access Revocation** → Admins can revoke user access

## Future Enhancements
- Audit logging for admin actions
- Bulk whitelist operations
- User activity monitoring
- Advanced access controls
